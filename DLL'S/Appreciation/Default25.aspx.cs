using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Default25 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
         SqlConnection sCon = new SqlConnection("user Id=sa;password=111;database=srinu;data source=hydit18");
        SqlCommand sCmd = new SqlCommand();
        try
        {
            sCon.Open();
            sCmd.CommandText = "Insert into Appreciations (appreciation,deleted)values(@appreciation,0) ";
            sCmd.Connection = sCon;
            sCmd.CommandType = CommandType.Text;
            sCmd.Parameters.Add("@appreciation", SqlDbType.VarChar).Value = editor.Content;
            sCmd.CommandTimeout = 30;
            sCmd.ExecuteNonQuery();
            //lblmsg.Text = "Health tip Saved successfully";
            //lblmsg.ForeColor = System.Drawing.Color.Green;
            editor.Content = string.Empty;
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