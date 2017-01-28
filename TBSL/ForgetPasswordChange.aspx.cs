using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class ForgetPasswordChange : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    Validate objCon = new Validate();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           lblError.Text = "";          
            sCon = new SqlConnection(ConfigurationManager.ConnectionStrings["sLoginCon"].ToString());
            lblHeading.Text = ConfigurationManager.AppSettings["Heading"].ToString();
            if (!IsPostBack)
            {
                if (Request.QueryString.Count>0)
                {
                    
                    ViewState["EmployeeID"] = objCon.DecryptCode(Request.QueryString["ID"].ToString());
                    ////ViewState["ID"] = DecryptCode("Nw==Q");

                    sCon.Open();
                    sCmd = new SqlCommand("select ForgotPassword from TBSL_MEMBERINFO where EmployeeID=@EmployeeID and Deleted=0", sCon);
                    sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar).Value = ViewState["EmployeeID"].ToString();
                    sDr = sCmd.ExecuteReader();
                    if (sDr.Read())
                    {
                        // ViewState["DataBaseName"] = sDr["DataBaseName"].ToString();

                        if (sDr["ForgotPassword"].ToString().ToUpper() == "FALSE")
                            Response.Redirect("Results.aspx?MSG=This link is invalid as the password is already been reset.", false);
                    }
                    else
                    {
                        Response.Redirect("Results.aspx?MSG=EmployeeID not found.", false);
                    }
                }
                else
                    Response.Redirect("Results.aspx?MSG=EmployeeID not supplied.", false);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Results.aspx?MSG=" + ex.Message, false);
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            sCmd = new SqlCommand("update TBSL_MEMBERINFO set Password=@Password,ForgotPassword=@ForgotPassword where EmployeeID=@EmployeeID and Deleted=0", sCon);
            sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar).Value = ViewState["EmployeeID"].ToString();
            sCmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = txtConfirmPassword.Text.Trim();
            sCmd.Parameters.Add("@ForgotPassword", SqlDbType.VarChar).Value = 0;
            sCmd.ExecuteNonQuery();

            objCon.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator) values('"
                      + ViewState["EmployeeID"].ToString() + "',9,'" + ViewState["EmployeeID"].ToString() + "')", sCon);

            mpePopup.Show();
            
            pnlPasswordChange.Visible = true;
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

    public void AlertBox(string Msg)
    {
        Label lbl = new Label();
        lbl.Text = "";
        lbl.Text = "<script type='text/javascript'>" + Environment.NewLine + "window.alert('" + Msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    protected void lbLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("TBSL_Login.aspx", false);
    }

}