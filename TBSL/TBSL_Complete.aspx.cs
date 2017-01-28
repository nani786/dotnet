using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TBSL_Complete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Login"] == "Login")
        { }
        else
            Response.Redirect("TBSL_Login.aspx", false);
    }
    protected void BtnCompleted_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("TBSL_Login.aspx");
    }
}