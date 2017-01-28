using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Bally_HR : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    SqlDataAdapter sDa;
    DataSet Ds;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            sCon = new SqlConnection(ConfigurationManager.ConnectionStrings["sLoginCon"].ToString());
            ((LinkButton)Master.FindControl("lbLogout")).Visible = true;

            if (!IsPostBack)
            {
                if (Session["Login"] != "Login")
                {
                    Response.Redirect("Bally_Login.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    protected void BtnUnlock_Click(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            sCmd = new SqlCommand("select employeeid from DemoEnrollment_MEMBERINFO where employeeid=@employeeid and deleted=0", sCon);
            sCmd.Parameters.Add("@employeeid", SqlDbType.VarChar, 30).Value = txtUnlockID.Text.Trim();
            sDr = sCmd.ExecuteReader();
            if (sDr.Read())
            {
                sDr.Close();
                sCmd = new SqlCommand("update DemoEnrollment_MEMBERINFO set locked=0,AdminUnLockDatetime=@AdminUnLockDatetime where employeeid=@employeeid and deleted=0", sCon);
                sCmd.Parameters.Add("@AdminUnLockDatetime", SqlDbType.DateTime).Value = DateTime.Now;
                sCmd.Parameters.Add("@employeeid", SqlDbType.VarChar, 30).Value = txtUnlockID.Text.Trim();
                sCmd.ExecuteNonQuery();

                Master.AlertBox("Employee Details Unlocked Successfully");
            }
            else
            {
                sDr.Close();
                Master.AlertBox("Please enter valid employee ID");
            }

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
        finally
        {
            if (sCon.State == ConnectionState.Open)
                sCon.Close();
        }
    }

    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        try
        {
            sCmd = new SqlCommand("select m.EmployeeID,m.MemberName,g.Name as Gender,r.Name as Relation,mi.Password,convert(varchar,m.DateOfBirth,106)DateOfBirth,convert(varchar,m.Age)+' '+a.Name as Age,case when locked=1 then 'Confirmed' else 'Not Confirmed' end 'Status' from DemoEnrollment_MEMBERS m, AgeType A,Gender G,Relationship r,DemoEnrollment_MEMBERINFO mi where m.AgeTypeID=A.ID and m.Deleted=0 and m.EmployeeID=mi.EmployeeID and m.GenderID=G.ID and m.RelationshipID=r.id and mi.EmployeeID=@employeeid", sCon);
            sCmd.Parameters.Add("@employeeid", SqlDbType.VarChar, 30).Value = txtUnlockID.Text.Trim();
            sDa = new SqlDataAdapter(sCmd);
            Ds = new DataSet();
            sDa.Fill(Ds);

            grdEmpDetails.DataSource = Ds.Tables[0];
            grdEmpDetails.DataBind();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
        finally
        {
            if (sCon.State == ConnectionState.Open)
                sCon.Close();
        }
    }

    protected void grdEmpDetails_DataBound(object sender, EventArgs e)
    {
        int[] gridcell = { 0, 4, 7 };
        for (int rowIndex = grdEmpDetails.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow gvRow = grdEmpDetails.Rows[rowIndex];
            GridViewRow gvPreviousRow = grdEmpDetails.Rows[rowIndex + 1];
            for (int i = 0; i < gridcell.Length; i++)
            {
                int cellCount = gridcell[i];
                if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                {
                    if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                    {
                        gvRow.Cells[cellCount].RowSpan = 2;
                    }
                    else
                    {
                        gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                    }
                    gvPreviousRow.Cells[cellCount].Visible = false;
                }
            }
        }

        #region ForallRows

        //for (int rowIndex = grdEmpDetails.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        //{
        //    GridViewRow gvRow = grdEmpDetails.Rows[rowIndex];
        //    GridViewRow gvPreviousRow = grdEmpDetails.Rows[rowIndex + 1];
        //    for (int cellCount = 0; cellCount < gvRow.Cells.Count; cellCount++)
        //    {
        //        if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
        //        {
        //            if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
        //            {
        //                gvRow.Cells[cellCount].RowSpan = 2;
        //            }
        //            else
        //            {
        //                gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
        //            }
        //            gvPreviousRow.Cells[cellCount].Visible = false;
        //        }
        //    }
        //}

        #endregion
    }
}