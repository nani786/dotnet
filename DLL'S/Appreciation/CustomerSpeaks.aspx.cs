using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class CustomerSpeaks : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    protected void Page_Load(object sender, EventArgs e)
    {
        GetAppreciation();
    }
    private void GetAppreciation()
    {
        sCon = new SqlConnection("user Id=sa;password=111;database=srinu;data source=hydit18");
        SqlDataAdapter sDa;
        DataTable dt = new DataTable();
        sCon.Open();

        try
        {
            sCmd = new SqlCommand("select appreciation from Appreciations where deleted=0");
            sCmd.Connection = sCon;
            sCmd.CommandTimeout = 30;
            sDa = new SqlDataAdapter(sCmd);
            sDa.Fill(dt);
            dlstAppreciation.DataSource = dt;
            dlstAppreciation.DataBind();

        }
        catch (Exception ex)
        {
            //lblmsg.Text = ex.Message;
            //lblmsg.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            if (sCon.State == ConnectionState.Open)
            {
                sCon.Close();
            }
        }

    }
}