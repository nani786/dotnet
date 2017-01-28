<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
        <center>
            <table bgcolor="white" style="width: 990px; height: 550px; margin-top: 0px;" border="0"
                cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" class="style2">
                        <asp:Label ID="lblError" runat="server" CssClass="labelError"></asp:Label>
                        <br />
                        <table style="width: 561px; height: 262px; border-color: #003399">
                            <tr>
                                <td align="center" valign="middle">
                                    <br />
                                    <table cellpadding="0" cellspacing="0" style="width: 433px; height: 209px; background-color: #C0C0C0"
                                        class="label">
                                        <tr>
                                            <td align="center" style="backcolor: #333333; background-color: #333333; color: #FFFFFF;"
                                                class="style1">
                                                CHANGE PASSWORD HERE
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table cellpadding="0" cellspacing="0" style="width: 336px; height: 137px">
                                                    <%-- <tr>
                                                        <td align="center" colspan="2" style="height: 21px" valign="top">
                                                            <asp:Label ID="Label3" runat="server" BackColor="#333333" Font-Bold="True" Font-Names="Verdana"
                                                                Font-Size="Small" ForeColor="White" Height="18px" Text="Employee Log In" 
                                                                Width="281px"></asp:Label>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="Label1" runat="server" Text="Old Password"></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtOldPassword" runat="server" CssClass="textBox" Width="150px"
                                                                TabIndex="1" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ControlToValidate="txtOldPassword"
                                                                CssClass="labelError" ErrorMessage="Please Enter User Name" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="rfvOldPassword_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" TargetControlID="rfvOldPassword">
                                                            </asp:ValidatorCalloutExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="Label2" runat="server" Text="New Password"></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="textBox"
                                                                Width="150px" TabIndex="2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtNewPassword_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtNewPassword" ValidChars="0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWZYZ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtNewPassword"
                                                                CssClass="labelError" ErrorMessage="Please Enter Password" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="rfvPassword_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" TargetControlID="rfvPassword">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:CompareValidator ID="cvOldVsNew" runat="server" ControlToCompare="txtOldPassword"
                                                                ControlToValidate="txtNewPassword" CssClass="labelError" Display="None" ErrorMessage="Old Password and New Password should not be same"
                                                                SetFocusOnError="True" Operator="NotEqual">*</asp:CompareValidator>
                                                            <asp:ValidatorCalloutExtender ID="cvOldVsNew_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" TargetControlID="cvOldVsNew">
                                                            </asp:ValidatorCalloutExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="Label3" runat="server" Text="Confirm Password"></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password" CssClass="textBox"
                                                                Width="150px" TabIndex="2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtConfirmPwd_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtConfirmPwd" ValidChars="0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWZYZ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtConfirmPwd"
                                                                CssClass="labelError" ErrorMessage="Please Enter Password" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:CompareValidator ID="cvNewVsConfirm" runat="server" ControlToCompare="txtNewPassword"
                                                                ControlToValidate="txtConfirmPwd" CssClass="labelError" Display="None" ErrorMessage="New Password and Confirm Password should be same"
                                                                SetFocusOnError="True">*</asp:CompareValidator>
                                                            <asp:ValidatorCalloutExtender ID="cvNewVsConfirm_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" TargetControlID="cvNewVsConfirm">
                                                            </asp:ValidatorCalloutExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2" style="height: 16px">
                                                            <asp:Button ID="btnChange" runat="server" Height="25px" Text="Change" TabIndex="2"
                                                                CssClass="button" OnClick="btnChange_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <tr>
                                                    <td align="left" class="data" colspan="2">
                                                        <ul>
                                                            <li>Password length should be 6 to 10 characters</li>
                                                            <li>Password can contain only numbers, lower alphabet and upper alphabet. Special symbols
                                                                are not allowed</li>
                                                        </ul>
                                                    </td>
                                                </tr>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>

