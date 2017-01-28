using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net.Mail;

public partial class TBSL_ExistingMemberDetails : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    SqlDataAdapter sDa;
    DataSet Ds;
    // SqlTransaction sTr;
    Validate obj = new Validate();
    int children;
    string CheckPath;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            //lblErrorPopUp.Text = "";
            sCon = new SqlConnection(ConfigurationManager.ConnectionStrings["sLoginCon"].ToString());
            ((LinkButton)Master.FindControl("lbLogout")).Visible = true;

            if (!IsPostBack)
            {
                Session["Refresh"] = Server.UrlDecode(DateTime.Now.ToString());
                if (Session["Login"] == "Login")
                {
                    TodayDate.Value = DateTime.Now.ToShortDateString();
                    txtEmpDob.Attributes.Add("OnBlur", "getEmpageCurrent(this.form.ContentPlaceHolder1_txtEmpDob,this.form.ContentPlaceHolder1_TodayDate,this.form.ContentPlaceHolder1_EmpAge);");
                    btnSave.Attributes.Add("OnClick", "getEmpageCurrent(this.form.ContentPlaceHolder1_txtEmpDob,this.form.ContentPlaceHolder1_TodayDate,this.form.ContentPlaceHolder1_EmpAge);");
                    //btnAddDependents.Attributes.Add("OnClick", "getageCurrent();");
                    txtGender.Attributes.Add("OnBlur", "genValidate(this.form.ContentPlaceHolder1_ddlRelation,this.form.ContentPlaceHolder1_txtGender);");
                    ddlRelation.Attributes.Add("OnChange", "genValidate(this.form.ContentPlaceHolder1_ddlRelation,this.form.ContentPlaceHolder1_txtGender);");
                    txtAge.Attributes.Add("OnBlur", "getageCurrent(this.form.ContentPlaceHolder1_txtDateOfBirth,this.form.ContentPlaceHolder1_TodayDate,this.form.ContentPlaceHolder1_txtAge,this.form.ContentPlaceHolder1_txtAgeType);");
                    txtDateOfBirth.Attributes.Add("OnBlur", "getageCurrent(this.form.ContentPlaceHolder1_txtDateOfBirth,this.form.ContentPlaceHolder1_TodayDate,this.form.ContentPlaceHolder1_txtAge,this.form.ContentPlaceHolder1_txtAgeType);");
                    txtAgeType.Attributes.Add("OnBlur", "getageCurrent(this.form.ContentPlaceHolder1_txtDateOfBirth,this.form.ContentPlaceHolder1_TodayDate,this.form.ContentPlaceHolder1_txtAge,this.form.ContentPlaceHolder1_txtAgeType);");

                    sCon.Open();
                    sCmd = new SqlCommand("Select m.ID,m.EmployeeID,m.MemberName,m.GenderID,convert(varchar,m.DateOfBirth,103) DateOfBirth, mi.Contactnumber,mi.EmailID,m.Age,mi.EnrollStartDateTime,m.AuditTrail,mi.Audittrail as InfoAuditTrail,mi.TopUP,mi.TopUpPremium,CONVERT(varchar,mi.MarriageDateTime,103) MarriageDateTime from TBSL_Members m,RelationShip R,AgeType A ,Gender G,TBSL_MemberInfo mi where m.RelationShipID=R.ID and m.AgeTypeID=A.ID and m.Deleted=0 and m.EmployeeID=mi.EmployeeID and m.GenderID=G.ID and m.RelationShipID=2 and mi.EmployeeID='" + Session["EmployeeID"] + "'", sCon);
                    sDr = sCmd.ExecuteReader();
                    if (sDr.Read())
                    {
                        txtEmployeeID.Text = sDr["EmployeeID"].ToString();
                        txtEmployeeName.Text = sDr["MemberName"].ToString();
                        ViewState["MainmemberName"] = sDr["MemberName"].ToString();
                        txtEmpDob.Text = sDr["DateOfBirth"].ToString();
                        ddlEmpGender.SelectedValue = sDr["GenderID"].ToString();
                        txtContactNo.Text = sDr["Contactnumber"].ToString();
                        txtEmailID.Text = sDr["EmailID"].ToString();
                        txtMarriageDate.Text = sDr["MarriageDateTime"].ToString();

                        ViewState["EmailID"] = sDr["EmailID"].ToString();
                        EmpAge.Value = sDr["Age"].ToString();
                        ViewState["ID"] = sDr["ID"].ToString();
                        ViewState["EnrollStartDate"] = sDr["EnrollStartDateTime"].ToString();
                        ViewState["EmpAudittrail"] = sDr["AuditTrail"].ToString();
                        ViewState["EmpInfoAudittrail"] = sDr["infoAuditTrail"].ToString();
                        lblWelcomeNote.Text = "Welcome " + sDr["MemberName"].ToString();
                        ViewState["MarriageDateTime"] = sDr["MarriageDateTime"].ToString();
                        //ddlSuminsured.SelectedValue = sDr["TopUP"].ToString();
                        //txtPremium.Text = sDr["Premium"].ToString();

                        ViewState["EmpTransaction"] = "Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,RelationShipID,DateOfBirth,Age,AgeTypeID,"
                       + "Contactnumber,EmailID,MemberName,GenderID) values('" + Session["EmployeeID"].ToString() + "',5,'" + Session["Operator"].ToString() + "',2,'"
                       + obj.DOBConvertion(sDr["DateOfBirth"].ToString()) + "'," + EmpAge.Value + ",1,'" + sDr["Contactnumber"].ToString() + "','"
                       + sDr["EmailID"].ToString() + "','" + sDr["MemberName"].ToString() + "','" + sDr["GenderID"].ToString() + "')";
                    }
                    sDr.Close();
                    LoadMethods();
                    sCon.Close();

                    //if (Session["Locked"].ToString() == "True")
                    //    ControlsDisable();
                }
                else
                    Response.Redirect("TBSL_Login.aspx", false);
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
            if (sCon.State == ConnectionState.Open)
                sCon.Close();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ViewState["PageRefresh"] = Session["Refresh"];
    }

    #region Userdefined Methods

    public void LoadMethods()
    {
        AddRelation();
        OldEmployees();
        GridBind();
        RemoveRelation();
    }

    public void AddRelation()
    {
        ddlRelation.Items.Clear();
        ddlRelation.Items.Add(new ListItem("Select Relation", "0"));
        ddlRelation.Items.Add(new ListItem("Son", "4"));
        ddlRelation.Items.Add(new ListItem("Daughter", "5"));
        ddlRelation.Items.Add(new ListItem("Wife", "18"));
        ddlRelation.Items.Add(new ListItem("Husband", "21"));
        //ddlRelation.Items.Add(new ListItem("Father in law", "12"));
        //ddlRelation.Items.Add(new ListItem("Mother in law", "13"));
        //ddlRelation.Items.Add(new ListItem("Father", "20"));
        //ddlRelation.Items.Add(new ListItem("Mother", "3"));
    }

    public void OldEmployees()
    {
        sDa = new SqlDataAdapter("select m.membername,r.name as Relation,g.name Gender,convert(varchar,m.dateofbirth,103) dateofbirth,m.age,a.name as agetype from TBSL_Members m,TBSL_MemberInfo mi,agetype A,gender G ,relationship r where m.employeeid=mi.employeeid and m.agetypeid=A.id and m.genderid=G.id and m.relationshipid=r.id and mi.deleted=0 and m.deleted=0 and m.relationshipid in(3,4,5,12,13,18,20,21) and m.Confirmed=1 and m.Employeeid='" + Session["EmployeeID"] + "' order by m.id", sCon);
        Ds = new DataSet();
        sDa.Fill(Ds);
        grdOldMemberDetails.DataSource = Ds.Tables[0];
        grdOldMemberDetails.DataBind();

        if (grdOldMemberDetails.Rows.Count > 0)
        {
            for (int i = 0; i < grdOldMemberDetails.Rows.Count; i++)
            {
                if ((grdOldMemberDetails.Rows[i].Cells[1].Text == "Son") || (grdOldMemberDetails.Rows[i].Cells[1].Text == "Daughter"))
                {
                    children = children + 1;
                }
                else if ((grdOldMemberDetails.Rows[i].Cells[1].Text == "Husband") || (grdOldMemberDetails.Rows[i].Cells[1].Text == "Wife"))
                {
                    if (ddlRelation.Items.FindByValue("18") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
                    if (ddlRelation.Items.FindByValue("21") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
                }

                if (children == 2)
                {
                    if (ddlRelation.Items.FindByValue("4") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                    if (ddlRelation.Items.FindByValue("5") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));
                    children = 0;
                }
            }

        }
        else
        {
            lblOldDependentsDetails.Visible = false;
            grdOldMemberDetails.Visible = false;
        }
    }

    public void GridBind()
    {
        sDa = new SqlDataAdapter("select m.ID,m.MemberName,r.Name as Relation,g.Name as Gender, CONVERT(varchar,m.DateOfBirth,103) as DateOfBirth, m.Age,A.Name as AgeType from TBSL_Members m,RelationShip r, Gender g, AgeType A where m.AgeTypeID=a.ID and m.GenderID=g.ID and m.RelationShipID=r.ID and m.Deleted=0 and m.Confirmed=0 and m.RelationShipID in(3,4,5,12,13,18,20,21) and m.EmployeeID='" + Session["EmployeeID"] + "' order by m.ID", sCon);
        Ds = new DataSet();
        sDa.Fill(Ds);
        grdDependentdetails.Columns[0].Visible = true;

        grdDependentdetails.DataSource = Ds.Tables[0];
        grdDependentdetails.DataBind();
        grdDependentdetails.Columns[0].Visible = false;

        if (grdDependentdetails.Rows.Count > 0)
        {
            lblDependentsDetails.Visible = true;
            btnConfirm.Enabled = true;
        }
        else
        {
            lblDependentsDetails.Visible = false;
            btnConfirm.Enabled = false;
        }

        if ((grdOldMemberDetails.Rows.Count + grdDependentdetails.Rows.Count) >= 5)
        {
            txtName.Enabled = false;
            ddlRelation.Enabled = false;
            txtGender.Enabled = false;
            txtDateOfBirth.Enabled = false;
            txtAge.Enabled = false;
            txtAgeType.Enabled = false;
            btnAddDependents.Enabled = false;
        }
        else
        {
            txtName.Enabled = true;
            ddlRelation.Enabled = true;
            txtGender.Enabled = true;
            txtDateOfBirth.Enabled = true;
            txtAge.Enabled = true;
            txtAgeType.Enabled = true;
            btnAddDependents.Enabled = true;
        }
    }

    public void RemoveRelation()
    {

        children = 0;
        string spouse = String.Empty;
        sDa = new SqlDataAdapter("select m.membername,r.name as Relation,g.name Gender,convert(varchar,m.dateofbirth,103)dateofbirth,m.age,a.name agetype from TBSL_Members m,TBSL_MemberInfo mi,agetype A,gender G ,relationship r where m.agetypeid=A.id and m.genderid=G.id and m.relationshipid=r.id and m.deleted=0 and m.employeeid=mi.employeeid and m.relationshipid in(3,4,5,12,13,18,20,21) and m.Employeeid='" + Session["EmployeeID"] + "' order by m.id", sCon);
        Ds = new DataSet();
        sDa.Fill(Ds);

        if (ddlEmpGender.SelectedValue == "1")
        {
            if (ddlRelation.Items.FindByValue("21") != null)
                ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
        }
        else if (ddlEmpGender.SelectedValue == "2")
        {
            if (ddlRelation.Items.FindByValue("18") != null)
                ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
        }

        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if ((Ds.Tables[0].Rows[i][1].ToString() == "Son") || (Ds.Tables[0].Rows[i][1].ToString() == "Daughter"))
                {
                    children = children + 1;
                }
                else if ((Ds.Tables[0].Rows[i][1].ToString() == "Husband") || (Ds.Tables[0].Rows[i][1].ToString() == "Wife"))
                {
                    //spouse = "Added";
                    if (ddlRelation.Items.FindByValue("18") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
                    if (ddlRelation.Items.FindByValue("21") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
                }
                else if ((Ds.Tables[0].Rows[i][1].ToString() == "Father"))
                {
                    if (ddlRelation.Items.FindByValue("20") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("20")));
                    if (ddlRelation.Items.FindByValue("12") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("12")));
                    if (ddlRelation.Items.FindByValue("13") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("13")));
                }
                else if ((Ds.Tables[0].Rows[i][1].ToString() == "Mother"))
                {
                    if (ddlRelation.Items.FindByValue("3") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("3")));
                    if (ddlRelation.Items.FindByValue("12") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("12")));
                    if (ddlRelation.Items.FindByValue("13") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("13")));
                }
                else if ((Ds.Tables[0].Rows[i][1].ToString() == "Father in law"))
                {
                    if (ddlRelation.Items.FindByValue("20") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("20")));
                    if (ddlRelation.Items.FindByValue("12") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("12")));
                    if (ddlRelation.Items.FindByValue("3") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("3")));
                }
                else if ((Ds.Tables[0].Rows[i][1].ToString() == "Mother in law"))
                {
                    if (ddlRelation.Items.FindByValue("3") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("3")));
                    if (ddlRelation.Items.FindByValue("20") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("20")));
                    if (ddlRelation.Items.FindByValue("13") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("13")));
                }
                if (children == 2)
                {
                    if (ddlRelation.Items.FindByValue("4") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                    if (ddlRelation.Items.FindByValue("5") != null)
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));

                }
            }
        }

    }

    public void ModifyRelation(string relation)
    {
        for (int i = 0; i < grdDependentdetails.Rows.Count; i++)
        {
            for (int j = 0; j < ddlRelation.Items.Count; j++)
            {
                if ((relation == "Son") || (relation == "Daughter"))
                {
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Wife") && (ddlRelation.Items[j].Text == "Wife"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
                    }
                    else if ((grdDependentdetails.Rows[i].Cells[2].Text == "Husband") && (ddlRelation.Items[j].Text == "Husband"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father") && (ddlRelation.Items[j].Text == "Father"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("20")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother") && (ddlRelation.Items[j].Text == "Mother"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("3")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father in law") && (ddlRelation.Items[j].Text == "Father in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("12")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother in law") && (ddlRelation.Items[j].Text == "Mother in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("13")));
                    }
                }
                else if (relation == "Wife")
                {
                    if (((grdDependentdetails.Rows[i].Cells[2].Text == "Son") && (ddlRelation.Items[j].Text == "Son")) || ((grdDependentdetails.Rows[i].Cells[2].Text == "Daughter") && (ddlRelation.Items[j].Text == "Daughter")))
                    {
                        children = children + 1;
                    }
                    if (children == 2)
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));
                        children = 0;
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father") && (ddlRelation.Items[j].Text == "Father"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("20")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother") && (ddlRelation.Items[j].Text == "Mother"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("3")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father in law") && (ddlRelation.Items[j].Text == "Father in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("12")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother in law") && (ddlRelation.Items[j].Text == "Mother in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("13")));
                    }
                }
                else if (relation == "Husband")
                {
                    if (((grdDependentdetails.Rows[i].Cells[2].Text == "Son") && (ddlRelation.Items[j].Text == "Son")) || ((grdDependentdetails.Rows[i].Cells[2].Text == "Daughter") && (ddlRelation.Items[j].Text == "Daughter")))
                    {
                        children = children + 1;
                    }
                    if (children == 2)
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));
                        children = 0;
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father") && (ddlRelation.Items[j].Text == "Father"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("20")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother") && (ddlRelation.Items[j].Text == "Mother"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("3")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father in law") && (ddlRelation.Items[j].Text == "Father in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("12")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother in law") && (ddlRelation.Items[j].Text == "Mother in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("13")));
                    }
                }
                else if (relation == "Mother")
                {
                    if (((grdDependentdetails.Rows[i].Cells[2].Text == "Son") && (ddlRelation.Items[j].Text == "Son")) || ((grdDependentdetails.Rows[i].Cells[2].Text == "Daughter") && (ddlRelation.Items[j].Text == "Daughter")))
                    {
                        children = children + 1;
                    }
                    if (children == 2)
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));
                        children = 0;
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father") && (ddlRelation.Items[j].Text == "Father"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("20")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Wife") && (ddlRelation.Items[j].Text == "Wife"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
                    }
                    else if ((grdDependentdetails.Rows[i].Cells[2].Text == "Husband") && (ddlRelation.Items[j].Text == "Husband"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
                    }
                }
                else if (relation == "Father")
                {
                    if (((grdDependentdetails.Rows[i].Cells[2].Text == "Son") && (ddlRelation.Items[j].Text == "Son")) || ((grdDependentdetails.Rows[i].Cells[2].Text == "Daughter") && (ddlRelation.Items[j].Text == "Daughter")))
                    {
                        children = children + 1;
                    }
                    if (children == 2)
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));
                        children = 0;
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother") && (ddlRelation.Items[j].Text == "Mother"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("3")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Wife") && (ddlRelation.Items[j].Text == "Wife"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
                    }
                    else if ((grdDependentdetails.Rows[i].Cells[2].Text == "Husband") && (ddlRelation.Items[j].Text == "Husband"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
                    }
                }
                else if (relation == "Mother in law")
                {
                    if (((grdDependentdetails.Rows[i].Cells[2].Text == "Son") && (ddlRelation.Items[j].Text == "Son")) || ((grdDependentdetails.Rows[i].Cells[2].Text == "Daughter") && (ddlRelation.Items[j].Text == "Daughter")))
                    {
                        children = children + 1;
                    }
                    if (children == 2)
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));
                        children = 0;
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Father in law") && (ddlRelation.Items[j].Text == "Father in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("12")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Wife") && (ddlRelation.Items[j].Text == "Wife"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
                    }
                    else if ((grdDependentdetails.Rows[i].Cells[2].Text == "Husband") && (ddlRelation.Items[j].Text == "Husband"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
                    }
                }
                else if (relation == "Father in law")
                {
                    if (((grdDependentdetails.Rows[i].Cells[2].Text == "Son") && (ddlRelation.Items[j].Text == "Son")) || ((grdDependentdetails.Rows[i].Cells[2].Text == "Daughter") && (ddlRelation.Items[j].Text == "Daughter")))
                    {
                        children = children + 1;
                    }
                    if (children == 2)
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("4")));
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("5")));
                        children = 0;
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Mother in law") && (ddlRelation.Items[j].Text == "Mother in law"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("13")));
                    }
                    if ((grdDependentdetails.Rows[i].Cells[2].Text == "Wife") && (ddlRelation.Items[j].Text == "Wife"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("18")));
                    }
                    else if ((grdDependentdetails.Rows[i].Cells[2].Text == "Husband") && (ddlRelation.Items[j].Text == "Husband"))
                    {
                        ddlRelation.Items.RemoveAt(ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue("21")));
                    }
                }
            }
        }
    }

    public void ClearControls()
    {
        txtName.Text = "";
        ddlRelation.SelectedValue = "0";
        txtGender.Text = "";
        txtDateOfBirth.Text = "";
        txtAge.Text = "";
        txtAgeType.Text = "";
        // txtMarriageDate.Text = "";
        tdtxtmarriage.Visible = false;
        tdmarriage.Visible = false;
        btnAddDependents.Text = "Add Dependent";
    }

    public void ControlsDisable()
    {
        txtEmailID.Enabled = false;
        txtEmployeeName.Enabled = false;
        txtEmpDob.Enabled = false;
        ddlEmpGender.Enabled = false;
        txtContactNo.Enabled = false;
        txtEmailID.Enabled = false;
        btnSave.Enabled = false;
        //txtAccountno.Enabled = false;
        //txtbankname.Enabled = false;
        //txtifsccode.Enabled = false;
        //....................................
        txtName.Enabled = false;
        ddlRelation.Enabled = false;
        txtGender.Enabled = false;
        txtDateOfBirth.Enabled = false;
        txtAge.Enabled = false;
        txtAgeType.Enabled = false;
        btnAddDependents.Enabled = false;
        //..................................
        btnConfirm.Enabled = false;
        grdDependentdetails.Columns[7].Visible = false;
        grdDependentdetails.Columns[8].Visible = false;
    }

    public bool AgeValidation()
    {
        EmpAgeCalculation();

        //DateTime today = DateTime.ParseExact(DateTime.Now, "dd/mm/yyyy", System.Globalization.CultureInfo.CurrentCulture);
        //DateTime depdob = DateTime.ParseExact(txtDob.Text, "dd/mm/yyyy", System.Globalization.CultureInfo.CurrentCulture);
        int Empage = Convert.ToInt32(EmpAge.Value);
        int depAge = Convert.ToInt32(txtAge.Text);
        if ((ddlRelation.SelectedValue == "4") || (ddlRelation.SelectedValue == "5"))
        {
            if (Empage > depAge)
            {
                if (depAge <= 25 || txtAgeType.Text != "Year(s)")
                    return true;
                else
                {
                    Master.AlertBox("Son/Daughter age should be below 25 years");
                    return false;
                }
            }
            else
            {
                if (txtAgeType.Text == "Year(s)")
                {
                    Master.AlertBox("Son/Daughter age should less than Employee age");
                    return false;
                }
                else
                    return true;
            }
        }
        else if ((ddlRelation.SelectedValue == "3") || (ddlRelation.SelectedValue == "20") || (ddlRelation.SelectedValue == "12") || (ddlRelation.SelectedValue == "13"))
        {
            if (Empage < depAge)
            {
                if ((depAge <= 85) && (txtAgeType.Text == "Year(s)"))
                    return true;
                else
                {
                    Master.AlertBox("Father/Mother age should be below 80 years");
                    return false;
                }
            }
            else
            {
                Master.AlertBox("Father/Mother age should more than Employee age");
                return false;

            }
        }
        else if (ddlRelation.SelectedValue == "18")
        {
            if ((depAge < 18) && (txtAgeType.Text == "Year(s)"))
            {
                Master.AlertBox("Wife age should be more than 18 years");
                return false;
            }
            else
            {
                if (txtAgeType.Text != "Year(s)")
                {
                    Master.AlertBox("Wife age should not be in Months/Days");
                    return false;
                }
                else
                    return true;
            }
        }
        else if (ddlRelation.SelectedValue == "21")
        {
            if ((depAge < 21) && (txtAgeType.Text == "Year(s)"))
            {
                Master.AlertBox("Husband age should be be more than 21 years");
                return false;
            }

            else
            {
                if (txtAgeType.Text != "Year(s)")
                {
                    Master.AlertBox("Husband age should not be in Months/Days");
                    return false;
                }
                else
                    return true;
            }
        }
        else
            return true;
    }

    public void EmpAgeCalculation()
    {
        DateTime Dob = DateTime.ParseExact(txtEmpDob.Text, "dd/mm/yyyy", System.Globalization.CultureInfo.CurrentCulture);
        int years;
        // get the difference in years
        years = DateTime.Now.Year - Dob.Year;

        // subtract another year if we're before the
        // birth day in the current year
        if (DateTime.Now.Month < Dob.Month || (DateTime.Now.Month == Dob.Month && DateTime.Now.Day < Dob.Day))
        {
            years--;
        }
        EmpAge.Value = years.ToString();
    }

    public bool CheckDependentDob(DataSet Ds)
    {
        bool flag = true;
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            if (Ds.Tables[0].Rows[i][3].ToString() != "")
                flag = true;
            else
            {
                flag = false;
                break;
            }
        }
        return flag;
    }


    private void SendMail()
    {
        try
        {
            sCon.Open();
            sDa = new SqlDataAdapter("Select m.MemberName,r.name,case when m.GenderID=1 then 'Male' else 'Female' end Gender,convert(varchar,m.DateOfBirth,106) DateOfBirth,convert(varchar, m.Age) +' '+ A.name as Age,mi.TopUp,mi.TopUpPremium from TBSL_Members m,Relationship R,agetype A,TBSL_MemberInfo mi Where m.RelationshipId=R.Id and m.AgeTypeId=A.Id and m.RelationshipId  in (2,3,4,5,12,13,18,20,21)and m.deleted=0 and mi.Deleted=0 and m.EmployeeId='" + Session["EmployeeID"] + "' and mi.employeeid=m.employeeid order by m.id", sCon);
            sDa.SelectCommand.CommandTimeout = 30000;
            Ds = new DataSet();
            sDa.Fill(Ds, "abc");

            if (CheckDependentDob(Ds))
            {
                if (txtEmailID.Text.Trim() != "")
                {
                    string MailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
                    string MailSend = ConfigurationManager.AppSettings["SendMail"].ToString();
                    string SendMailID = ConfigurationManager.AppSettings["SendMailID"].ToString();
                    StringBuilder format = new StringBuilder();
                    format.Append("<table id=table1 width=850px border=0 cellspacing=0 cellpadding=0>");
                    //format.Append("<tr style=color:Navy;>");
                    //format.Append("<td align=left><font size=3;>To</font></td><td></td></tr>");
                    format.Append("<tr><td colspan=2><br></td>");
                    format.Append("<tr style=color:Navy;>");
                    format.Append("<td align=left><font size=3;>");
                    format.Append("Dear  " + ViewState["MainmemberName"].ToString());
                    format.Append("</font></td> <br>");
                    format.Append("<td align=right><font size=3;>");
                    format.Append("Date : " + DateTime.Now);
                    format.Append("</font></td></tr>");
                    format.Append("<tr><td colspan=2><br></td>");
                    format.Append("<tr style=color:Navy;>");
                    format.Append("<td height=20px align=left colspan=2><font size=3;>");
                    format.Append("You have successfully completed the enrollment of your dependents for medical insurance.");//for the Year From 01-Jun-2009 till 31-May-2010
                    format.Append("</font></td></tr>");
                    format.Append("<tr style=color:Navy;>");
                    format.Append("<td height=20px align=left colspan=2><font size=3;>");
                    format.Append("Please find below, the details you have confirmed in the online enrollment tool for your reference:");
                    format.Append("</font></td></tr>");
                    format.Append("</table>");

                    MailMessage msg = new MailMessage();
                    SmtpClient smclt = new SmtpClient();
                    StringBuilder depMembers = new StringBuilder();
                    depMembers.Append("<table id=table2 width=850px border=1 cellspacing=1 cellpadding=1>");
                    depMembers.Append("<thead style=color:black;background-color:#cccccc;font-size:15;>");
                    depMembers.Append("<th width=100px>EmployeeID</th>");
                    depMembers.Append("<th width=200px>Member Name</th>");
                    depMembers.Append("<th width=100px>Relationship</th>");
                    depMembers.Append("<th width=100px>Gender</th>");
                    depMembers.Append("<th width=100px>DateOfBirth</th>");
                    depMembers.Append("<th width=100px>Age</th>");
                    //depMembers.Append("<th width=100px>SumInsured</th>");
                    //if (Ds.Tables[0].Rows[0][5].ToString() != "0") //Commented on 10-06-2015 as per aji requirement.
                    //{
                    //    depMembers.Append("<th width=100px>Top-Up</th>");
                    //    depMembers.Append("<th width=100px>Premium</th>");
                    //}
                    depMembers.Append("</thead>");

                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                        {
                            depMembers.Append("<tr style=color:Navy;>");
                            depMembers.Append("<td align=left><font size=3;>");
                            depMembers.Append(Session["Employeeid"]);
                            depMembers.Append("</font></td>");
                            depMembers.Append("<td align=left><font size=3;>");
                            depMembers.Append(Ds.Tables[0].Rows[i][0].ToString());
                            depMembers.Append("</font></td>");
                            depMembers.Append("<td align=left><font size=3;>");
                            depMembers.Append(Ds.Tables[0].Rows[i][1].ToString());
                            depMembers.Append("</font></td>");
                            depMembers.Append("<td align=left><font size=3;>");
                            depMembers.Append(Ds.Tables[0].Rows[i][2].ToString());
                            depMembers.Append("</font></td>");
                            depMembers.Append("<td align=left><font size=3;>");
                            depMembers.Append(Ds.Tables[0].Rows[i][3].ToString());
                            depMembers.Append("</font></td>");
                            depMembers.Append("<td align=left><font size=3;>");
                            depMembers.Append(Ds.Tables[0].Rows[i][4].ToString());
                            depMembers.Append("</font></td>");
                            //if (i == 0) //Commented on 10-06-2015 as per aji requirement.
                            //{
                            //    //depMembers.Append("<td align=left rowspan=7><font size=3;>");
                            //    //depMembers.Append("5,00,000");
                            //    //depMembers.Append("</font></td>");
                            //    if (Ds.Tables[0].Rows[i][5].ToString() != "0")
                            //    {

                            //        depMembers.Append("<td align=left rowspan=7><font size=3;>");
                            //        depMembers.Append(Ds.Tables[0].Rows[i][5].ToString());
                            //        depMembers.Append("</font></td>");
                            //        depMembers.Append("<td align=left rowspan=7><font size=3;>");
                            //        depMembers.Append(Ds.Tables[0].Rows[i][6].ToString());
                            //        depMembers.Append("</font></td>");
                            //    }
                            //}

                            depMembers.Append("</tr>");
                        }
                        depMembers.Append("</table>");

                        if (MailSend.ToString().ToUpper() == "TRUE")
                        {
                            msg.To.Add(ViewState["EmailID"].ToString());
                            msg.CC.Add("ganeshbabu.ch@fhpl.net");
                            msg.Bcc.Add("Enrollment@fhpl.net");
                        }
                        else
                        {
                            msg.To.Add(SendMailID);
                        }

                        msg.From = new MailAddress("donotreply@fhpl.net");

                        msg.Subject = "Dependents Details";
                        msg.Body = format.ToString() + "<br><br>" + depMembers.ToString() + "<br><br><font size=3 color=#000066><p style=width:750px;>For any concerns please write to ganeshbabu.ch@fhpl.net.<br><br>Regards.<br>FHPL Team</font>";
                        msg.IsBodyHtml = true;
                        smclt.Host = MailServer;
                        // smclt.Host = "200.200.200.244";
                        smclt.Send(msg);

                        sCmd = new SqlCommand("update TBSL_MemberInfo set locked=1,ConfirmDateTime='" + DateTime.Now + "' where employeeid='" + Session["EmployeeID"] + "' and deleted=0", sCon);
                        sCmd.ExecuteNonQuery();

                        sCmd = new SqlCommand("update TBSL_Members set confirmed=1 where employeeid='" + Session["EmployeeID"] + "' and deleted=0", sCon);
                        sCmd.ExecuteNonQuery();

                        obj.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,locked) "
                      + "values('" + Session["EmployeeID"].ToString() + "',1,'" + Session["Operator"].ToString() + "',1)", sCon);

                        Response.Redirect("TBSL_Complete.aspx", false);
                    }
                    else
                    {
                        Master.AlertBox("Error occured. Please try again..");
                    }
                }
                else
                {
                    Master.AlertBox("Invalid Email");
                }
            }
            else
                Master.AlertBox("Please update your dependents date of birth");
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


    private void addDependent()
    {
        if (btnAddDependents.Text == "Add Dependent")
        {
            if (grdDependentdetails.Rows.Count > 5)
            {
                Master.AlertBox("Maximum Dependents Added...");
            }
            else
            {
                // ViewState["AuditTrail"] = txtName.Text.Trim() + "," + ddlRelation.SelectedValue + "," + txtGender.Text.Trim() + "," + txtDateOfBirth.Text.Trim() + "," + txtAge.Text.Trim() + "," + txtAgeType.Text.Trim() + "<>";
                sCmd = new SqlCommand("insert into TBSL_Members (EmployeeID,MemberName,RelationshipID,GenderID,DateOfBirth,"
                    + "Age,AgetypeID,EnrolledDateTime,Confirmed,Deleted) values(@EmployeeID,@MemberName,@RelationshipID,@GenderID,@DateOfBirth,"
                    + "@Age,@AgetypeID,@EnrolledDateTime,@Confirmed,@Deleted)", sCon);

                sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
                sCmd.Parameters.Add("@MemberName", SqlDbType.VarChar, 100).Value = txtName.Text.Trim();
                sCmd.Parameters.Add("@RelationshipID", SqlDbType.Int).Value = Convert.ToInt32(ddlRelation.SelectedValue);
                sCmd.Parameters.Add("@GenderID", SqlDbType.Int).Value = obj.GetGenderCode(txtGender.Text);
                sCmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = Convert.ToDateTime(obj.DOBConvertion(txtDateOfBirth.Text));
                sCmd.Parameters.Add("@Age", SqlDbType.Int).Value = Convert.ToInt32(txtAge.Text.Trim());
                sCmd.Parameters.Add("@AgetypeID", SqlDbType.Int).Value = obj.Agetype(txtAgeType.Text);
                sCmd.Parameters.Add("@EnrolledDateTime", SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now);
                // sCmd.Parameters.Add("@Audittrail", SqlDbType.VarChar, 1000).Value = ViewState["AuditTrail"].ToString();
                sCmd.Parameters.Add("@Confirmed", SqlDbType.Bit).Value = 0;
                sCmd.Parameters.Add("@Deleted", SqlDbType.Bit).Value = 0;
                int i = sCmd.ExecuteNonQuery();

                if ((ddlRelation.SelectedValue == "4") || (ddlRelation.SelectedValue == "5"))
                {
                    obj.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,MemberName,"
                    + "RelationShipID,GenderID,DateOfBirth,Age,AgeTypeID,ChildDOBDateTime) values('" + Session["EmployeeID"].ToString() + "',3,'" + Session["Operator"].ToString()
                    + "','" + txtName.Text.Trim() + "'," + ddlRelation.SelectedValue + "," + obj.GetGenderCode(txtGender.Text)
                    + ",'" + obj.DOBConvertion(txtDateOfBirth.Text) + "'," + txtAge.Text.Trim() + "," + obj.Agetype(txtAgeType.Text) + ",'" + obj.DOBConvertion(txtDateOfBirth.Text) + "')", sCon);
                }
                else if ((ddlRelation.SelectedValue == "18") || (ddlRelation.SelectedValue == "21"))
                {
                    obj.InsertTransactions("Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,MemberName,"
                    + "RelationShipID,GenderID,DateOfBirth,Age,AgeTypeID,MarriageDateTime) values('" + Session["EmployeeID"].ToString() + "',3,'" + Session["Operator"].ToString()
                    + "','" + txtName.Text.Trim() + "'," + ddlRelation.SelectedValue + "," + obj.GetGenderCode(txtGender.Text)
                    + ",'" + obj.DOBConvertion(txtDateOfBirth.Text) + "'," + txtAge.Text.Trim() + "," + obj.Agetype(txtAgeType.Text) + ",'" + obj.DOBConvertion(txtMarriageDate.Text) + "')", sCon);
                }

                if (i > 0)
                    Master.AlertBox("Dependent Details Saved Successfully");
                else
                    Master.AlertBox("Unable to Save Dependent Details..");
            }
        }
        else if (btnAddDependents.Text == "Modify Dependent")
        {
            obj.InsertTransactions(ViewState["DepTransaction"].ToString(), sCon);
            // ViewState["AuditTrail"] = ViewState["AuditTrail"] + ":" + txtName.Text.Trim() + "," + ddlRelation.SelectedValue + "," + txtGender.Text.Trim() + "," + txtDateOfBirth.Text.Trim() + "," + txtAge.Text.Trim() + "," + txtAgeType.Text.Trim() + "<>";
            sCmd = new SqlCommand("update TBSL_Members set membername=@MemberName,relationshipid=@RelationshipID,"
                + "GenderID=@GenderID,DateOfBirth=@DateOfBirth,Age=@Age,AgetypeID=@AgetypeID,"
                + "ModifiedDateTime=@ModifiedDateTime where EmployeeID=@EmployeeID and ID=@ID and Deleted=@Deleted", sCon);

            sCmd.Parameters.Add("@MemberName", SqlDbType.VarChar, 100).Value = txtName.Text.Trim();
            sCmd.Parameters.Add("@RelationshipID", SqlDbType.Int).Value = Convert.ToInt32(ddlRelation.SelectedValue);
            sCmd.Parameters.Add("@GenderID", SqlDbType.Int).Value = obj.GetGenderCode(txtGender.Text.Trim());
            sCmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = Convert.ToDateTime(obj.DOBConvertion(txtDateOfBirth.Text));
            sCmd.Parameters.Add("@Age", SqlDbType.Int).Value = Convert.ToInt32(txtAge.Text.Trim());
            sCmd.Parameters.Add("@AgetypeID", SqlDbType.Int).Value = obj.Agetype(txtAgeType.Text);
            // sCmd.Parameters.Add("@Audittrail", SqlDbType.VarChar, 1000).Value = ViewState["AuditTrail"].ToString();
            sCmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime).Value = DateTime.Now;
            sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
            sCmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(ViewState["DepID"].ToString());
            sCmd.Parameters.Add("@Deleted", SqlDbType.Bit).Value = 0;

            int i = sCmd.ExecuteNonQuery();

            if (i > 0)
                Master.AlertBox("Dependent Details Updated Successfully");
            else
                Master.AlertBox("Unable to Update Dependent Details..");
        }
        Session["Refresh"] = Server.UrlDecode(DateTime.Now.ToString());
        ClearControls();
    }

    public int DateValidationChildMarriage(string startDate, string ChldMrg)
    {
        DateTime ds = Convert.ToDateTime(startDate);
        startDate = ds.ToString("dd/MM/yyyy");

        DateTime Date1 = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
        DateTime Date2 = DateTime.ParseExact(ChldMrg, "dd/MM/yyyy", null);
        TimeSpan TS = Date2 - Date1;

        int d = TS.Days;
        return d;
    }

    public int DateValidation(string startDate)
    {
        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

        //DateTime ds = Convert.ToDateTime(startDate);        
        //startDate = ds.ToString("dd/MM/yyyy");

        DateTime Date1 = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
        DateTime Date2 = DateTime.ParseExact(todayDate, "dd/MM/yyyy", null);
        TimeSpan TS = Date2 - Date1;
        int d = TS.Days;
        return d;
    }

    #endregion

    #region Events


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            obj.InsertTransactions(ViewState["EmpTransaction"].ToString(), sCon);
            // ViewState["EmpAudittrail"] = ViewState["EmpAudittrail"] + ":" + obj.DOBConvertion(txtEmpDob.Text) + "," + EmpAge.Value + "<>";
            //sTr = sCon.BeginTransaction(IsolationLevel.ReadCommitted);
            sCmd = new SqlCommand("update TBSL_Members set  dateofbirth=@DateOfBirth,age=@Age where employeeid=@EmployeeID and id=@ID and deleted=0 ", sCon);
            sCmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = obj.DOBConvertion(txtEmpDob.Text);
            sCmd.Parameters.Add("@Age", SqlDbType.Int).Value = EmpAge.Value;
            //sCmd.Parameters.Add("@Audittrail", SqlDbType.VarChar, 1000).Value = ViewState["EmpAudittrail"].ToString();
            sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
            sCmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(ViewState["ID"].ToString());
            // sCmd.Transaction = sTr;
            sCmd.ExecuteNonQuery();
            // ViewState["EmpInfoAudittrail"] = ViewState["EmpInfoAudittrail"] + ":" + txtContactNo.Text.Trim() + "," + txtEmailID.Text.Trim() + "<>";

            //if (fuCheckForm.HasFile)
            //{
            //    string ext = Path.GetExtension(fuCheckForm.FileName);
            //    CheckPath = Server.MapPath("~/CheckForms/" + txtEmployeeID.Text + "_" + txtEmployeeName.Text + ext);
            //    string Cpath = txtEmployeeID.Text + "_" + txtEmployeeName.Text + ext;
            //    if ((ext.Trim().ToUpper() == ".JPG") || (ext.Trim().ToUpper() == ".JPEG") || (ext.Trim().ToUpper() == ".PDF"))
            //    {
            //        fuCheckForm.SaveAs(CheckPath);

            sCmd = new SqlCommand("update TBSL_MemberInfo set ContactNumber=@ContactNumber,EmailID=@EmailID where employeeid=@EmployeeID and deleted=0", sCon);//,AccountNo=@AccountNo,BankName=@BankName,IFSCCode=@IFSCCode,CancelledCheque=@CancelledCheque
            sCmd.Parameters.Add("@ContactNumber", SqlDbType.VarChar, 10).Value = txtContactNo.Text.Trim();
            sCmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 100).Value = txtEmailID.Text.Trim();
            sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
            //sCmd.Parameters.Add("@Audittrail", SqlDbType.VarChar, 1000).Value = ViewState["EmpInfoAudittrail"].ToString();
            //sCmd.Parameters.Add("@AccountNo", SqlDbType.VarChar, 50).Value = txtAccountno.Text;
            //sCmd.Parameters.Add("@BankName", SqlDbType.VarChar, 50).Value = txtbankname.Text;
            //sCmd.Parameters.Add("@IFSCCode", SqlDbType.VarChar, 11).Value = txtifsccode.Text;
            //sCmd.Parameters.Add("@CancelledCheque", SqlDbType.VarChar, 300).Value = Cpath;


            //sCmd.Transaction = sTr;
            sCmd.ExecuteNonQuery();

            ViewState["EmpTransaction"] = null;

            ViewState["EmpTransaction"] = "Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,RelationShipID,DateOfBirth,Age,AgeTypeID,"
                            + "Contactnumber,EmailID,MemberName,GenderID) values('" + Session["EmployeeID"].ToString() + "',5,'" + Session["Operator"].ToString() + "',2,'"
                            + obj.DOBConvertion(txtEmpDob.Text) + "'," + EmpAge.Value + ",1,'" + txtContactNo.Text.Trim() + "','"
                            + txtEmailID.Text.Trim() + "','" + txtEmployeeName.Text.Trim() + "','" + obj.GetGenderCode(ddlEmpGender.SelectedItem.Text) + "')";
            ViewState["EmailID"] = txtEmailID.Text.Trim();

            // sTr.Commit();
            Master.AlertBox("Member Details Updated Successfully");
            //}
            //else
            //    Master.AlertBox("File should be either Jpg, Jpeg or Pdf only...");
            //}
            //else
            //{
            //    sCmd = new SqlCommand("update TBSL_MemberInfo set ContactNumber=@ContactNumber,EmailID=@EmailID where employeeid=@EmployeeID and deleted=0", sCon);//AccountNo=@AccountNo,BankName=@BankName,IFSCCode=@IFSCCode
            //    sCmd.Parameters.Add("@ContactNumber", SqlDbType.VarChar, 10).Value = txtContactNo.Text.Trim();
            //    sCmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 100).Value = txtEmailID.Text.Trim();
            //    sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
            //   // sCmd.Parameters.Add("@Audittrail", SqlDbType.VarChar, 1000).Value = ViewState["EmpInfoAudittrail"].ToString();
            //    //sCmd.Parameters.Add("@AccountNo", SqlDbType.VarChar, 50).Value = txtAccountno.Text;
            //    //sCmd.Parameters.Add("@BankName", SqlDbType.VarChar, 50).Value = txtbankname.Text;
            //    //sCmd.Parameters.Add("@IFSCCode", SqlDbType.VarChar, 11).Value = txtifsccode.Text;
            //    // sCmd.Parameters.Add("@CheckForm", SqlDbType.VarChar, 300).Value = Cpath;


            //    //sCmd.Transaction = sTr;
            //    sCmd.ExecuteNonQuery();
            //    ViewState["EmailID"] = txtEmailID.Text.Trim();

            //    // sTr.Commit();
            //    Master.AlertBox("Member Details Updated Successfully");
            //}
        }
        catch (Exception ex)
        {
            //sTr.Rollback();
            lblError.Text = ex.Message;
        }
        finally
        {
            if (sCon.State == ConnectionState.Open)
                sCon.Close();
        }
    }

    protected void btnAddDependents_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["Refresh"].ToString() == ViewState["PageRefresh"].ToString())
            {
                sCon.Open();
                if (AgeValidation())
                {
                    if (ddlRelation.SelectedValue == "18" || ddlRelation.SelectedValue == "21")
                    {
                        if (txtMarriageDate.Text != "")
                        {
                            DateTime dt = DateTime.ParseExact(txtMarriageDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                            if (DateTime.Compare(dt, DateTime.Today) <= 0)
                            {
                                int days = 0, days1 = 0;
                                days = DateValidationChildMarriage(ViewState["EnrollStartDate"].ToString(), txtMarriageDate.Text.Trim());
                                days1 = DateValidation(txtMarriageDate.Text.Trim());
                                if (days <= 0)
                                    Master.AlertBox("Spouse cannot be enrolled as the date of intimation exceeds 30days from the date of marriage.");
                                else if (30 - days1 < 0)
                                    Master.AlertBox("Spouse cannot be enrolled as the date of intimation exceeds 30days from the date of marriage.");
                                else
                                {
                                    sCmd = new SqlCommand("update TBSL_MemberInfo set MarriageDateTime='" + obj.DOBConvertion(txtMarriageDate.Text.Trim()) + "' where deleted=0 and employeeid='" + Session["EmployeeID"] + "'", sCon);
                                    sCmd.ExecuteNonQuery();

                                    addDependent();
                                }
                            }
                            else
                                Master.AlertBox("Invalid Date of Marriage");
                        }
                        else
                            Master.AlertBox("Please enter Date of Marriage");
                    }
                    else if (ddlRelation.SelectedValue == "4" || ddlRelation.SelectedValue == "5")
                    {
                        int days = 0, days1 = 0;
                        days = DateValidationChildMarriage(ViewState["EnrollStartDate"].ToString(), txtDateOfBirth.Text.Trim());
                        days1 = DateValidation(txtDateOfBirth.Text.Trim());
                        if (days <= 0)
                            Master.AlertBox("Child cannot be enrolled as the date of intimation exceeds 30days from the date of birth");
                        else if (30 - days1 < 0)
                            Master.AlertBox("Child cannot be enrolled as the date of intimation exceeds 30days from the date of birth");
                        else
                        {
                            sCmd = new SqlCommand("update TBSL_MemberInfo set ChildDOBDateTime='" + obj.DOBConvertion(txtDateOfBirth.Text) + "' where deleted=0 and employeeid='" + Session["EmployeeID"] + "'", sCon);
                            sCmd.ExecuteNonQuery();
                            addDependent();
                        }
                    }
                }
            }
            else
            {
                Master.AlertBox("Page Refreshed.....");
            }
            LoadMethods();

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

    protected void Modify_OnClick(object sender, EventArgs e)
    {
        try
        {
            txtName.Enabled = true;
            ddlRelation.Enabled = true;
            txtGender.Enabled = true;
            txtDateOfBirth.Enabled = true;
            txtAge.Enabled = true;
            txtAgeType.Enabled = true;
            btnAddDependents.Enabled = true;


            string relation = String.Empty;
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
            int index = row.RowIndex;

            ViewState["DepID"] = grdDependentdetails.Rows[index].Cells[0].Text;
            txtName.Text = grdDependentdetails.Rows[index].Cells[1].Text;
            relation = grdDependentdetails.Rows[index].Cells[2].Text;
            txtGender.Text = grdDependentdetails.Rows[index].Cells[3].Text;
            txtDateOfBirth.Text = grdDependentdetails.Rows[index].Cells[4].Text;
            txtAge.Text = grdDependentdetails.Rows[index].Cells[5].Text;
            txtAgeType.Text = grdDependentdetails.Rows[index].Cells[6].Text;
            btnAddDependents.Text = "Modify Dependent";
            // ViewState["AuditTrail"] = grdDependentdetails.Rows[index].Cells[7].Text;
            //..........................................
            ViewState["DepTransaction"] = null;

            if ((relation == "Wife") || (relation == "Husband"))
            {
                tdmarriage.Visible = true;
                tdtxtmarriage.Visible = true;
                txtMarriageDate.Text = ViewState["MarriageDateTime"].ToString();

                ViewState["DepTransaction"] = "Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,MemberName,RelationShipID,GenderID,DateOfBirth,Age,AgeTypeID,MarriageDateTime) "
               + "values('" + Session["EmployeeID"].ToString() + "',4,'" + Session["Operator"].ToString() + "','" + txtName.Text + "'," + obj.GetRelationshipID(relation)
               + "," + obj.GetGenderCode(txtGender.Text) + ",'" + obj.DOBConvertion(txtDateOfBirth.Text) + "'," + txtAge.Text.Trim() + "," + obj.Agetype(txtAgeType.Text) + ",'" + obj.DOBConvertion(txtMarriageDate.Text.Trim()) + "')";
            }
            else
            {
                ViewState["DepTransaction"] = "Insert into TBSL_Transactions(EmployeeID,Enrollment_statusID,Operator,MemberName,RelationShipID,GenderID,DateOfBirth,Age,AgeTypeID,ChildDOBDateTime) "
                + "values('" + Session["EmployeeID"].ToString() + "',4,'" + Session["Operator"].ToString() + "','" + txtName.Text + "'," + obj.GetRelationshipID(relation)
                + "," + obj.GetGenderCode(txtGender.Text) + ",'" + obj.DOBConvertion(txtDateOfBirth.Text) + "'," + txtAge.Text.Trim() + "," + obj.Agetype(txtAgeType.Text) + ",'" + obj.DOBConvertion(txtDateOfBirth.Text) + "')";
            }

            if ((relation == "Son") || (relation == "Daughter"))
            {
                if ((ddlRelation.Items.FindByText("Son") == null) || (ddlRelation.Items.FindByText("Daughter") == null))
                {
                    ddlRelation.Items.Add(new ListItem("Son", "4"));
                    ddlRelation.Items.Add(new ListItem("Daughter", "5"));
                }
            }
            else if (relation == "Wife")
            {
                if (ddlRelation.Items.FindByText("Wife") == null)
                {
                    ddlRelation.Items.Add(new ListItem(relation, "18"));
                }
            }
            else if (relation == "Husband")
            {
                if (ddlRelation.Items.FindByText("Husband") == null)
                {
                    ddlRelation.Items.Add(new ListItem(relation, "21"));
                }
            }
            else if (relation == "Mother")
            {
                if (ddlRelation.Items.FindByText("Mother") == null)
                {
                    ddlRelation.Items.Add(new ListItem(relation, "3"));
                }
            }
            else if (relation == "Father")
            {
                if (ddlRelation.Items.FindByText("Father") == null)
                {
                    ddlRelation.Items.Add(new ListItem(relation, "20"));
                }
            }
            else if (relation == "Father in law")
            {
                if (ddlRelation.Items.FindByText("Father in law") == null)
                {
                    ddlRelation.Items.Add(new ListItem(relation, "12"));
                }
            }
            else if (relation == "Mother in law")
            {
                if (ddlRelation.Items.FindByText("Mother in law") == null)
                {
                    ddlRelation.Items.Add(new ListItem(relation, "13"));
                }
            }
            //..........................................
            ddlRelation.SelectedValue = Convert.ToString(obj.GetRelationshipID(relation));

            ModifyRelation(relation);
        }
        catch (Exception ex)
        {
            Master.AlertBox(ex.Message);
        }
    }

    protected void Delete_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton b = new LinkButton();
            string clientid = ((LinkButton)sender).ClientID.ToString();
            for (int i = 0; i < grdDependentdetails.Rows.Count; i++)
            {
                b = (LinkButton)grdDependentdetails.Rows[i].FindControl("btnDelete");
                if (clientid == b.ClientID.ToString())
                {
                    string id = grdDependentdetails.Rows[i].Cells[0].Text;


                    sCon.Open();
                    sCmd = new SqlCommand("update TBSL_Members set deleted=1,Deleteddatetime='" + DateTime.Now + "' where employeeid='" + Session["EmployeeID"] + "' and id='" + id + "'", sCon);
                    sCmd.ExecuteNonQuery();

                    sCmd = new SqlCommand("update TBSL_Members set deleted=1,Deleteddatetime='" + DateTime.Now + "' where employeeid='" + Session["EmployeeID"] + "' and id='" + id + "'", sCon);
                    sCmd.ExecuteNonQuery();
                }
            }
            LoadMethods();
            ClearControls();
            Master.AlertBox("Dependent Details Deleted Successfully");
        }
        catch (Exception ex)
        {
            Master.AlertBox(ex.Message.Replace("'", " "));
        }
        finally
        {
            if (sCon.State == ConnectionState.Open)
                sCon.Close();
        }


        //try
        //{
        //    sCon.Open();
        //    GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
        //    int index = row.RowIndex;
        //    string id = grdDependentdetails.Rows[index].Cells[0].Text;
        //    sCmd = new SqlCommand("update TBSL_Members set deleted=@Deleted,Deleteddatetime=@Deleteddatetime where EmployeeID=@EmployeeID and ID=@ID", sCon);
        //    sCmd.Parameters.Add("@Deleted", SqlDbType.Bit).Value = 1;
        //    sCmd.Parameters.Add("@Deleteddatetime", SqlDbType.DateTime).Value = DateTime.Now;
        //    sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
        //    sCmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

        //    int j = sCmd.ExecuteNonQuery();

        //    if (j > 0)
        //        Master.AlertBox("Dependent Details Deleted Successfully");
        //    else
        //        Master.AlertBox("Unable to Delete Dependent Details..");


        //    LoadMethods();
        //    ClearControls();
        //}
        //catch (Exception ex)
        //{
        //    lblError.Text = ex.Message;
        //}
        //finally
        //{
        //    if (sCon.State == ConnectionState.Open)
        //        sCon.Close();
        //}
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        //try
        //{

        //    if (ddlSuminsured.SelectedItem.Text == "300000")
        //    {
        //        int i = grdDependentdetails.Rows.Count+grdOldMemberDetails.Rows.Count + 1;
        //        txtPremium.Text = Convert.ToString(Convert.ToInt32(1292) * (Convert.ToInt32(i)));

        //        lblfinalTOPUP.Text = ddlSuminsured.SelectedValue;
        //        lblFinalPremium.Text = txtPremium.Text;

        //    }
        //    else if (ddlSuminsured.SelectedItem.Text == "500000")
        //    {
        //        int i = grdDependentdetails.Rows.Count + grdOldMemberDetails.Rows.Count + 1;
        //        txtPremium.Text = Convert.ToString(Convert.ToUInt32(1629) * (Convert.ToUInt32(i)));
        //        lblfinalTOPUP.Text = ddlSuminsured.SelectedValue;
        //        lblFinalPremium.Text = txtPremium.Text;
        //    }

        //    mpTopUp.PopupControlID = "pnlConfirm";
        //    pnlConfirm.Visible = true;
        //    mpTopUp.Show();

        //}
        //catch (Exception ex)
        //{
        //    lblError.Text = ex.Message;
        //}

        SendMail();
    }

    protected void lbLogout_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("TBSL_Login.aspx");
    }

    //protected void btnParentsYES_Click(object sender, EventArgs e)
    //{
    //    pnlConfirm.Visible = false;
    //    mpTopUp.PopupControlID = "pnlTopup";
    //    pnlTopup.Visible = true;
    //    mpTopUp.Show();
    //}

    //protected void btnParentsNO_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        sCon.Open();
    //        sCmd = new SqlCommand("Update TBSL_MemberInfo set TopUP=@TopUP,Premium=@Premium where employeeid=@EmployeeID and deleted=0", sCon);
    //        sCmd.Parameters.Add("@TopUP", SqlDbType.VarChar, 10).Value = 0;
    //        sCmd.Parameters.Add("@Premium", SqlDbType.VarChar, 10).Value = 0;
    //        sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
    //        sCmd.ExecuteNonQuery();
    //        sCon.Close();
    //        SendMail();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblError.Text = ex.Message;
    //        pnlConfirm.Visible = true;
    //        mpTopUp.Show();
    //    }
    //    finally
    //    {
    //        if (sCon != null)
    //        {
    //            if (sCon.State == ConnectionState.Open)
    //                sCon.Close();
    //        }
    //    }
    //}

    //protected void btnContinue_Click(object sender, EventArgs e)
    //{
    //    pnlTopup.Visible = false;
    //    mpTopUp.PopupControlID = "pnlFinalTopUP";
    //    pnlFinalTopUP.Visible = true;
    //    mpTopUp.Show();

    //}

    //protected void LbBack_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("TBSL_Introduction.aspx", false);
    //}

    //protected void btnTopUpContinue_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ////System.Threading.Thread.Sleep(1000);
    //        sCon.Open();

    //        sCmd = new SqlCommand("Update TBSL_MemberInfo set TopUP=@TopUP,Premium=@Premium where employeeid=@EmployeeID and deleted=0", sCon);
    //        sCmd.Parameters.Add("@TopUP", SqlDbType.VarChar, 200).Value = ddlSuminsured.SelectedValue;
    //        sCmd.Parameters.Add("@Premium", SqlDbType.VarChar, 150).Value = txtPremium.Text.ToString();
    //        sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
    //        sCmd.ExecuteNonQuery();
    //        sCon.Close();
    //        SendMail();

    //    }
    //    catch (Exception ex)
    //    {
    //        mpTopUp.Show();
    //        pnlTopup.Visible = true;
    //        lblfnltopup.Text = ex.Message;
    //    }
    //    finally
    //    {
    //        if (sCon != null)
    //        {
    //            if (sCon.State == ConnectionState.Open)
    //                sCon.Close();
    //        }
    //    }
    //}

    //protected void btnTopUpBack_Click(object sender, EventArgs e)
    //{
    //    mpTopUp.Hide();
    //}

    //protected void ddlSuminsured_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlSuminsured.SelectedValue == "300000")
    //        {
    //            int i = (grdDependentdetails.Rows.Count) + (grdOldMemberDetails.Rows.Count) + 1;
    //            txtPremium.Text = Convert.ToString(Convert.ToUInt32(1292) * (Convert.ToUInt32(i)));
    //            lblfinalTOPUP.Text = ddlSuminsured.SelectedValue;
    //            lblFinalPremium.Text = txtPremium.Text;
    //        }
    //        else if (ddlSuminsured.SelectedValue == "500000")
    //        {
    //            int i = (grdDependentdetails.Rows.Count) + (grdOldMemberDetails.Rows.Count) + 1;
    //            txtPremium.Text = Convert.ToString(Convert.ToUInt32(1629) * (Convert.ToUInt32(i)));
    //            lblfinalTOPUP.Text = ddlSuminsured.SelectedValue;
    //            lblFinalPremium.Text = txtPremium.Text;
    //        }
    //        else
    //            txtPremium.Text = "0";


    //        pnlTopup.Visible = true;
    //        mpTopUp.Show();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorPopUp.Text = ex.Message;
    //        pnlTopup.Visible = true;
    //        mpTopUp.Show();
    //    }
    //}

    protected void ddlRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlRelation.SelectedValue == "18") || (ddlRelation.SelectedValue == "21"))
        {
            tdtxtmarriage.Visible = true;
            tdmarriage.Visible = true;
            txtMarriageDate.Text = "";

        }
        else
        {
            tdtxtmarriage.Visible = false;
            tdmarriage.Visible = false;
            txtMarriageDate.Text = "";
            //  txtMarriageDate.Visible = false;
        }
    }

    #endregion

}