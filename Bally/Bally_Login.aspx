<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Bally_Login.aspx.cs" Inherits="Bally_Login" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="StyleSheet.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <br />
    <br />
    <asp:Label ID="lblError" runat="server" CssClass="labelError"></asp:Label>
    <br />
    <table style="width: 530px; height: 266px; border-color: #003399">
        <tr>
            <td align="center" valign="middle">
                <br />
                <table cellpadding="0" cellspacing="0" style="width: 327px; height: 149px; background-color: aliceblue">
                    <tr>
                        <td align="center">
                            <table cellpadding="0" cellspacing="0" style="width: 285px; height: 120px">
                                <tr>
                                    <td align="center" colspan="2" style="height: 21px" valign="top">
                                        <asp:Label ID="Label3" runat="server" BackColor="SteelBlue" Font-Bold="True" Font-Names="Verdana"
                                            Font-Size="Small" ForeColor="White" Height="18px" Text="Employee Log In" Width="281px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small"
                                            ForeColor="Navy" Text="User Name"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="textBox" Width="150px" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName"
                                            CssClass="labelError" ErrorMessage="Please Enter User Name" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvUserName_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvUserName" Width="200px">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small"
                                            ForeColor="Navy" Text="Password"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textBox"
                                            Width="150px" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                            CssClass="labelError" ErrorMessage="Please Enter Password" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvPassword_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvPassword" Width="200px">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="height: 16px">
                                        <asp:Button ID="btnLogIn" runat="server" Height="25px" Text="Log In" CssClass="button"
                                            OnClick="btnLogIn_Click" TabIndex="2" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
</asp:Content>
