using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Results : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeading.Text = ConfigurationManager.AppSettings["Heading"].ToString();
        if (Request.QueryString["MSG"] != null)
        {
            lblError.Text = Request.QueryString["MSG"].ToString();
        }
    }
}