using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

public partial class TBSL_Login : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                Session.RemoveAll();
                txtUserName.Focus();
                //Response.Redirect("TBSL_Lock.aspx", false);
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
            sCon.Open();
            if ((txtUserName.Text.Trim() == "TBSL") && (txtPassword.Text.Trim() == "TBSL"))
            {
                Session["Login"] = "Login";
                Session["Operator"] = "Admin";
                obj.InsertTransactions("Insert into TBSL_transactions(EmployeeID,Enrollment_statusID,Operator) values('"
                      + Session["Operator"] + "',7,'" + Session["Operator"] + "')", sCon);
                Response.Redirect("TBSL_HR.aspx", false);
            }
            else
            {

                sCmd = new SqlCommand("Select EmployeeID,UserID,Password,Logged,Locked,WindowEndDate from TBSL_MemberInfo where UserID=@UserName and Password=@Password and Deleted=0", sCon);
                sCmd.Parameters.Add("@UserName", SqlDbType.VarChar, 30).Value = txtUserName.Text;
                sCmd.Parameters.Add("@Password", SqlDbType.VarChar, 30).Value = txtPassword.Text;
                sDr = sCmd.ExecuteReader();
                //sDr = sCmd.ExecuteReader();
                if (sDr.Read() && sDr["Password"].ToString() == txtPassword.Text.Trim())
                {
                   // Session["Flag"] = sDr["Flag"].ToString();
                    Session["EmployeeID"] = sDr["EmployeeID"].ToString();
                    Session["Locked"] = sDr["Locked"].ToString();
                    Session["Operator"] = sDr["EmployeeID"].ToString();
                    ViewState["WindowEndDate"] = sDr["WindowEndDate"].ToString();
                    int log = Convert.ToInt32(sDr["Logged"].ToString()) + 1;
                    sDr.Close();


                    obj.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,logged) values('" + Session["EmployeeID"] + "',7,'" + Session["EmployeeID"] + "'," + log + ")", sCon);
                    if (DateTime.Now > Convert.ToDateTime(ViewState["WindowEndDate"].ToString()))
                    {
                       // OldEmployees();
                        sCmd = new SqlCommand("Update TBSL_MemberInfo set Logged=" + log + ",LoginDateTime='" + DateTime.Now.ToString() + "' where EmployeeID='" + Session["EmployeeID"] + "' and Deleted=0", sCon);
                        sCmd.ExecuteNonQuery();
                        Session["Login"] = "Login";
                        //{
                        //    if (Session["Expire"] == "Expire")
                        Response.Redirect("TBSL_Lock.aspx", false);
                        //    else
                        //        Response.Redirect("TBSL_MemberDetails.aspx", false);
                        //}
                    }
                    else
                    {
                        sCmd = new SqlCommand("Update TBSL_MemberInfo set Logged=" + log + ",LoginDateTime='" + DateTime.Now.ToString() + "' where EmployeeID='" + Session["EmployeeID"] + "' and Deleted=0", sCon);
                        sCmd.ExecuteNonQuery();
                        Session["Login"] = "Login";
                        Response.Redirect("TBSL_MemberDetails.aspx", false);

                    }
                }
                else
                {
                    lblError.Text = "Invalid User Name/Password";
                }
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
                {
                    sDr.Close();
                }
            }
            if (sCon != null)
            {
                if (sCon.State == ConnectionState.Open)
                    sCon.Close();
            }
        }
    }

    public void OldEmployees()
    {
        int Days = 0;
        string rel = String.Empty;
        sDa = new SqlDataAdapter("select mi.EnrollStartDateTime,MarriageDateTime,m.DateOfBirth,m.relationshipid,m.EnrolledDateTime,m.id from TBSL_Members m,TBSL_MemberInfo mi where mi.deleted=0 and m.deleted=0 and m.employeeid=mi.employeeid and m.relationshipid in(2,3,4,5,18,20,21) and m.Employeeid='" + Session["EmployeeID"] + "' order by m.relationshipid", sCon);
        Ds = new DataSet();
        sDa.Fill(Ds);

        Days = DateValidation(Ds.Tables[0].Rows[0][0].ToString());
        Session["Days"] = 14 - Days;//15-Days; Telephone discussion by AjiThomas

        if (Convert.ToInt32(Session["Days"]) < 0)
        {
            Session["Expire"] = "Expire";
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
                            sCmd = new SqlCommand("update TBSL_Members set Confirmed=1 where employeeid='" + Session["EmployeeID"] + "' and id='" + Ds.Tables[0].Rows[i][5].ToString() + "' and deleted=0", sCon);
                            sCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        sCmd = new SqlCommand("update TBSL_Members set Confirmed=1 where employeeid='" + Session["EmployeeID"] + "' and id='" + Ds.Tables[0].Rows[i][5].ToString() + "' and deleted=0", sCon);
                        sCmd.ExecuteNonQuery();
                    }
                    //Days = DateValidation(Ds.Tables[0].Rows[i][2].ToString());
                    //if (30 - Days < 0)
                    //{
                    //    sCmd = new SqlCommand("update TBSL_Members set Confirmed=1 where employeeid='" + Session["EmployeeID"] + "' and id='" + Ds.Tables[0].Rows[i][5].ToString() + "' and deleted=0", sCon);
                    //    sCmd.ExecuteNonQuery();
                    //}
                }
            }
            sCmd = new SqlCommand("update TBSL_Members set Confirmed=1 where relationshipid in(" + rel + ") and employeeid='" + Session["EmployeeID"] + "' and deleted=0", sCon);
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
    protected void lbForgotPassword_Click(object sender, EventArgs e)
    {
        mpTopUp.PopupControlID = "pnlConfirm";
        mpTopUp.Show();
        pnlConfirm.Visible = true;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            sCmd = new SqlCommand("Select EmailID from TBSL_MemberInfo where EmployeeID=@EmployeeID and Deleted=0", sCon);
            sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar).Value = txtEmployeeID.Text;
            //sDr = sCmd.ExecuteReader();
            Object result = (string)sCmd.ExecuteScalar();
            //if (!string.IsNullOrEmpty(result))
            if (result != null)
            {
                if (result.ToString() != "")
                {
                    sCmd = new SqlCommand("update TBSL_MemberInfo set ForgotPassword=@ForgotPassword where EmployeeID=@EmployeeID and Deleted=0", sCon);
                    sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = txtEmployeeID.Text.Trim();
                    sCmd.Parameters.Add("@ForgotPassword", SqlDbType.Bit).Value = 1;
                    int res = sCmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        SendMail(result.ToString());
                        //lblError.Text = "An email is sent your registered email id to reset your password. Thank you.";
                        lblError.Text = "You will be receiving an email shortly on your registered email ID to reset your password. Thank you.";

                        obj.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator) values('"
                       + txtEmployeeID.Text + "',10,'" + txtEmployeeID.Text + "')", sCon);
                    }
                }
                else
                {
                    lblPopUpError.Text = "Sorry, Your email ID is not registered with us. Please contact your HR department.";
                    mpTopUp.Show();
                }
            }
            else
            {
                lblPopUpError.Text = "EmployeeID Not Exist";
                mpTopUp.Show();
            }
        }
        catch (Exception ex)
        {
            lblPopUpError.Text = ex.Message;
            mpTopUp.Show();
        }
        finally
        {
            if (sCon != null)
            {
                if (sCon.State != ConnectionState.Open)
                    sCon.Close();
                txtEmployeeID.Text = string.Empty;
            }
        }
    }



    private void SendMail(string ToMailid)
    {
        string link = String.Empty;
        if (ConfigurationManager.AppSettings["Live"].ToString().ToUpper() == "TRUE")
            link = "https://www.fhpl.net/TBSL/ForgetPasswordChange.aspx?ID=" + obj.EncryptData(txtEmployeeID.Text.Trim());
        else
            link = "http://119.226.90.164/TBSL/ForgetPasswordChange.aspx?ID=" + obj.EncryptData(txtEmployeeID.Text.Trim());

        StringBuilder format = new StringBuilder();
        format.Append("<table id=table1 width=750px border=0 cellspacing=0 cellpadding=0>");
        format.Append("<tr style=color:Navy;><td align=left><font size=3;>Dear Member,<br></font></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><br></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><font size=3;>Please click on the below link to get new password<br></font></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><br></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><font size=2;><a href=" + link + ">Click Here</a></font><br></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><br><br></td></tr>");
        format.Append("<tr style=color:Red;><td align=left><font size=2;>OR</font><br></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><br></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><font size=2;>Copy and paste the below link on the browser</font><br></td></tr>");
        format.Append("<tr style=color:Navy;><td align=left><br></td></tr>");
        format.Append("<tr style=color:Blue;><td align=left><font size=2;>" + link + "</font></td></tr>");
        format.Append("</table>");

        obj.SendingMail("Forgot Password: Emp ID: " + txtEmployeeID.Text.Trim() + "", "donotreply@fhpl.net", ToMailid, "", "", "" + format.ToString() + "<br><br><font size=3 color=#000066>Regards,<br>FHPL Team.</font>");

        //obj.SendingMail("donotreply@fhpl.net", txtEmailID.Text.Trim(), "", "", body);
    }
}
