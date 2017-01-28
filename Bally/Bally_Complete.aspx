<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Bally_Complete.aspx.cs" Inherits="Bally_Complete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="StyleSheet.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="300" style="height: 450px">
        <tr>
            <td align="center">
                <table class="table" border="3">
                    <tr>
                        <td align="center" class="label">
                            Thank for enrolling under  Bally technology  group mediclaim policy 2013-14. You will receive a confirmation mailer with details of registration within 24 hours.<br />
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
