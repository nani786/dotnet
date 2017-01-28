<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgetPasswordChange.aspx.cs"
    Inherits="ForgetPasswordChange" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

  
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .cssTextBox
        {
            margin-left: 0px;
        }
        .style3
        {
            width: 150px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <table cellpadding="0" cellspacing="0" width="900px" style="height: 600px">
            <tr>
                <td align="center" style="height: 50px; background-image: url(images/header-top.jpg);">
                    <table cellpadding="0" cellspacing="0" style="width: 990px; height: 65px;">
                        <tr>
                            <td align="left">
                                <img src="Images/logo.png" alt="" />
                            </td>
                            <td align="left" style="height: 65px">
                                <asp:Label ID="lblHeading" runat="server" Text=""
                                    Font-Bold="True" Font-Names="Arial" Font-Size="Large" ForeColor="White" Width="505px"
                                    Font-Italic="False"></asp:Label>
                            </td>
                            <%-- <td align="right" style="height: 65px; padding-right: 10px; padding-top: 5px;">
                                    <img src="Images/TataElxsiLogo.jpg" />
                                </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 3px;">
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 15px; background-color: #333333;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblError" runat="server" CssClass="labelError" ForeColor="Red"></asp:Label><br />
                    <table cellpadding="0" cellspacing="0" style="height: 214px; width: 411px;">
                        <tr>
                            <td align="center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="background-color: #CCCCCC;">
                                <table id="tblLogin" runat="server" style="width: 372px; height: 150px;">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblLogin" runat="server" Text="CHANGE PASSWORD" CssClass="label" Font-Bold="true"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <
                                    <tr>
                                        <td align="left" class="style3">
                                            <asp:Label ID="lblNewPassword" runat="server" Text="New Password" CssClass="label" />
                                            &nbsp;
                                            <asp:CompareValidator ID="cmv_NewPassword" runat="server" ControlToCompare="txtNewPassword"
                                                ControlToValidate="txtConfirmPassword" ErrorMessage="New password and confirm password should be same "
                                                CssClass="cssErrLabel" ValidationGroup="Update" Display="None">@</asp:CompareValidator>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="cssTextBox" TabIndex="1"
                                                AutoCompleteType="Disabled" MaxLength="15" TextMode="Password" />
                                            <asp:RequiredFieldValidator ID="rfvtxtNewPassword" runat="server" ControlToValidate="txtNewPassword"
                                                ErrorMessage="Please enter New Password" Display="None" ForeColor="Red" SetFocusOnError="True"
                                                ValidationGroup="Update">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="rfvtxtNewPassword_ValidatorCalloutExtender" runat="server"
                                                Enabled="True" TargetControlID="rfvtxtNewPassword">
                                            </asp:ValidatorCalloutExtender>
                                            <%--  <asp:RegularExpressionValidator ID="REVTxtNewpassword" runat="server" ControlToValidate="txtNewPassword"
                                                ErrorMessage="Must have at least 1 number, 1 special character,
                                              and more than 6 characters." ValidationExpression="(?=^.{6,}$)(?=.*\d)(?=.*\W+)(?![.\n]).*$"
                                                CssClass="cssErrLabel" ValidationGroup="Update" ForeColor="Red">*</asp:RegularExpressionValidator>--%>
                                            <asp:RegularExpressionValidator ID="REVTxtNewpassword" runat="server" ControlToValidate="txtNewPassword"
                                                ErrorMessage="Password guidelines: Must be at least 6 characters length; Must contain at least 1 numeric value; Must contain at least 1 special character;"
                                                ValidationExpression="(?=^.{6,}$)(?=.*\d)(?=.*\W+)(?![.\n]).*$" CssClass="cssErrLabel"
                                                ValidationGroup="Update" ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="REVTxtNewpassword_ValidatorCalloutExtender" runat="server"
                                                Enabled="True" TargetControlID="REVTxtNewpassword">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style3">
                                            <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password" CssClass="label" />
                                            &nbsp; &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="cssTextBox" TabIndex="2"
                                                AutoCompleteType="Disabled" MaxLength="15" TextMode="Password" />
                                            <asp:RequiredFieldValidator ID="rfvEmployeeID" runat="server" ControlToValidate="txtConfirmPassword"
                                                ErrorMessage="Please enter Confirm Password" ForeColor="Red" Display="None" SetFocusOnError="True"
                                                ValidationGroup="Update">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="rfvEmployeeID_ValidatorCalloutExtender" runat="server"
                                                Enabled="True" TargetControlID="rfvEmployeeID">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:CompareValidator ID="cmptxtConfirmPassword" CssClass="labelError" runat="server"
                                                ErrorMessage="Your passwords do not match" ControlToValidate="txtConfirmPassword"
                                                ControlToCompare="txtNewPassword" ForeColor="Red">*</asp:CompareValidator>
                                            <asp:ValidatorCalloutExtender ID="cmptxtConfirmPassword_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="cmptxtConfirmPassword">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Update" Width="80px"
                                                TabIndex="4" OnClick="btnSubmit_Click" ValidationGroup="Update" />
                                            &nbsp;&nbsp; &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="Button" Style="visibility: hidden;" />
                                <asp:ModalPopupExtender ID="mpePopup" runat="server" DynamicServicePath="" Enabled="True"
                                    TargetControlID="Button1" PopupControlID="pnlPasswordChange" BackgroundCssClass="modalPopup">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="pnlPasswordChange" runat="server" Height="150px" Width="400px" BackColor="white">
                                    <center>
                                        <table cellpadding="0" cellspacing="0" style="font-size: small; font-family: Verdana;
                                            height: 150px; width: 400px;">
                                            <tr>
                                                <td align="center">
                                                    Your Password Updated Successfully.
                                                    <br />
                                                    <br />
                                                    Please click here to go to login page:
                                                    <asp:LinkButton ID="lbLogin" runat="server" onclick="lbLogin_Click">Login</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 25px; background-color: #333333;" class="data">
                    <marquee id="scroll_news" style="color: White; font-family: Book Antiqua; font-weight: bold"
                        behavior="alternate" scrollamount="4" onmouseover="document.getElementById('scroll_news').stop();"
                        onmouseout="document.getElementById('scroll_news').start();">An ISO 9001:2008 Certified TPA</marquee>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
