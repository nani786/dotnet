<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TBSL_MemberDetails.aspx.cs" Inherits="TBSL_MemberDetails" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link id="datetime" type="text/css" href="DatePicker/css/ui-lightness/jquery-ui-1.8.19.custom.css"
        rel="stylesheet" />
    <script type="text/javascript" src="DatePicker/js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="DatePicker/js/jquery-ui-1.8.19.custom.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("input[id$='txtDateOfBirth']").datepicker({

                changeMonth: true,
                changeYear: true,
                yearRange: '1910:2025',
                dateFormat: 'dd/mm/yy', gotoCurrent: true
            });
        });

        $(function () {
            $("input[id$='txtEmpDob']").datepicker({

                changeMonth: true,
                changeYear: true,
                yearRange: '1910:2025',
                dateFormat: 'dd/mm/yy', gotoCurrent: true
            });
        });
    </script>
    <script type="text/javascript">

        function getEmpageCurrent(strnewval, presentdate, age) {

            var dateofbirth = "";
            var present = "";

            var str1;
            var str2;
            var i = 0;
            var stp = 0;
            var enp = 0;
            var j;
            if (strnewval.value != '') {
                j = strnewval.value.length;
                for (i = 0; i < j - 1; i++) {
                    enp++;
                    if (strnewval.value.substr(i, 1) == "/") {
                        str2 = strnewval.value.substr(stp, enp - 1);
                        if (str2.length == 1) {
                            dateofbirth = dateofbirth + "0" + str2 + "/";
                        }
                        else {
                            dateofbirth = dateofbirth + str2 + "/";
                        }
                        stp = i + 1;
                        enp = 0;
                    }
                }
                dateofbirth = dateofbirth + strnewval.value.substr(stp, enp + 1);
                if (presentdate.value != '') {
                    str1 = "";
                    str2 = "";
                    i = 0;
                    stp = 0;
                    enp = 0;
                    j;
                    var j = presentdate.value.length;
                    for (i = 0; i < j - 1; i++) {
                        enp++;
                        if (presentdate.value.substr(i, 1) == "/") {
                            str2 = presentdate.value.substr(stp, enp - 1);
                            if (str2.length == 1) {
                                present = present + "0" + str2 + "/";
                            }
                            else {
                                present = present + str2 + "/";
                            }
                            stp = i + 1;
                            enp = 0;
                        }
                    }
                    present = present + presentdate.value.substr(stp, enp + 1);
                }

                //......................................................

                var dobdated = 0;
                var dobdate = dateofbirth.substr(0, 2);
                var dobmonth = dateofbirth.substr(3, 2);
                var dobyear = dateofbirth.substr(6, 4);

                var presentdated = 0;
                var presentdate = present.substr(3, 2);
                var presentmonth = present.substr(0, 2);
                var presentyear = present.substr(6, 4);

                if (dateofbirth.length >= 10) {
                    //	        document.write("("+dobyear+" > "+presentyear+") || (("+dobmonth+">"+presentmonth+") && ("+dobyear+"=="+presentyear+"))  ||	(("+dobdate+">"+presentdate+") && ("+dobmonth+"=="+presentmonth+") && ("+dobyear+"=="+presentyear+")) || ("+dobyear+" < 1900))---------");
                    ValidateDate(strnewval);
                    if ((dobyear > presentyear) || ((dobmonth > presentmonth) && (dobyear == presentyear)) || ((dobdate > presentdate) && (dobmonth == presentmonth) && (dobyear == presentyear)) || (dobyear < 1900)) {
                        type.value = "";
                        alert('Invalid Date of Birth');
                        return false;
                    }
                    else {
                        if ((presentyear == dobyear) && (presentmonth == dobmonth)) {
                            //				    document.write("("+dobyear+" > "+presentyear+") || (("+dobmonth+">"+presentmonth+") && ("+dobyear+"=="+presentyear+"))  ||	(("+dobdate+">"+presentdate+") && ("+dobmonth+"=="+presentmonth+") && ("+dobyear+"=="+presentyear+")) || ("+dobyear+" < 1900))---------");
                            var days = presentdate - dobdate;
                            if (days == 0) {
                                age.value = 1;
                            }
                            else {
                                age.value = days;
                            }
                        }
                        else if ((presentyear == dobyear) && (presentmonth != dobmonth)) {
                            var month = presentmonth - dobmonth;
                            age.value = month;
                        }
                        else if (presentyear != dobyear) {
                            var year = presentyear - dobyear;
                            if ((year != 1) && (presentmonth < dobmonth)) {
                                age.value = year - 1;
                            }
                            else if ((year != 1) && (presentmonth > dobmonth)) {
                                age.value = year;
                            }

                            else if ((year != 1) && (presentmonth = dobmonth) && (dobdate > presentdate)) {
                                age.value = year - 1;
                            }
                            else if ((year != 1) && (presentmonth = dobmonth) && (dobdate < presentdate)) {
                                age.value = year;
                            }
                            else if ((year = 1) && (presentmonth < dobmonth)) {
                                year--;
                                var month1 = ((12 * 1) + (presentmonth * 1));
                                var mon = month1 - dobmonth;
                                age.value = mon;
                            }
                            else if ((year = 1) && (presentmonth > dobmonth)) {
                                var year1 = presentyear - dobyear;
                                age.value = year1;
                            }
                            else if ((year = 1) && (presentmonth = dobmonth)) {
                                var year1 = presentyear - dobyear;
                                age.value = year1;
                            }
                        }
                    }
                }
                else {
                    age.value = "";
                }
            }
        }

        function getageCurrent(strnewval, presentdate, age, type) {
            //document.write("xxxxxxxxxxxxxxxxx")

            if (strnewval.value != '') {
                var dateofbirth = "";
                var present = "";

                var str1;
                var str2;
                var i = 0;
                var stp = 0;
                var enp = 0;
                var j;
                if (strnewval.value != '') {
                    j = strnewval.value.length;
                    for (i = 0; i < j - 1; i++) {
                        enp++;
                        if (strnewval.value.substr(i, 1) == "/") {
                            str2 = strnewval.value.substr(stp, enp - 1);
                            if (str2.length == 1) {
                                dateofbirth = dateofbirth + "0" + str2 + "/";
                            }
                            else {
                                dateofbirth = dateofbirth + str2 + "/";
                            }
                            stp = i + 1;
                            enp = 0;
                        }
                    }
                    dateofbirth = dateofbirth + strnewval.value.substr(stp, enp + 1);
                    if (presentdate.value != '') {
                        str1 = "";
                        str2 = "";
                        i = 0;
                        stp = 0;
                        enp = 0;
                        j;
                        var j = presentdate.value.length;
                        for (i = 0; i < j - 1; i++) {
                            enp++;
                            if (presentdate.value.substr(i, 1) == "/") {
                                str2 = presentdate.value.substr(stp, enp - 1);
                                if (str2.length == 1) {
                                    present = present + "0" + str2 + "/";
                                }
                                else {
                                    present = present + str2 + "/";
                                }
                                stp = i + 1;
                                enp = 0;
                            }
                        }
                        present = present + presentdate.value.substr(stp, enp + 1);
                    }

                    //......................................................

                    var dobdated = 0;
                    var dobdate = dateofbirth.substr(0, 2);
                    var dobmonth = dateofbirth.substr(3, 2);
                    var dobyear = dateofbirth.substr(6, 4);

                    var presentdated = 0;
                    var presentdate = present.substr(3, 2);
                    var presentmonth = present.substr(0, 2);
                    var presentyear = present.substr(6, 4);

                    if (dateofbirth.length >= 10) {
                        //	        document.write("("+dobyear+" > "+presentyear+") || (("+dobmonth+">"+presentmonth+") && ("+dobyear+"=="+presentyear+"))  ||	(("+dobdate+">"+presentdate+") && ("+dobmonth+"=="+presentmonth+") && ("+dobyear+"=="+presentyear+")) || ("+dobyear+" < 1900))---------");
                        ValidateDate(strnewval);
                        if ((dobyear > presentyear) || ((dobmonth > presentmonth) && (dobyear == presentyear)) || ((dobdate > presentdate) && (dobmonth == presentmonth) && (dobyear == presentyear)) || (dobyear < 1900)) {
                            type.value = "";
                            alert('Invalid Date of Birth');
                            return false;
                        }
                        else {
                            if ((presentyear == dobyear) && (presentmonth == dobmonth)) {
                                //				    document.write("("+dobyear+" > "+presentyear+") || (("+dobmonth+">"+presentmonth+") && ("+dobyear+"=="+presentyear+"))  ||	(("+dobdate+">"+presentdate+") && ("+dobmonth+"=="+presentmonth+") && ("+dobyear+"=="+presentyear+")) || ("+dobyear+" < 1900))---------");
                                var days = presentdate - dobdate;
                                if (days == 0) {
                                    age.value = 1;
                                    type.value = "Day(s)";
                                }
                                else {
                                    age.value = days;
                                    type.value = "Day(s)";
                                }
                            }
                            else if ((presentyear == dobyear) && (presentmonth != dobmonth)) {
                                var month = presentmonth - dobmonth;

                                //------------ This code is added on 2nd june 2012 ------------------------------

                                if (presentdate < dobdate) {
                                    month = month - 1;
                                }
                                if (month == 0) {
                                    if (presentdate < dobdate) {
                                        days = (parseInt(30) + parseInt(presentdate)) - parseInt(dobdate);
                                    }
                                    else {
                                        days = presentdate - dobdate;
                                    }
                                    age.value = days;
                                    type.value = "Day(s)";
                                }

                                //till

                                else {
                                    age.value = month;
                                    type.value = "Month(s)";
                                }
                            }
                            else if (presentyear != dobyear) {
                                var year = presentyear - dobyear;
                                if ((year != 1) && (presentmonth < dobmonth)) {
                                    age.value = year - 1;
                                    type.value = "Year(s)";
                                }
                                else if ((year != 1) && (presentmonth > dobmonth)) {
                                    age.value = year;
                                    type.value = "Year(s)";
                                }

                                else if ((year != 1) && (presentmonth = dobmonth) && (dobdate > presentdate)) {
                                    age.value = year - 1;
                                    type.value = "Year(s)";
                                }
                                else if ((year != 1) && (presentmonth = dobmonth) && (dobdate < presentdate)) {
                                    age.value = year;
                                    type.value = "Year(s)";
                                }
                                else if ((year = 1) && (presentmonth < dobmonth)) {
                                    year--;
                                    var month1 = ((12 * 1) + (presentmonth * 1));
                                    var mon = month1 - dobmonth;
                                    age.value = mon;
                                    type.value = "Month(s)";
                                }
                                else if ((year = 1) && (presentmonth > dobmonth)) {
                                    var year1 = presentyear - dobyear;
                                    age.value = year1;
                                    type.value = "Year(s)";
                                }
                                else if ((year = 1) && (presentmonth = dobmonth)) {
                                    var year1 = presentyear - dobyear;
                                    age.value = year1;
                                    type.value = "Year(s)";
                                }
                            }
                        }
                    }
                    else {
                        age.value = "";
                        type.value = "";
                    }
                }
            }
        }
        function ValidateDate(objField) {
            var revtext;
            revtext = objField.value;
            var ivalue;

            var idate = revtext.substr(0, 2);
            var imon = revtext.substr(3, 2);
            var iyear = revtext.substr(6, 4);

            if (objField.value.length == 10) {
                //document.write(objField.value.length);

                if (imon > 12 || imon < 1) {
                    alert('InValid Month');
                    objField.focus();
                }
                else if (idate < 1 || idate > 31) {
                    alert('InValid Date');
                    objField.focus();
                }

                else if ((iyear < 1900) || (iyear > 2050)) {
                    alert('Year Should be between 1900 To 2050');
                    objField.focus();
                    //iyear=2005;
                }
            }
        }
        function genValidate(id, gender) {
            var gen = id.value;
            if (gen == 1) {
                gender.value = "";
            }
            else if (gen == 21 || gen == 4 || gen == 20 || gen == 22 || gen == 12) {
                gender.value = "Male";
            }
            else if (gen == 3 || gen == 5 || gen == 18 || gen == 19 || gen == 13) {
                gender.value = "Female";
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            font-family: Verdana;
            font-size: small;
            color: Navy;
            font-weight: bold;
            width: 116px;
        }
        .cssbutton
        {
        }
        .style3
        {
            height: 178px;
        }
        .style4
        {
            width: 7338px;
        }
        .style6
        {
            width: 7338px;
            height: 131px;
        }
        .style7
        {
            width: 136px;
        }
        p.MsoNormal
        {
            margin: 0in;
            margin-bottom: .0001pt;
            font-size: 12.0pt;
            font-family: "Times New Roman" , "serif";
        }
        p
        {
            margin-right: 0in;
            margin-left: 0in;
            font-size: 12.0pt;
            font-family: "Times New Roman" , "serif";
        }
        .style8
        {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" style="height: 572px">
        <tr>
            <td align="left">
                <asp:Label ID="lblWelcomeNote" runat="server" Font-Names="Verdana" Font-Size="Small"
                    ForeColor="#0000CC"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnHrBack" runat="server" Text="Back" CssClass="cssbutton" Width="146px"
                    Visible="false" OnClick="btnHrBack_Click" Height="26px" />
            </td>
        </tr>
        <tr>
            <td align="center" class="style3">
                <asp:Label ID="lblEmpDetails" runat="server" Text="EMPLOYEE DETAILS" CssClass="label"
                    Font-Underline="True"></asp:Label><br />
                <table style="height: 115px; background-color: #B2B2B2; width: 921px;" border="1">
                    <tr>
                        <td align="center">
                            <table style="width: 899px; height: 125px;">
                                <tr>
                                    <td class="style1" align="left">
                                        EmployeeID
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEmployeeID" runat="server" Width="130px" CssClass="textBox" TabIndex="1"
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                    <td class="label" align="left">
                                        EmployeeName
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEmployeeName" runat="server" Width="200px" CssClass="textBox"
                                            TabIndex="2" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td align="left" class="label">
                                        Gender
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlEmpGender" runat="server" CssClass="dropdown" TabIndex="3"
                                            Width="120px" Enabled="False">
                                            <asp:ListItem Value="0">Select Gender</asp:ListItem>
                                            <asp:ListItem Value="1">Male</asp:ListItem>
                                            <asp:ListItem Value="2">Female</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1" align="left">
                                        DateOfBirth<asp:Label ID="Label1" runat="server" Font-Bold="false" Font-Size="XX-Small"
                                            Text="(dd/mm/yyyy)"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEmpDob" runat="server" CssClass="textBoxDate" TabIndex="4" Enabled="False"></asp:TextBox>
                                        <%--  <asp:CalendarExtender ID="txtEmpDob_CalendarExtender" runat="server" Enabled="True"
                                            Format="dd/MM/yyyy" TargetControlID="txtEmpDob" TodaysDateFormat="dd/MM/yyyy">
                                        </asp:CalendarExtender>--%>
                                        <asp:RequiredFieldValidator ID="rfvvEmpDob" runat="server" ControlToValidate="txtEmpDob"
                                            CssClass="labelError" Display="None" ErrorMessage="Please Enter DateOfBirth"
                                            SetFocusOnError="True" ValidationGroup="main">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvvEmpDob_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvvEmpDob">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td class="label" align="left">
                                        EmailID
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEmailID" runat="server" Width="200px" CssClass="textBox" TabIndex="5"
                                            Enabled="False"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rxvEmailID" runat="server" ControlToValidate="txtEmailID"
                                            CssClass="labelError" Display="None" ErrorMessage="Invalid EmailID" SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="main">*</asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="rxvEmailID_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rxvEmailID">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td align="left" class="label">
                                        MobileNumber
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtContactNo" runat="server" Width="130px" MaxLength="10" TabIndex="6"
                                            AutoPostBack="True" OnTextChanged="txtContactNo_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtContactNo_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="txtContactNo">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="rxContactNo" runat="server" ErrorMessage="Please enter Valid Mobile Number"
                                            ControlToValidate="txtContactNo" CssClass="labelError" Display="None" SetFocusOnError="True"
                                            ValidationGroup="main" ValidationExpression="^[0-9]{10}">*</asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="rxContactNo_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rxContactNo" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContactNo"
                                            CssClass="labelError" Display="none" ErrorMessage="Enter Contact number" ForeColor="#FF3300"
                                            SetFocusOnError="True" ValidationGroup="main">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator1" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label" align="left">
                                        Base SumInsured
                                    </td><td>
                                    <asp:TextBox ID="txtBaseSumInsured" runat="server"  CssClass="textBoxDate"
                                        AutoPostBack="True" Enabled="False"></asp:TextBox>
</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="6" bgcolor="White" style="height: 35px;">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Width="78px"
                                TabIndex="7" OnClick="btnSave_Click" ValidationGroup="main" />
                            <%--<asp:Button ID="btnUploadPhoto" runat="server" Text="UploadPhoto" CssClass="button"
                                Width="129px" TabIndex="8" />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%-- <tr>
            <td align="center">
                <asp:Label ID="lblDependentsDetails0" runat="server" Text="DEPENDENTS DETAILS" CssClass="label"
                    Font-Underline="True"></asp:Label>
                <asp:GridView ID="grdOldMemberDetails" runat="server" AutoGenerateColumns="False"
                    BorderColor="Navy" BorderWidth="1px" CellPadding="4" Font-Names="Verdana" Font-Size="Small"
                    ForeColor="#333333" GridLines="None" Height="5px" Width="866px">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="Membername" HeaderText="Name" />
                        <asp:BoundField DataField="Relation" HeaderText="Relationship" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" />
                        <asp:BoundField DataField="Dateofbirth" HeaderText="DateOfBirth" />
                        <asp:BoundField DataField="age" HeaderText="Age" />
                        <asp:BoundField DataField="agetype" HeaderText="AgeType" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </td>
        </tr>--%>
        <tr align="center">
            <td>
                &nbsp;<asp:Label ID="lblError" runat="server" CssClass="labelError"></asp:Label>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblAddDependent" runat="server" Text="ADD YOUR DEPENDANTS HERE" CssClass="label"
                    Font-Underline="True"></asp:Label>
                <table class="label" style="width: 919px; height: 95px; background-color: #B2B2B2"
                    border="1">
                    <tr>
                        <td align="center">
                            <table style="width: 902px; height: 60px;">
                                <tr align="center">
                                    <td class="style8">
                                        Name
                                    </td>
                                    <td class="style8">
                                        Relationship
                                    </td>
                                    <td class="style8">
                                        Gender
                                    </td>
                                    <td class="style8">
                                        DateOfBirth<asp:Label ID="Label2" runat="server" Font-Bold="false" Font-Size="XX-Small"
                                            Text="(dd/mm/yyyy)"></asp:Label>
                                    </td>
                                    <td class="style8">
                                        Age
                                    </td>
                                    <td class="style8">
                                        AgeType
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="textBox" TabIndex="12"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtName_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" TargetControlID="txtName" ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="labelError" Display="None"
                                            ErrorMessage="Please Enter Name" SetFocusOnError="True" ControlToValidate="txtName"
                                            ValidationGroup="Dep">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvName_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvName" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="ddlRelation" runat="server" CssClass="dropdownRelation" TabIndex="13">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvRelation" runat="server" Display="None" ErrorMessage="Please Select Relation"
                                            SetFocusOnError="True" CssClass="labelError" Text="*" ControlToValidate="ddlRelation"
                                            ValidationGroup="Dep" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvRelation_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvRelation" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center" style="margin-left: 40px">
                                        <asp:TextBox ID="txtGender" runat="server" CssClass="textBoxDate" TabIndex="14"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvGender" runat="server" ErrorMessage="Please Select Gender"
                                            Display="None" CssClass="labelError" ValidationGroup="Dep" ControlToValidate="txtGender">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvGender_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvGender" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="textBoxDate" TabIndex="16"
                                            ControlToValidate="txtDateOfBirth"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDob" runat="server" ErrorMessage="Please Enter DateOfBirth"
                                            Display="None" CssClass="labelError" ValidationGroup="Dep" ControlToValidate="txtDateOfBirth"
                                            SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvDob_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvDob" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtAge" runat="server" CssClass="textBoxDate" TabIndex="17"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtAge_FilteredTextBoxExtender" runat="server" Enabled="True"
                                            FilterType="Numbers" TargetControlID="txtAge">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvAge" runat="server" ErrorMessage="Please Enter Age"
                                            ControlToValidate="txtAge" CssClass="labelError" ValidationGroup="Dep" Display="None"
                                            SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvAge_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvAge" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtAgeType" runat="server" CssClass="textBoxDate" TabIndex="18"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAgetype" runat="server" ErrorMessage="Please Enter Age"
                                            ControlToValidate="txtAgeType" ValidationGroup="Dep" CssClass="labelError" Display="None"
                                            SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvAgetype_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvAgetype" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="6" bgcolor="White" style="height: 35px;">
                            <asp:Button ID="btnAddDependents" runat="server" Text="Add Dependant" CssClass="button"
                                Width="150px" TabIndex="15" ValidationGroup="Dep" OnClick="btnAddDependents_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblDependentsDetails" runat="server" Text="DEPENDANTS DETAILS" CssClass="label"
                    Font-Underline="True"></asp:Label><br />
                <asp:GridView ID="grdDependentdetails" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="826px" AutoGenerateColumns="False" Font-Names="Verdana"
                    Font-Size="Small" Height="5px" BorderColor="Navy" BorderWidth="1px">
                    <RowStyle BackColor="#EFF3FB" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor=" #124d77" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Membername" HeaderText="Name" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Relation" HeaderText="Relationship" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Dateofbirth" HeaderText="DateOfBirth" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="age" HeaderText="Age" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="agetype" HeaderText="AgeType" ItemStyle-HorizontalAlign="center" />
                        <%--<asp:BoundField DataField="AuditTrail" HeaderText="AuditTrail" />--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnModify" runat="server" OnClick="Modify_OnClick" Text="Modify"
                                    Font-Names="Verdana" Font-Size="Small" ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" OnClick="Delete_OnClick" Text="Delete"
                                    Font-Names="Verdana" Font-Size="Small"></asp:LinkButton>
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete"
                                    ConfirmText="Are you sure to Delete the dependant?">
                                </asp:ConfirmButtonExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <table class="label" style="width: 663px; height: 176px; background-color: #B2B2B2"
            border="1">
            <tr>
                <td align="CENTER" colspan="2" class="style6" style="font-family: Verdana; font-weight: bold;
                    color: #0000FF;">
                    <asp:Label ID="lblSumInsured" runat="server" Text="SumInsured" CssClass="label"></asp:Label>
                    &nbsp;
                    <asp:DropDownList ID="ddlSuminsured" runat="server" AutoPostBack="True" Height="30px"
                        OnSelectedIndexChanged="ddlSuminsured_SelectedIndexChanged" Width="150px">
                        <asp:ListItem Selected="True" Value="0">--Select One--</asp:ListItem>
                        <asp:ListItem>100000</asp:ListItem>
                        <asp:ListItem>200000</asp:ListItem>
                        <asp:ListItem>300000</asp:ListItem>
                        <asp:ListItem>400000</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblPremium" runat="server" Text="Premium" CssClass="label"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtPremium" runat="server" Width="150px" MaxLength="10" TabIndex="6"
                        Enabled="False"></asp:TextBox>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center" class="style7">
                    <asp:CheckBox ID="chkDisclaimer1" runat="server" OnCheckedChanged="chkDisclaimer1_CheckedChanged"
                        AutoPostBack="True" />
                </td>
                <td align="left" style="color: #0000FF; font-family: Verdana; font-size: small;"
                    class="style4">
                    <br />
                    I am aware that the above mentioned premium will be deducted from my salary for
                    Super Top Up policy and I agree to this condition.
                </td>
            </tr>
            <br />
            <%--   <tr>
                        <td align="center" class="style7">
                            <asp:CheckBox ID="chkDisclaimer2" runat="server" 
                                AutoPostBack="True" oncheckedchanged="chkDisclaimer2_CheckedChanged" />
                        </td>
                        <td align="left" 
                            style="color: #FF0000; font-family: Arial; font-size: medium;" 
                            class="style4">
                             <p class="MsoNormal" 
                                 
                                 style="MARGIN-BOTTOM: 12pt; font-family: Verdana; font-size: small; color: #0000FF;">
                                 &nbsp;I have understood and agree to the policy details and conditions as detailed in 
                                 the employer communication.
                                 <span style="FONT-FAMILY: 'Calibri','sans-serif'; COLOR: #1f497d; FONT-SIZE: 11pt"></span></p>
                                    
                           
                           </td>
                    </tr>--%>
            <br />
            <tr>
                <%--  <td align="center" class="style7">
                            <asp:CheckBox ID="chkDisclaimer3" runat="server" 
                                AutoPostBack="True" oncheckedchanged="chkDisclaimer3_CheckedChanged" />
                        </td>--%>
                <%-- <td align="left" 
                            style="color: #FF0000; font-family: Andalus; font-size: medium;" 
                            class="style4">
                            I have understood that the policy tenure for this year, as a one-time realignment, is of 8 months i.e. August 1, 2016 to March 31, 2017. 
                            From next renewal cycle, it will be 12 months - April 1 to March 31. 
                         </td>--%>
            </tr>
        </table>
        <tr>
        </tr>
        <tr>
            <td align="center">
                <br />
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="button" TabIndex="16"
                    OnClick="btnConfirm_Click" ValidationGroup="main" />
                <br />
            </td>
        </tr>
        <%--<td align="center" runat="server">
            <asp:Button ID="Button1" runat="server" Text="Button" Style="visibility: hidden;"
                Height="19px" Width="112px" />
            <asp:ModalPopupExtender ID="mpTopUp" runat="server" BackgroundCssClass="modalPopup"
                Enabled="true" PopupControlID="pnlConfirm" TargetControlID="Button1">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlConfirm" runat="server" BackColor="white" Height="198px" Width="644px" Visible="false">
                <center>
                    <table id="tbl1" runat="server" cellpadding="0" cellspacing="0" style="font-size: small;
                        font-family: Verdana; height: 191px; width: 624px; margin-top: 1px;" visible="true">
                        <tr>
                            <td align="center" style="font-size: small; font-family: Verdana;" class="style2">
                                 Once updated, details cannot be changed during the policy period.
                                 <br />
                                 <br />
                                 To proceed click on “OK” button. 
                                 <br />
                                 <br />
                                 To cancel click on “BACK” button.   

                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnyes" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small"
                                     ForeColor="Navy"
                                    Text="OK" Width="100px" TabIndex="19"  Style="height: 26px" 
                                    onclick="btnyes_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                <asp:Button ID="btnNo" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small"
                                  ForeColor="Navy"
                                    Text="Back" Width="100px" TabIndex="20"  Style="height: 26px" 
                                    onclick="btnNo_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
        </td>--%>
        <%--    <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BackColor="white" Height="130px" Width="644px"
                    Visible="false">
                    <center>
                        <table id="Table1" runat="server" cellpadding="0" cellspacing="0" style="font-size: small;
                            font-family: Verdana; height: 118px; width: 604px; margin-top: 1px;">
                            <tr>
                                <td align="center" style="font-size: small; font-family: Verdana;" class="style4">
                                    <br />
                                 
                                    <asp:Label ID="lblmessage" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOk" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small"
                                        ForeColor="Navy" Text="Confirm" Width="100px" TabIndex="19" OnClick="btnOk_Click"
                                        OnClientClick="DisableButtonbtnok()" UseSubmitBehavior="false" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btncancel" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small"
                                        ForeColor="Navy" Text="Back" Width="100px" TabIndex="19" OnClick="btncancel_Click"
                                        OnClientClick="DisableButtonbtnok()" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
            </td>
          
        </tr>--%>
        <tr>
            <td align="center">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="EmpAge" runat="server" 
         />
    <asp:HiddenField ID="TodayDate" runat="server" />
</asp:Content>
