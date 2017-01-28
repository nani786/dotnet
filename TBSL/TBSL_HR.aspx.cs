using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class TBSL_HR : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    SqlDataAdapter sDa;
    DataSet Ds;
    Validate obj = new Validate();

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
                    Response.Redirect("TBSL_Login.aspx", false);
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
            sCmd = new SqlCommand("select employeeid from TBSL_MemberInfo where employeeid=@employeeid and deleted=0", sCon);
            sCmd.Parameters.Add("@employeeid", SqlDbType.VarChar, 30).Value = txtUnlockID.Text.Trim();
            sDr = sCmd.ExecuteReader();
            if (sDr.Read())
            {
                sDr.Close();
                sCmd = new SqlCommand("update TBSL_MemberInfo set locked=0,AdminUnLockDatetime=@AdminUnLockDatetime where employeeid=@employeeid and deleted=0", sCon);
                sCmd.Parameters.Add("@AdminUnLockDatetime", SqlDbType.DateTime).Value = DateTime.Now;
                sCmd.Parameters.Add("@employeeid", SqlDbType.VarChar, 30).Value = txtUnlockID.Text.Trim();
                sCmd.ExecuteNonQuery();

                obj.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator) "
                + "values('" + txtUnlockID.Text.Trim() + "','6','" + Session["Operator"] + "')", sCon);
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
            sCmd = new SqlCommand("select m.EmployeeID,m.MemberName,g.Name as Gender,r.Name as Relation,mi.Password,convert(varchar,m.DateOfBirth,106)DateOfBirth,convert(varchar,m.Age)+' '+a.Name as Age,case when locked=1 then 'Confirmed' else 'Not Confirmed' end 'Status' from TBSL_Members m, AgeType A,Gender G,Relationship r,TBSL_MemberInfo mi where m.AgeTypeID=A.ID and m.Deleted=0 and m.EmployeeID=mi.EmployeeID and m.GenderID=G.ID and m.RelationshipID=r.id and mi.EmployeeID=@employeeid", sCon);
            sCmd.Parameters.Add("@employeeid", SqlDbType.VarChar, 30).Value = txtUnlockID.Text.Trim();
           
            sDa = new SqlDataAdapter(sCmd);
            Ds = new DataSet();
            sDa.Fill(Ds);
            grdEmpDetails.DataSource = Ds.Tables[0];
            grdEmpDetails.DataBind();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                grdEmpDetails.Rows[0].Cells[4].Text = "**********";
            }
            else
            {
                lblError.Text = "Invalid Employee Id.";

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

    protected void btnAdddependent_Click(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            sCmd = new SqlCommand("Select EmployeeID,Logged,Locked from TBSL_MemberInfo where EmployeeID=@EmployeeID and Deleted=0", sCon);
            sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 20).Value = txtUnlockID.Text.Trim();
            sDr = sCmd.ExecuteReader();
            if (sDr.Read())
            {
                Session["EmployeeID"] = sDr["EmployeeID"].ToString();
                Session["Locked"] = sDr["Locked"].ToString();
                sDr.Close();

                obj.InsertTransactions("Insert into TBSL_transactions(EmployeeID,Enrollment_statusID,Operator) "
                    + "values('" + txtUnlockID.Text.Trim() + "',8,'" + Session["Operator"] + "')", sCon);
                //sCmd.ExecuteReader();

                // OldEmployees();

                if (Session["Expire"] != null)
                {
                    if ((string)Session["Expire"] == "Expire")
                        Response.Redirect("TBSL_ExistingMemberDetails.aspx?Type=*@*", false);
                    else
                        Response.Redirect("TBSL_MemberDetails.aspx?Type=*@*", false);
                }
                else
                    Response.Redirect("TBSL_MemberDetails.aspx?Type=*@*", false);

            }
            else
            {
                Master.AlertBox("EmployeeID does not Exist !");
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
        finally
        {
            if (sDr != null)
            {
                if (!sDr.IsClosed)
                    sDr.Close();
            }

            if (sCon.State == ConnectionState.Open)
                sCon.Close();
        }
    }
}