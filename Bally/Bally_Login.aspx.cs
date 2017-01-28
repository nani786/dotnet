using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Bally_Login : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    DataSet Ds;
    SqlDataAdapter sDa;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            sCon = new SqlConnection(ConfigurationManager.ConnectionStrings["sLoginCon"].ToString());

            if (!IsPostBack)
            {
                Session.RemoveAll();
                txtUserName.Focus();
                //Response.Redirect("Bally_Lock.aspx", false);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    protected void btnLogIn_Click(object sender, EventArgs e)
    {
        try
        {
            if ((txtUserName.Text.Trim().ToUpper() == "BALLY") && (txtPassword.Text.Trim() == "BALLY"))
            {
                Session["Login"] = "Login";
                Response.Redirect("Bally_HR.aspx", false);
            }
            else
            {
                sCon.Open();

                sCmd = new SqlCommand("Select EmployeeID,UserID,Password,Logged,Locked from DemoEnrollment_MEMBERINFO where UserID=@UserName and Password=@Password and Deleted=0", sCon);
                sCmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = txtUserName.Text;
                sCmd.Parameters.Add("@Password", SqlDbType.VarChar, 20).Value = txtPassword.Text;
                sDr = sCmd.ExecuteReader();
                if (sDr.Read() && sDr["Password"].ToString() == txtPassword.Text.Trim())
                {
                    Session["EmployeeID"] = sDr["EmployeeID"].ToString();
                    Session["Locked"] = sDr["Locked"].ToString();
                    int log = Convert.ToInt32(sDr["Logged"].ToString()) + 1;
                    sDr.Close();

                   // OldEmployees();

                    sCmd = new SqlCommand("Update DemoEnrollment_MEMBERINFO set Logged=" + log + ",LoginDateTime='" + DateTime.Now.ToString() + "' where EmployeeID='" + Session["EmployeeID"] + "' and Deleted=0", sCon);
                    sCmd.ExecuteNonQuery();

                    Session["Login"] = "Login";

                    if (ViewState["Expire"] == "Expire")
                        Response.Redirect("Bally_ExpiredMemberDetails.aspx", false);
                    else
                    Response.Redirect("Bally_MemberDetails.aspx", false);
                }
                else
                {
                    Master.AlertBox("Invalid User Name/Password");
                }
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

    public void OldEmployees()
    {
        int Days = 0;
        string rel = String.Empty;
        sDa = new SqlDataAdapter("select mi.EnrollStartDateTime,MarriageDateTime,m.DateOfBirth,m.relationshipid,m.EnrolledDateTime,m.id from DemoEnrollment_MEMBERS m,DemoEnrollment_MEMBERINFO mi where mi.deleted=0 and m.deleted=0 and m.employeeid=mi.employeeid and m.Employeeid='" + Session["EmployeeID"] + "' order by m.relationshipid", sCon);
        Ds = new DataSet();
        sDa.Fill(Ds);

        Days = DateValidation(Ds.Tables[0].Rows[0][0].ToString());
        Session["Days"] = 10 - Days;

        if (Convert.ToInt32(Session["Days"]) < 0)
        {
            ViewState["Expire"] = "Expire";
            rel = "2,3,20,12,13";

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if ((Ds.Tables[0].Rows[i][3].ToString() == "18") || (Ds.Tables[0].Rows[i][3].ToString() == "21"))
                {
                    Session["Spouse"] = "Enrolled";
                    Days = 0;
                    if (Ds.Tables[0].Rows[0][1].ToString() != "")
                    {
                        Days = DateValidation(Ds.Tables[0].Rows[0][1].ToString());
                        if (30 - Days < 0)
                        {
                            rel = rel + ",18,21";
                        }
                    }
                    else
                    {
                        rel = rel + ",18,21";
                    }
                }
                else if ((Ds.Tables[0].Rows[i][3].ToString() == "4") || (Ds.Tables[0].Rows[i][3].ToString() == "5"))
                {
                    Days = 0;

                    if (Ds.Tables[0].Rows[i][2].ToString() != "")
                    {
                        Days = DateValidation(Ds.Tables[0].Rows[i][2].ToString());
                        if (30 - Days < 0)
                        {
                            sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set Confirm=1 where employeeid='" + Session["EmployeeID"] + "' and id='" + Ds.Tables[0].Rows[i][5].ToString() + "' and deleted=0", sCon);
                            sCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set Confirm=1 where employeeid='" + Session["EmployeeID"] + "' and id='" + Ds.Tables[0].Rows[i][5].ToString() + "' and deleted=0", sCon);
                        sCmd.ExecuteNonQuery();
                    }
                    //Days = DateValidation(Ds.Tables[0].Rows[i][2].ToString());
                    //if (30 - Days < 0)
                    //{
                    //    sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set Confirmed=1 where employeeid='" + Session["EmployeeID"] + "' and id='" + Ds.Tables[0].Rows[i][5].ToString() + "' and deleted=0", sCon);
                    //    sCmd.ExecuteNonQuery();
                    //}
                }
            }
            sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set Confirm=1 where relationshipid in(" + rel + ") and employeeid='" + Session["EmployeeID"] + "' and deleted=0", sCon);
            sCmd.ExecuteNonQuery();
        }
        else
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if ((Ds.Tables[0].Rows[i][3].ToString() == "18") || (Ds.Tables[0].Rows[i][3].ToString() == "21"))
                {
                    Session["Spouse"] = "Enrolled";
                }
            }
        }

    }

    public int DateValidation(string startDate)
    {
        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
        DateTime ds = Convert.ToDateTime(startDate);
        int m = ds.Month;
        startDate = ds.ToString("dd/MM/yyyy");
        DateTime Date1 = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
        DateTime Date2 = DateTime.ParseExact(todayDate, "dd/MM/yyyy", null);
        TimeSpan TS = Date2 - Date1;
        int d = TS.Days;

        return d;
    }


}
