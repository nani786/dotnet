<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="Results" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="StyleSheet.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table bgcolor="white" style="width: 990px; height: 550px" border="0" cellpadding="0"
            cellspacing="0">
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
                <td align="center" valign="top">
                    <table width="900" style="height: 450px">
                        <tr>
                            <td align="center">
                                <table class="table" border="3">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" width="900px" style="height: 400px">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblError" runat="server" Text="" CssClass="cssErrLabel"></asp:Label><br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
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
