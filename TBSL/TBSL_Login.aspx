<%@ Page Title="" Language="C#" AutoEventWireup="true"
    CodeFile="TBSL_Login.aspx.cs" Inherits="TBSL_Login" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="StyleSheet.css" type="text/css" rel="Stylesheet" />
    <style type="text/css">
        .style1
        {
            height: 27px;
        }
        .cssbutton
        {}
    </style>
</head>
<body style="margin-top: 0; margin-left: 0; margin-right: 0">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <table bgcolor="white" style="width: 1240px; height: 550px" border="0" cellpadding="0"
                cellspacing="0">
                <tr>
                    <td align="center" style="height: 50px;" bgcolor="#3366FF">
                        <table cellpadding="0" cellspacing="0" style="width: 1231px; height: 65px;">
                            <tr>
                                <td align="left" class="style1" bgcolour= bgcolor="#3366FF">
                                    <img src="Images/logo.png" alt="" />
                                </td>
                                <td align="center" style="height: 65px" bgcolor="#3366FF">
                                    <asp:Label ID="lblHeading" runat="server" Text="Tata BlueScope Steel Ltd." 
                                        Font-Bold="True" Font-Names="Verdana" 
                                        Font-Size="Large" ForeColor="White" Width="291px"></asp:Label>
                                </td>
                                <td align="right" style="height: 65px">
                                    <img src="Images/TBSL%20Logo.JPG" style="height: 78px; width: 217px" />
                                </td>
                                <td align="right" style="height: 65px">
                                    <img src="Images/Marsh%20Logo.JPG" style="height: 100px; width: 267px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="background-color: #124d77; height: 20px;">
                    </td>
                </tr>
                <tr style="background-image: url(images/bg.jpg);">
                    <td align="center">
                        <asp:Label ID="lblError" runat="server" CssClass="labelError"></asp:Label>
                        <table style="border: 0px solid #ccc; margin-top: 40px; width: 350px; height: 300px;
                            margin-bottom: 38px; border-radius: 6px; background-color: aliceblue;">
                            <tr>
                                <td align="center" style="color: #003366; border-bottom-style: solid; border-bottom-width: 1px;
                                    border-bottom-color: #000000;" class="labelHeading">
                                    LogIn
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table cellpadding="0" cellspacing="0" style="width: 272px; height: 248px" class="label">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label1" runat="server" Text="User Name" Style="color: #124d77"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName"
                                                    CssClass="labelError" ErrorMessage="Please Enter User Name" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="rfvUserName_ValidatorCalloutExtender" runat="server"
                                                    Enabled="True" TargetControlID="rfvUserName" Width="200px">
                                                </asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="textBox" Width="250px" TabIndex="1"
                                                    BorderColor="#333333" BorderWidth="1px" Height="30px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label2" runat="server" Text="Password" Style="color: #124d77"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                    CssClass="labelError" ErrorMessage="Please Enter Password" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="rfvPassword_ValidatorCalloutExtender" runat="server"
                                                    Enabled="True" TargetControlID="rfvPassword" Width="200px">
                                                </asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textBox"
                                                    Width="250px" TabIndex="2" BorderColor="Black" BorderWidth="1px" Height="30px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnLogIn" runat="server" Text="Log In" OnClick="btnLogIn_Click" TabIndex="2"
                                                    CssClass="cssbutton" Width="64px" />
                                            </td>
                                        </tr>
                                         <tr>
                                                        <td align="right" colspan="2">&nbsp;&nbsp;</td>
                                                   </tr>
                                                   <tr>
                                                        <td align="right" colspan="2">
                                                        <asp:LinkButton ID="lbForgotPassword" runat="server" CausesValidation="false" 
                                                    OnClick="lbForgotPassword_Click">Forgot Password</asp:LinkButton>
                                                        </td>
                                                  </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
               <tr>
                    <td align="center">                     
                        <asp:ModalPopupExtender ID="mpTopUp" runat="server" BackgroundCssClass="modalPopup"
                            Enabled="True" PopupControlID="pnlConfirm" TargetControlID="lbForgotPassword"
                            CancelControlID="btnClose">
                        </asp:ModalPopupExtender>                      
                        <asp:Panel ID="pnlConfirm" runat="server" Visible="false" BackColor="white" Height="170px" Width="340px">
                            <center>
                                <table cellpadding="0" cellspacing="0" style="width: 344px; height: 171px; background-color: #C0C0C0"
                                    class="label">
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="btnClose" runat="server" CssClass="button" Text="X" ForeColor="Red" />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <br />
                                            <asp:Label ID="lblPopUpError" runat="server" CssClass="labelError"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">
                                            &nbsp;&nbsp; Employee ID
                                        </td>
                                        <td align="left" class="style3">
                                            &nbsp;
                                            <asp:TextBox ID="txtEmployeeID" runat="server" AutoCompleteType="Disabled" TabIndex="3"
                                                CssClass="textBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtEmployeeID" runat="server" ErrorMessage="Please Enter EmployeeID"
                                                ControlToValidate="txtEmployeeID" SetFocusOnError="true" Display="None" CssClass="labelError"
                                                ValidationGroup="fpwd">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="rfvtxtEmployeeID_ValidatorCalloutExtender" PopupPosition="TopLeft"
                                                runat="server" Enabled="True" TargetControlID="rfvtxtEmployeeID">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <br />
                                            <asp:Button ID="btnSubmit" CssClass="button" runat="server" ValidationGroup="fpwd"
                                                Text="Submit" OnClick="btnSubmit_Click" />
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <table width="135" border="0" cellpadding="2" cellspacing="0" title="Click to Verify - This site chose VeriSign Trust Seal to promote trust online with consumers.">
                            <tr>
                                <td width="135" align="center" valign="top">
                                    <script type="text/javascript" src="https://seal.verisign.com/getseal?host_name=www.fhpl.net&amp;size=S&amp;use_flash=NO&amp;use_transparent=NO&amp;lang=en"></script>
                                    <br />
                                    <a href="http://www.verisign.com/verisign-trust-seal" target="_blank" style="color: #000000;
                                        text-decoration: none; font: bold 7px verdana,sans-serif; letter-spacing: .5px;
                                        text-align: center; margin: 0px; padding: 0px;">ABOUT TRUST ONLINE</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 19px; background-color: #3366FF;" class="data">
                        <marquee id="scroll_news" style="color: #FFFFFF; font-family: Book Antiqua; font-weight: bold"
                            behavior="alternate" scrollamount="4" onmouseover="document.getElementById('scroll_news').stop();"
                            onmouseout="document.getElementById('scroll_news').start();">ISO 9001:2008 Certified TPA</marquee>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
