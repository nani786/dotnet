<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TBSL_Lock.aspx.cs" Inherits="TBSL_Lock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lock</title>
    <link href="StyleSheet.css" type="text/css" rel="Stylesheet" />
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
                    <td align="center" style="height: 50px;">
                        <table cellpadding="0" cellspacing="0" style="width: 1087px; height: 65px;">
                            <tr>
                                <td align="left" class="style1">
                                    <img src="Images/Logo_Gif.JPG" alt="" />
                                </td>
                                <td align="left" style="height: 65px">
                                    <asp:Label ID="lblHeading" runat="server" 
                                        Text="TBSL MEDICAL INSURANCE ENROLLMENT" Font-Bold="True" Font-Names="Verdana"
                                        Font-Size="Large" ForeColor="Navy"  Width="421px"></asp:Label>
                                </td>
                                <td align="right" style="height: 65px">
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
                      <table width="900" style="height: 450px">
                            <tr>
                                <td align="center">
                                    <table class="table" border="3">
                                        <tr>
                                            <td>
                                                <table style="height: 142px">
                                                    <tr>
                                                        <td align="left" class="Message">
                                                            Online enrollment process is closed. For any further addition/modification of dependents<br />
                                                            please contact your HR Team.<br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="Message">
                                                            Thank You
                                                            <br />
                                                            FHPL
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>  
                       
                    </td>
                </tr>
                <%--<tr style="background-image: url(images/bg.jpg);">
                    <td align="center">
                        <asp:ModalPopupExtender ID="mpTopUp" runat="server" BackgroundCssClass="modalPopup"
                            Enabled="True" PopupControlID="pnlConfirm" TargetControlID="lbForgotPassword"
                            CancelControlID="btnClose">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlConfirm" runat="server" Visible="false" BackColor="white" Height="170px"
                            Width="340px">
                            <center>
                                <table cellpadding="0" cellspacing="0" style="width: 344px; height: 171px; background-color: White;"
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
                                            <asp:Button ID="btnSubmit" CssClass="cssbutton" runat="server" ValidationGroup="fpwd"
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
                </tr>--%>
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
                    <td style="height: 19px; background-color: #124d77;" class="data">
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
