<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bally_Lock.aspx.cs" Inherits="Bally_Lock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lock</title>
    <link href="StyleSheet.css" type="text/css" rel="Stylesheet" />
</head>
<body style="margin-top: 0; margin-left: 0; margin-right: 0" bgcolor="#F0F8FF">
    <form id="form1" runat="server">
    <div>
        <center>
            <table bgcolor="white" style="width: 990px; height: 550px" border="0" cellpadding="0"
                cellspacing="0">
                <tr>
                    <td align="center" style="height: 50px; background-image: url(images/TopHead.jpg);">
                        <table cellpadding="0" cellspacing="0" style="width: 990px; height: 65px;">
                            <tr>
                                <td align="left">
                                    <img src="Images/Logo_Gif.JPG" alt="" />
                                </td>
                                <td align="center" style="height: 65px">
                                    <asp:Label ID="lblHeading" runat="server" Text="Label" Font-Bold="True" Font-Names="Verdana"
                                        Font-Size="Large" ForeColor="Navy" Width="550px"></asp:Label>
                                </td>
                                <td align="right" style="height: 65px">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="background-image: url(images/MasterHeadDown.jpg); height: 20px;">
                      
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
                <tr>
                    <td style="height: 19px; background-image: url('images/Footer.jpg')" class="data">
                        <marquee id="scroll_news" style="color: #FFFFFF; font-family: Book Antiqua; font-weight: bold"
                            behavior="alternate" scrollamount="4" onmouseover="document.getElementById('scroll_news').stop();"
                            onmouseout="document.getElementById('scroll_news').start();">An ISO 9001:2008 Certified TPA</marquee>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
