using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class ChangePassword : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    Validate obj = new Validate();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        sCon = new SqlConnection(ConfigurationManager.ConnectionStrings["sLoginCon"].ToString());
        ((LinkButton)Master.FindControl("lbLogout")).Visible = true;
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            sCmd = new SqlCommand("select Password from TBSL_Memberinfo where EmployeeID=@EmpID and deleted=0", sCon);
            sCmd.Parameters.Add("@EmpID", SqlDbType.VarChar, 20).Value = Session["EmployeeID"];
            sDr = sCmd.ExecuteReader();
            if (sDr.Read() && txtOldPassword.Text.Trim() == sDr["Password"].ToString())
            {
                sDr.Close();
                sCmd.Dispose();

                UpdatePassword();
            }
            else
            {
                lblError.Text = "Please enter Old Password Correctly";
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

    public void UpdatePassword()
    {
        try
        {
            if (txtConfirmPwd.Text.Trim().Length >= 6 && txtConfirmPwd.Text.Trim().Length <= 10)
            {
                sCmd = new SqlCommand("update TBSL_Memberinfo set Password=@Pwd,ChangePassword=@Passwordchanged where EmployeeID=@EmpID and Deleted=0", sCon);
                sCmd.Parameters.Add("@EmpID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"];
                sCmd.Parameters.Add("@Pwd", SqlDbType.VarChar, 30).Value = txtConfirmPwd.Text.Trim();
                sCmd.Parameters.Add("@Passwordchanged", SqlDbType.Bit).Value = 1;

                int i = sCmd.ExecuteNonQuery();

                obj.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator) values('"
                      + Session["EmployeeID"] + "',14,'" + Session["EmployeeID"] + "')", sCon);

                if (i > 0)
                {
                    if (Session["Expire"] != null)
                    {
                        if ((string)Session["Expire"] == "Expire")
                            Response.Redirect("TBSL_ExpiredMemberDetails.aspx", false);
                        else
                            Response.Redirect("TBSL_MemberDetails.aspx", false);
                    }
                    else
                        Response.Redirect("TBSL_MemberDetails.aspx", false);
                }
                else
                    lblError.Text = "Unable to Update Password. Please Try again";
            }
            else
                lblError.Text = "Password should contain 6 to 10 characters";

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}