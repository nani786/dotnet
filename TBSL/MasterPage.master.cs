using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeading.Text = ConfigurationManager.AppSettings["Heading"].ToString();
    }

    public void AlertBox(string Msg)
    {
        Label lbl = new Label();
        lbl.Text = "";
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + Msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void lbLogout_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("TBSL_Login.aspx", false);
    }
    protected void lnkChgPwd_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChangePassword.aspx", false);
    }
}
