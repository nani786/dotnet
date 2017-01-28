<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TBSL_ExistingMemberDetails.aspx.cs" Inherits="TBSL_ExistingMemberDetails" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="StyleSheet.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript">

        //        function BlinkText() {
        //            var txtblink = document.getElementById('<%=lblWelcomeNote.ClientID %>');
        //            if (txtblink.style.color == "red") {
        //                txtblink.style.color = "blue";
        //            }
        //            else {
        //                txtblink.style.color = "red";
        //            }
        //            setTimeout("BlinkText()", 300);
        //        }
        function DisableParentsButton() {

            document.getElementById("<%=btnConfirm.ClientID %>").disabled = true;
          
        }

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
        .style6
        {
            height: 15px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="lblWelcomeNote" runat="server" Font-Names="Verdana" Font-Size="Small"
                    ForeColor="#0000CC"></asp:Label>
                <%--<asp:LinkButton ID="LinkButton1" Style="float: right; margin-left: 27px;" runat="server"
                    Font-Bold="True" Font-Overline="False" Font-Size="Medium" Font-Underline="True"
                    OnClick="LbBack_Click">Back</asp:LinkButton>--%>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblEmpDetails" runat="server" Text="EMPLOYEE DETAILS" CssClass="label"
                    Font-Underline="True"></asp:Label><br />
                <table style="height: 115px; background-color: #B2B2B2; width: 921px;" border="1">
                    <tr>
                        <td align="center">
                            <table style="width: 899px; height: 69px;">
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
                                        <asp:TextBox ID="txtEmpDob" runat="server" CssClass="textBoxDate" TabIndex="4" 
                                            Enabled="False" EnableTheming="True"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEmpDob_CalendarExtender" runat="server" Enabled="True"
                                            Format="dd/MM/yyyy" TargetControlID="txtEmpDob" TodaysDateFormat="dd/MM/yyyy">
                                        </asp:CalendarExtender>
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
                                        <asp:TextBox ID="txtEmailID" runat="server" Width="200px" CssClass="textBox" 
                                            TabIndex="5" Enabled="False"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEmailId" runat="server" ControlToValidate="txtEmailID"
                                            CssClass="labelError" Display="None" ErrorMessage="EmailID" SetFocusOnError="True"
                                            ValidationGroup="main">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvEmailId_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvEmailId">
                                        </asp:ValidatorCalloutExtender>
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
                                        <asp:TextBox ID="txtContactNo" runat="server" Width="130px" MaxLength="10" TabIndex="6"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rfv_ContactNumber" runat="server" ErrorMessage="Please enter Contact Number"
                                            Text="*" ForeColor="Red" ValidationGroup="main" ControlToValidate="txtContactNo"
                                            Visible="false"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="Rfv_ContactNumber_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="Rfv_ContactNumber">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:FilteredTextBoxExtender ID="txtContactNo_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="txtContactNo">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="rxContactNo" runat="server" ErrorMessage="Please enter Valid Mobile Number"
                                            ControlToValidate="txtContactNo" CssClass="labelError" Display="None" SetFocusOnError="True"
                                            ValidationGroup="main" ValidationExpression="^[0-9]{10}">*</asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="rxContactNo_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rxContactNo" PopupPosition="TopLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td align="left" class="label">
                                        Suminsured
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEmployeeID0" runat="server" Width="130px" CssClass="textBox"
                                            TabIndex="7" Enabled="False" ReadOnly="True">INR 500000</asp:TextBox>
                                    </td>
                                    <td class="label" align="left">
                                        Cancelled Check
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="fuCheckForm" runat="server" Width="200px" TabIndex="8" />
                                    </td>
                                    <td align="left" class="label">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                    </tr>
                    <%--<tr>
                        <td align="center" colspan="6" bgcolor="White" style="height: 35px;">
                            <asp:Label ID="Label3" runat="server" Text="BANK DETAILS" CssClass="label" Font-Underline="True"></asp:Label><br />
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td align="center" colspan="6" style="height: 35px;">
                            <table style="width: 906px; height: 25px;">
                                <tr>
                                    <td class="style1" align="left">
                                        Account No
                                    </td>
                                    <td align="left" class="style3">
                                        <asp:TextBox ID="txtAccountno" runat="server" CssClass="textBox" TabIndex="9" 
                                            Enabled="False"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtAccountno_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" TargetControlID="txtAccountno" FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style4" align="left">
                                        Bank Name
                                    </td>
                                    <td align="left" class="style2">
                                        <asp:TextBox ID="txtbankname" runat="server" Width="200px" CssClass="textBox" 
                                            TabIndex="10" Enabled="False"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtbankname_FilteredTextBoxExtender" FilterType="LowercaseLetters,UppercaseLetters"
                                            runat="server" Enabled="True" TargetControlID="txtbankname">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left" class="label">
                                        IFSC CODE &nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtifsccode" runat="server" Width="150px" MaxLength="12" 
                                            TabIndex="11" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="center" colspan="6" bgcolor="White" style="height: 35px;">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Width="78px"
                                TabIndex="7" OnClick="btnSave_Click" ValidationGroup="main" 
                                 />
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
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblOldDependentsDetails" runat="server" Text="DEPENDENTS DETAILS"
                    CssClass="label" Font-Underline="True"></asp:Label>
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
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Label ID="lblError" runat="server" CssClass="labelError"></asp:Label>
               
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblAddDependent" runat="server" Text="ADD YOUR DEPENDENTS HERE" CssClass="label"
                    Font-Underline="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table class="label" 
                    style="width: 951px; height: 95px; background-color: #B2B2B2" border="1"">
                    <tr>
                        <td>
                            <center>
                                <table style="width:932px";>
                                    <tr>
                                        <td align="center">
                                            Name
                                            <asp:RequiredFieldValidator ID="Rfv_TxtdepName" runat="server" CssClass="labelError"
                                                Display="None" ErrorMessage="Please Enter Name" SetFocusOnError="True" ControlToValidate="txtName"
                                                ValidationGroup="Dep">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                                TargetControlID="Rfv_TxtdepName" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                         <td align="center">
                                            Relationship
                                            <asp:RequiredFieldValidator ID="Rfv_DDlDepgender" runat="server" Display="None" ErrorMessage="Please Select Relation"
                                                SetFocusOnError="True" CssClass="labelError" Text="*" ControlToValidate="ddlRelation"
                                                ValidationGroup="Dep" InitialValue="0"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                                TargetControlID="Rfv_DDlDepgender" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center" >
                                            Gender
                                            <asp:RequiredFieldValidator ID="Rfv_TxtDepGender" runat="server" ErrorMessage="Please Select Gender"
                                                Display="None" CssClass="labelDate" ValidationGroup="Dep" ControlToValidate="txtGender">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                                TargetControlID="Rfv_TxtDepGender" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                         <td align="center" id="tdmarriage" runat="server" visible="false">
                                             <asp:Label ID="Label2" CssClass="labelDate" runat="server" Font-Bold="True" 
                                                Text=" DateOfMarriage" Font-Size="Small"></asp:Label>
                                           <br /><asp:Label ID="Label4" runat="server" Font-Bold="false" Font-Size="XX-Small"
                                                Text="(dd/mm/yyyy)"></asp:Label>
                                            <asp:RequiredFieldValidator ID="rfvMarriageDate" runat="server" ControlToValidate="txtMarriageDate"
                                                CssClass="labelError" Display="None" ErrorMessage="Please enter Date of Marriage"
                                                SetFocusOnError="True" ValidationGroup="Dep">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="rfvMarriageDate_ValidatorCalloutExtender" runat="server"
                                                Enabled="True" TargetControlID="rfvMarriageDate">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                         <td align="center" >
                                            DateOfBirth<br /><asp:Label ID="Label5" runat="server" Font-Bold="false" Font-Size="XX-Small"
                                                Text="(dd/mm/yyyy)"></asp:Label>
                                            <asp:RequiredFieldValidator ID="Rfv_TxtDateOfBirth" runat="server" ErrorMessage="Please Enter DateOfBirth"
                                                Display="None" CssClass="labelError" ValidationGroup="Dep" ControlToValidate="txtDateOfBirth"
                                                SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                                TargetControlID="Rfv_TxtDateOfBirth" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>                                        
                                          <td align="center" >
                                            Age
                                            <asp:RequiredFieldValidator ID="Rfv_TxtDepAge" runat="server" ErrorMessage="Please Enter Age"
                                                ControlToValidate="txtAge" CssClass="labelError" ValidationGroup="Dep" Display="None"
                                                SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True"
                                                TargetControlID="Rfv_TxtDepAge" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                           <td align="center">
                                            AgeType
                                            <asp:RequiredFieldValidator ID="RFV_TxtAgeType" runat="server" ErrorMessage="Please Enter Age"
                                                ControlToValidate="txtAgeType" ValidationGroup="Dep" CssClass="labelError" Display="None"
                                                SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender9" runat="server" Enabled="True"
                                                TargetControlID="RFV_TxtAgeType" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td align="left">
                                            <asp:TextBox ID="txtName" runat="server" CssClass="textBox" TabIndex="12" 
                                                 Width="200px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                TargetControlID="txtName" ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlRelation" runat="server" CssClass="dropdownRelation" 
                                                TabIndex="13" AutoPostBack="True" 
                                                onselectedindexchanged="ddlRelation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                         <td align="left">
                                            <asp:TextBox ID="txtGender" runat="server" CssClass="textBoxDate" TabIndex="14" 
                                                 Width="100px"></asp:TextBox>
                                        </td>
                                           <td align="left" id="tdtxtmarriage" runat="server" visible="false" >
                                            <asp:TextBox ID="txtMarriageDate" runat="server" CssClass="textBoxDate" 
                                                   Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtMarriageDate_CalendarExtender" runat="server" Enabled="True" 
                                                Format="dd/MM/yyyy" TargetControlID="txtMarriageDate">
                                            </asp:CalendarExtender>
                                        </td>
                                           <td align="left">
                                            <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="textBoxDate" TabIndex="16"
                                                ControlToValidate="txtDateOfBirth" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                TargetControlID="txtDateOfBirth" TodaysDateFormat="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="True"
                                                TargetControlID="txtDateOfBirth" WatermarkText="dd/mm/yyyy">
                                            </asp:TextBoxWatermarkExtender>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtAge" runat="server" CssClass="textBoxDate" TabIndex="17" 
                                                Width="70px"></asp:TextBox>
                                        </td>
                                         <td align="left">
                                            <asp:TextBox ID="txtAgeType" runat="server" CssClass="textBoxDate" TabIndex="18" 
                                                 Width="70px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    </table>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="7" bgcolor="White">
                            <asp:Button ID="btnAddDependents" runat="server" Text="Add Dependent" CssClass="button"
                                Width="150px" TabIndex="15" ValidationGroup="Dep" OnClick="btnAddDependents_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%--<tr>
            <td align="center">
                <table class="label" style="width: 919px; height: 95px; background-color: #B2B2B2"
                    border="1">
                    <tr>
                        <td align="center">
                            <table style="width: 902px; height: 60px;">
                                <tr>
                                    <td>
                                        Name
                                    </td>
                                    <td>
                                        Relationship
                                    </td>
                                    <td>
                                        Gender
                                    </td>
                                    <td>
                                        DateOfBirth<asp:Label ID="Label2" runat="server" Font-Bold="false" Font-Size="XX-Small"
                                            Text="(dd/mm/yyyy)"></asp:Label>
                                    </td>
                                    <td>
                                        Age
                                    </td>
                                    <td>
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
                                        <asp:CalendarExtender ID="txtDateOfBirth_CalendarExtender" runat="server" Enabled="True"
                                            Format="dd/MM/yyyy" TargetControlID="txtDateOfBirth" TodaysDateFormat="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:TextBoxWatermarkExtender ID="txtDateOfBirth_TextBoxWatermarkExtender" runat="server"
                                            Enabled="True" TargetControlID="txtDateOfBirth" WatermarkText="dd/mm/yyyy">
                                        </asp:TextBoxWatermarkExtender>
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
                            <asp:Button ID="btnAddDependents" runat="server" Text="Add Dependent" CssClass="button"
                                Width="150px" TabIndex="15" ValidationGroup="Dep" OnClick="btnAddDependents_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblDependentsDetails" runat="server" Text="DEPENDENTS DETAILS" CssClass="label"
                    Font-Underline="True"></asp:Label><br />
                <asp:GridView ID="grdDependentdetails" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="826px" AutoGenerateColumns="False" Font-Names="Verdana"
                    Font-Size="Small" Height="5px" BorderColor="Navy" BorderWidth="1px">
                    <RowStyle BackColor="#EFF3FB" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" />
                        <asp:BoundField DataField="Membername" HeaderText="Name" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Relation" HeaderText="Relationship" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Dateofbirth" HeaderText="DateOfBirth" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="age" HeaderText="Age" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="agetype" HeaderText="AgeType" ItemStyle-HorizontalAlign="Left" />
                     <%--   <asp:BoundField DataField="AuditTrail" HeaderText="AuditTrail" />--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnModify" runat="server" OnClick="Modify_OnClick" Text="Modify"
                                    Font-Names="Verdana" Font-Size="Small"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" OnClick="Delete_OnClick" Text="Delete"
                                    Font-Names="Verdana" Font-Size="Small"></asp:LinkButton>
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete"
                                    ConfirmText="Are you sure to Delete the dependent?">
                                </asp:ConfirmButtonExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center" class="style6">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="button" TabIndex="16" OnClientClick="DisableParentsButton()" UseSubmitBehavior="false"  
                    OnClick="btnConfirm_Click" ValidationGroup="main" />
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;
            </td>
        </tr>
      
        <tr>
            <td align="center">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="EmpAge" runat="server" />
    <asp:HiddenField ID="TodayDate" runat="server" />
</asp:Content>
