<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Bally_HR.aspx.cs" Inherits="Bally_HR" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="900px" style="height: 440px">
        <tr>
            <td align="center">
                <asp:Label ID="lblError" runat="server" CssClass="labelError"></asp:Label>
                <table style="width: 806px; height: 297px; background-image: url('images/TableBack.jpg')">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label5" runat="server" Text="Unlock Employee Login" Font-Bold="True"
                                Font-Names="Verdana" Font-Size="Small" ForeColor="Navy" BorderColor="Navy" BorderWidth="2px"
                                Width="171px"></asp:Label>
                            <br />
                            <br />
                            <table style="width: 347px; height: 82px;" class="label">
                                <tr>
                                    <td align="left">
                                        EmployeeID
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtUnlockID" runat="server" CssClass="textBox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUnlockID" runat="server" ErrorMessage="Please Enter EmployeeID"
                                            ControlToValidate="txtUnlockID" CssClass="labelError">*</asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvUnlockID_ValidatorCalloutExtender" runat="server"
                                            Enabled="True" TargetControlID="rfvUnlockID">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="2">
                                        <asp:Button ID="BtnUnlock" runat="server" Text="Unlock" CssClass="button" OnClick="BtnUnlock_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnGetDetails" runat="server" Text="Get Details" CssClass="button"
                                            OnClick="btnGetDetails_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="grdEmpDetails" runat="server" AutoGenerateColumns="False" CssClass="data"
                                Width="800px" OnDataBound="grdEmpDetails_DataBound">
                                <Columns>
                                    <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
                                    <asp:BoundField DataField="MemberName" HeaderText="Employee Name" />
                                    <asp:BoundField DataField="Relation" HeaderText="Relation" />
                                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                    <%--<asp:BoundField DataField="UserID" HeaderText="User Name" />--%>
                                    <asp:BoundField DataField="Password" HeaderText="Password" />
                                    <asp:BoundField DataField="DateOfBirth" HeaderText="DOB" />
                                    <asp:BoundField DataField="Age" HeaderText="Age" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                </Columns>
                                <HeaderStyle BackColor="#000040" ForeColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
