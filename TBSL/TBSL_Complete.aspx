<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TBSL_Complete.aspx.cs" Inherits="TBSL_Complete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="StyleSheet.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="300" style="height: 450px">
        <tr>
            <td align="center">
                <table class="table" border="3">
                    <tr>
                        <td align="center" class="Message">
                            Thank you for registering under TBSL Medical Insurance Program.  You will receive a confirmation mail shortly.<br />
                            <br />
                            <br />
                            <asp:Button ID="BtnCompleted" runat="server" CssClass="button" OnClick="BtnCompleted_Click"
                                Text="Completed" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
