<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default25.aspx.cs" Inherits="Default25" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register namespace="TestEditor"  tagprefix="custom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
 
         <table class="style1">
             <tr>
                 <td align="center">
                     Add Appreciations Here</td>
             </tr>
             <tr>
                 <td>
                     &nbsp;</td>
             </tr>
             <tr>
                 <td align="center" >
 
         <custom:Class1 ID="editor"  Width="450px" Height="200px" runat="server" />
    
                 </td>
             </tr>
             <tr>
                 <td>
                     &nbsp;</td>
             </tr>
             <tr>
                 <td align="center">
    
    <asp:Button ID="btnSave" runat="server" Text="Button" onclick="btnSave_Click" />
                 </td>
             </tr>
    </table>
    </form>
</body>
</html>
