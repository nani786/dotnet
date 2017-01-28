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

public partial class MemberDetails : System.Web.UI.Page
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    SqlDataAdapter sDa;
    DataSet Ds;
    //SqlTransaction sTr;
    Validate obj = new Validate();

    int children;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            sCon = new SqlConnection(ConfigurationManager.ConnectionStrings["sLoginCon"].ToString());
            ((LinkButton)Master.FindControl("lbLogout")).Visible = true;
            if (!IsPostBack)
            {
                Session["Refresh"] = Server.UrlDecode(DateTime.Now.ToString());
                if (Session["Login"] == "Login")
                {
                    AddRelation();
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
                    sCmd = new SqlCommand("Select m.ID,m.EmployeeID,m.MemberName,m.GenderID,convert(varchar,m.DateOfBirth,103) DateOfBirth, mi.Contactnumber,mi.EmailID,m.Age,m.AuditTrail,mi.Audittrail as InfoAuditTrail,CONVERT(varchar,mi.MarriageDateTime,103) MarriageDateTime,mi.EnrollStartDateTime from DemoEnrollment_MEMBERINFO mi,DemoEnrollment_MEMBERS m,RelationShip R,AgeType A ,Gender G where m.EmployeeID=mi.EmployeeID and m.RelationShipID=R.ID and m.AgeTypeID=A.ID and mi.Deleted=0 and m.Deleted=0 and  m.GenderID=G.ID and m.RelationShipID=2 and mi.EmployeeID='" + Session["EmployeeID"] + "'", sCon);//,mi.AccountNo,mi.BankName,mi.IfscCode
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
                        ViewState["EmailID"] = sDr["EmailID"].ToString();
                        EmpAge.Value = sDr["Age"].ToString();
                        ViewState["ID"] = sDr["ID"].ToString();
                        ViewState["EmpAudittrail"] = sDr["AuditTrail"].ToString();
                        ViewState["EmpInfoAudittrail"] = sDr["infoAuditTrail"].ToString();
                        ViewState["EnrollStartDate"] = sDr["EnrollStartDateTime"].ToString();
                        txtMarriageDate.Text = sDr["MarriageDateTime"].ToString();                       
                        lblWelcomeNote.Text = "Welcome " + sDr["MemberName"].ToString();
                    }
                    sDr.Close();
                    LoadMethods();
                    sCon.Close();


                }
                else
                    Response.Redirect("Bally_Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
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
        RemoveChildRelation();
    }

    public void AddRelation()
    {
        ddlRelation.Items.Clear();
        ddlRelation.Items.Add(new ListItem("Select Relation", "0"));
        ddlRelation.Items.Add(new ListItem("Son", "4"));
        ddlRelation.Items.Add(new ListItem("Daughter", "5"));
        ddlRelation.Items.Add(new ListItem("Wife", "18"));
        ddlRelation.Items.Add(new ListItem("Husband", "21"));
    }

    public void GridBind()
    {
        sDa = new SqlDataAdapter("Select m.ID,m.MemberName,r.Name as Relation,g.Name as Gender, CONVERT(varchar,m.DateOfBirth,103) as DateOfBirth, m.Age,A.Name as AgeType,m.AuditTrail from DemoEnrollment_MEMBERS m,RelationShip r, Gender g, AgeType A where m.AgeTypeID=a.ID and m.GenderID=g.ID and m.RelationShipID=r.ID and m.RelationShipID not in(2) and m.Deleted=0 and m.Confirm=0 and m.EmployeeID='" + Session["EmployeeID"] + "' order by m.ID", sCon);//and m.RelationShipID in(3,4,5,12,13,18,20,21)
        Ds = new DataSet();
        sDa.Fill(Ds);
        grdDependentdetails.Columns[0].Visible = true;
        grdDependentdetails.Columns[7].Visible = true;
        grdDependentdetails.DataSource = Ds.Tables[0];
        grdDependentdetails.DataBind();
        grdDependentdetails.Columns[0].Visible = false;
        grdDependentdetails.Columns[7].Visible = false;

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
        tdMarriageDate.Visible = false;
        tdMarriageDate1.Visible = false;
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
        grdDependentdetails.Columns[8].Visible = false;
        grdDependentdetails.Columns[9].Visible = false;
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
                //if (depAge <= 21 || txtAgeType.Text != "Year(s)")
                //    return true;
                //else
                //{
                //    Master.AlertBox("Son/Daughter age should be below 21 years");
                //    return false;
                //}
                return true;
            }
            else
            {
                Master.AlertBox("Son/Daughter age should less than Employee age");
                return false;
            }
        }
        else if ((ddlRelation.SelectedValue == "3") || (ddlRelation.SelectedValue == "20") || (ddlRelation.SelectedValue == "12") || (ddlRelation.SelectedValue == "13"))
        {
            if (Empage < depAge)
            {
                //if (depAge <= 80)//&& txtAgeType.Text == "Year(s)")
                //    return true;
                //else
                //{
                //    Master.AlertBox("Parents/Parent in laws age should be below 80 years");
                //    return false;
                //}

                return true;
            }
            else
            {
                Master.AlertBox("Parents/Parent in laws age should more than Employee age");
                return false;
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

    public void OldEmployees()
    {
        sDa = new SqlDataAdapter("select m.membername,r.name as Relation,g.name Gender,convert(varchar,m.dateofbirth,103)dateofbirth,m.age,a.name agetype from DemoEnrollment_MEMBERS m,DemoEnrollment_MEMBERINFO mi,agetype A,gender G ,relationship r where m.employeeid=mi.employeeid and m.agetypeid=A.id and m.genderid=G.id and m.relationshipid=r.id and m.RelationShipID not in(2) and mi.Deleted=0 and m.deleted=0 and m.Confirm=1 and m.Employeeid='" + Session["EmployeeID"] + "' order by m.id", sCon);//and m.relationshipid in(3,4,5,12,13,18,20,21)
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

    public void RemoveChildRelation()
    {
        children = 0;
        string spouse = String.Empty;
        sDa = new SqlDataAdapter("select m.membername,r.name as Relation,g.name Gender,convert(varchar,m.dateofbirth,103)dateofbirth,m.age,a.name agetype from DemoEnrollment_MEMBERS m,DemoEnrollment_MEMBERINFO mi,agetype A,gender G ,relationship r where m.agetypeid=A.id and m.genderid=G.id and m.relationshipid=r.id and m.deleted=0 and m.employeeid=mi.employeeid and m.relationshipid not in(2) and m.Employeeid='" + Session["EmployeeID"] + "' order by m.id", sCon);
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

    public void InsertDependetDetails()
    {
        if (btnAddDependents.Text == "Add Dependent")
        {
            if (grdDependentdetails.Rows.Count > 5)
            {
                Master.AlertBox("Maximum Dependents Added...");
            }
            else
            {
                ViewState["AuditTrail"] = txtName.Text.Trim() + "," + ddlRelation.SelectedValue + "," + txtGender.Text.Trim() + "," + txtDateOfBirth.Text.Trim() + "," + txtAge.Text.Trim() + "," + txtAgeType.Text.Trim() + "<>";
                sCmd = new SqlCommand("insert into DemoEnrollment_MEMBERS (EmployeeID,membername,relationshipid,genderid,dateofbirth,age,agetypeid,"
                    + "EnrolledDateTime,audittrail,deleted,Confirm) values(@EmployeeID,@Name,@Relation,@Gender,@Dob,@age,@agetype,@EnrolledDate,"
                    + "@audittrail,@Delete,@Confirm)", sCon);

                sCmd.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text.Trim());
                sCmd.Parameters.AddWithValue("@Name", txtName.Text);
                sCmd.Parameters.AddWithValue("@Relation", ddlRelation.SelectedValue);
                sCmd.Parameters.AddWithValue("@Gender", obj.GetGenderCode(txtGender.Text));
                sCmd.Parameters.AddWithValue("@Dob", obj.DOBConvertion(txtDateOfBirth.Text));
                sCmd.Parameters.AddWithValue("@age", txtAge.Text);
                sCmd.Parameters.AddWithValue("@agetype", obj.Agetype(txtAgeType.Text));
                sCmd.Parameters.AddWithValue("@audittrail", ViewState["AuditTrail"].ToString());
                sCmd.Parameters.AddWithValue("@EnrolledDate", DateTime.Now);
                sCmd.Parameters.AddWithValue("@Delete", 0);
                sCmd.Parameters.AddWithValue("@Confirm", 0);

                int result = sCmd.ExecuteNonQuery();

                if (result > 0)
                    Master.AlertBox("Dependent Details Saved Successfully");
                else
                    Master.AlertBox("Please try again");
            }
        }
        else if (btnAddDependents.Text == "Modify Dependent")
        {
            ViewState["AuditTrail"] = ViewState["AuditTrail"] + ":" + txtName.Text.Trim() + "," + ddlRelation.SelectedValue + "," + txtGender.Text.Trim() + "," + txtDateOfBirth.Text.Trim() + "," + txtAge.Text.Trim() + "," + txtAgeType.Text.Trim() + "<>";
            sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set membername=@Name,relationshipid=@Relation,genderid=@Gender,dateofbirth=@Dob,"
                + "Age=@Age,agetypeid=@Agetype,AuditTrail=@audittrail,ModifiedDateTime=@ModifiedDate,Confirm=@Confirm where "
                + "employeeid='" + Session["EmployeeID"] + "' and id='" + ViewState["DepID"].ToString() + "' and deleted=0", sCon);

            sCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            sCmd.Parameters.AddWithValue("@Relation", ddlRelation.SelectedValue);
            sCmd.Parameters.AddWithValue("@Gender", obj.GetGenderCode(txtGender.Text));
            sCmd.Parameters.AddWithValue("@Dob", obj.DOBConvertion(txtDateOfBirth.Text));
            sCmd.Parameters.AddWithValue("@Age", txtAge.Text);
            sCmd.Parameters.AddWithValue("@Agetype", obj.Agetype(txtAgeType.Text));
            sCmd.Parameters.AddWithValue("@audittrail", ViewState["AuditTrail"].ToString());
            sCmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            sCmd.Parameters.AddWithValue("@Confirm", 0);

            int result = sCmd.ExecuteNonQuery();

            if (result > 0)
                Master.AlertBox("Dependent Details Updated Successfully");
            else
                Master.AlertBox("Please try again");
        }

        LoadMethods();
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

    #endregion


    #region Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            ViewState["EmpAudittrail"] = ViewState["EmpAudittrail"] + ":" + txtEmployeeName.Text.Trim() + "," + obj.DOBConvertion(txtEmpDob.Text) + "," + EmpAge.Value + "<>";
            //sTr = sCon.BeginTransaction(IsolationLevel.ReadCommitted);
            sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set  dateofbirth='" + obj.DOBConvertion(txtEmpDob.Text) + "',age='" + EmpAge.Value + "',audittrail='" + ViewState["EmpAudittrail"].ToString() + "'  where employeeid='" + Session["EmployeeID"] + "' and id='" + ViewState["ID"] + "' and deleted=0 ", sCon);
            //sCmd.Transaction = sTr;
            sCmd.ExecuteNonQuery();

            ViewState["EmpInfoAudittrail"] = ViewState["EmpInfoAudittrail"] + ":" + txtContactNo.Text + "," + txtEmailID.Text + "<>";
            sCmd = new SqlCommand("update DemoEnrollment_MEMBERINFO set  contactnumber='" + txtContactNo.Text + "' ,emailid='" + txtEmailID.Text + "',audittrail='" + ViewState["EmpInfoAudittrail"].ToString() + "' where employeeid='" + Session["EmployeeID"] + "' and deleted=0", sCon);//,AccountNo='" + txtAccountNo.Text.Trim() + "',BankName='" + txtBankName.Text.Trim() + "',IfscCode='" + txtIfscCode.Text.Trim() + "'
            //sCmd.Transaction = sTr;
            sCmd.ExecuteNonQuery();
            ViewState["EmailID"] = txtEmailID.Text.Trim();

            //sTr.Commit();
            Master.AlertBox("Member Details Updated Successfully");
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
                                    Master.AlertBox("Dependent can not be enrolled as your Date of marriage is more than 30 days.");
                                else if (30 - days1 < 0)
                                    Master.AlertBox("Dependent can not be enrolled as your Date of marriage is more than 30 days.");
                                else
                                {
                                    sCmd = new SqlCommand("update DemoEnrollment_MEMBERINFO set MarriageDateTime='" + obj.DOBConvertion(txtMarriageDate.Text.Trim()) + "' where deleted=0 and employeeid='" + Session["EmployeeID"] + "'", sCon);
                                    sCmd.ExecuteNonQuery();

                                    InsertDependetDetails();
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
                            Master.AlertBox("Dependent can not be enrolled as your child date of birth is more than 30 days");
                        else if (30 - days1 < 0)
                            Master.AlertBox("Dependent can not be enrolled as your child date of birth is more than 30 days");
                        else
                        {
                            sCmd = new SqlCommand("update DemoEnrollment_MEMBERINFO set ChildDobDateTime='" + obj.DOBConvertion(txtDateOfBirth.Text.Trim()) + "' where deleted=0 and employeeid='" + Session["EmployeeID"] + "'", sCon);
                            sCmd.ExecuteNonQuery();

                            InsertDependetDetails();
                        }
                    }
                }
            }
            else
            {
                LoadMethods();
                Master.AlertBox("Page Refreshed.....");
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

            LinkButton b = new LinkButton();
            string clientid = ((LinkButton)sender).ClientID.ToString();
            string relation = String.Empty;
            for (int i = 0; i < grdDependentdetails.Rows.Count; i++)
            {
                b = (LinkButton)grdDependentdetails.Rows[i].FindControl("btnModify");
                if (clientid == b.ClientID.ToString())
                {
                    ViewState["DepID"] = grdDependentdetails.Rows[i].Cells[0].Text;
                    txtName.Text = grdDependentdetails.Rows[i].Cells[1].Text;
                    relation = grdDependentdetails.Rows[i].Cells[2].Text;
                    txtGender.Text = grdDependentdetails.Rows[i].Cells[3].Text;
                    txtDateOfBirth.Text = grdDependentdetails.Rows[i].Cells[4].Text;
                    txtAge.Text = grdDependentdetails.Rows[i].Cells[5].Text;
                    txtAgeType.Text = grdDependentdetails.Rows[i].Cells[6].Text;
                    btnAddDependents.Text = "Modify Dependent";
                    ViewState["AuditTrail"] = grdDependentdetails.Rows[i].Cells[7].Text;
                    //..........................................

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
                }
            }
            ModifyRelation(relation);
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    protected void Delete_OnClick(object sender, EventArgs e)
    {
        try
        {
            sCon.Open();
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
            int index = row.RowIndex;
            string id = grdDependentdetails.Rows[index].Cells[0].Text;
            sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set deleted=@Deleted,Deleteddatetime=@Deleteddatetime where EmployeeID=@EmployeeID and ID=@ID", sCon);
            sCmd.Parameters.Add("@Deleted", SqlDbType.Bit).Value = 1;
            sCmd.Parameters.Add("@Deleteddatetime", SqlDbType.DateTime).Value = DateTime.Now;
            sCmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 30).Value = Session["EmployeeID"].ToString();
            sCmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

            int j = sCmd.ExecuteNonQuery();

            if (j > 0)
                Master.AlertBox("Dependent Details Deleted Successfully");
            else
                Master.AlertBox("Unable to Delete Dependent Details..");

            LoadMethods();
            ClearControls();
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

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            sDa = new SqlDataAdapter("Select m.MemberName,r.name,case when m.GenderID=1 then 'Male' else 'Female' end Gender,convert(varchar,m.DateOfBirth,106) DateOfBirth,convert(varchar, m.Age) +' '+ A.name as Age from DemoEnrollment_MEMBERS m,Relationship R,agetype A,DemoEnrollment_MEMBERINFO mi Where m.RelationshipId=R.Id and m.AgeTypeId=A.Id and m.RelationshipId  in (2,3,4,5,12,13,18,20,21)and m.deleted=0 and mi.Deleted=0 and m.EmployeeId='" + Session["EmployeeID"] + "' and mi.employeeid=m.employeeid order by m.id", sCon);
            sDa.SelectCommand.CommandTimeout = 30000;
            Ds = new DataSet();
            sDa.Fill(Ds, "abc");

            if (CheckDependentDob(Ds))
            {
                if (txtEmailID.Text.Trim() != "")
                {
                    sCon.Open();
                    StringBuilder format = new StringBuilder();
                    format.Append("<table id=table1 width=750px border=0 cellspacing=0 cellpadding=0>");
                    format.Append("<tr style=color:Navy;>");
                    format.Append("<td align=left><font size=3;>To</font></td><td></td></tr>");
                    format.Append("<tr><td colspan=2><br></td>");
                    format.Append("<tr style=color:Navy;>");
                    format.Append("<td align=left><font size=3;>");
                    format.Append("Dear  " + ViewState["MainmemberName"].ToString());
                    format.Append("</font></td>");
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
                    depMembers.Append("<table id=table2 width=750px border=1 cellspacing=1 cellpadding=1>");
                    depMembers.Append("<thead style=color:black;background-color:#cccccc;font-size:15;>");
                    depMembers.Append("<th width=100px>EmployeeID</th>");
                    depMembers.Append("<th width=200px>Member Name</th>");
                    depMembers.Append("<th width=100px>Relationship</th>");
                    depMembers.Append("<th width=100px>Gender</th>");
                    depMembers.Append("<th width=100px>DateOfBirth</th>");
                    depMembers.Append("<th width=100px>Age</th>");
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
                            depMembers.Append("</tr>");
                        }

                        depMembers.Append("</table>");

                        msg.To.Add(ViewState["EmailID"].ToString());
                        msg.From = new MailAddress("donotreply@fhpl.net");
                        //msg.CC.Add("nagarajuk@fhpl.net");
                        msg.Subject = "Web enrolment  for Group Mediclaim coverage 2013-14";
                        msg.Body = format.ToString() + "<br><br>" + depMembers.ToString() + "<br><br><font size=3 color=#000066><p style=width:750px;>Note: For further enrollment queries, please mail to csblr@fhpl.net<br><br>Regards.<br>FHPL Team</font>";
                        msg.IsBodyHtml = true;
                        smclt.Host = "fhpdcex";
                        smclt.Send(msg);


                        sCmd = new SqlCommand("update DemoEnrollment_MEMBERINFO set locked=1,ConfirmDateTime='" + DateTime.Now + "' where employeeid='" + Session["EmployeeID"] + "' and deleted=0", sCon);
                        sCmd.ExecuteNonQuery();

                        sCmd = new SqlCommand("update DemoEnrollment_MEMBERS set Confirm=1 where employeeid='" + Session["EmployeeID"] + "' and deleted=0", sCon);
                        sCmd.ExecuteNonQuery();

                        Response.Redirect("Bally_Complete.aspx", false);
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

    protected void ddlRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlRelation.SelectedValue == "18") || (ddlRelation.SelectedValue == "21"))
        {
            tdMarriageDate.Visible = true;
            tdMarriageDate1.Visible = true;
        }
        else
        {
            tdMarriageDate.Visible = false;
            tdMarriageDate1.Visible = false;
        }
    }

    #endregion

    protected void chkPolicyTerms_CheckedChanged(object sender, EventArgs e)
    {

    }
}