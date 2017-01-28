<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerSpeaks.aspx.cs" Inherits="CustomerSpeaks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>FHPL:::</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="js/chrome.js" type="text/javascript"></script>
    <script src="js/ThoughtOfDay.js" type="text/javascript"></script>
    <script src="js/CustomerSpeaks.js" type="text/javascript"></script>
    <style>
        .leftColInner p
        {
            padding-top: 8px;
            text-align: justify;
        }
        .psignature
        {
            padding-top: 4px;
            text-align: right;
        }
        font
        {
            color:#333;
        }
        span
        {
              color:#333;
        }
    </style>
</head>
<body id="index">
    <div id="wrapper">
        <!--header-->
        <div class="header">
            <div class="logo">
                <a href="index.html">
                    <img src="images/logo.gif" alt="FHPL" /></a></div>
            <div class="floatR">
                <div class="topnav">
                    <div class="number">
                        1-800-425-4033</div>
                    <div id="chromemenu" class="floatL">
                        <div class="qLinks">
                            <ul>
                                <li><a href="#" rel="dropmenu1"><span>Quick Links</span></a></li></ul>
                        </div>
                        <div class="Logins">
                            <ul>
                                <li><a href="#" rel="LoginDropMenu"><span>Logins</span></a></li></ul>
                        </div>
                        <!--1st drop down menu -->
                        <div id="dropmenu1" class="dropmenudiv">
                            <a href="DynamicPages/ClaimOnlineIntimation.aspx">Claim Intimation</a> <a href="DynamicPages/FeedBackCredentials.aspx">
                                Feed Back</a> <a href="DynamicPages/Enquiry.aspx">Enquiry</a>
                            <!--<a href="service-request.html">
                                Service Request </a>-->
                            <a href="https://mail.fhpl.net" target="_blank">Email</a> <a href="GuideBook.htm">Guide
                                Book</a> <a href="DynamicPages/ClaimTracker.aspx">SMS </a><a href="sitemap.html"
                                    class="noBordB">Site Map</a>
                        </div>
                        <!--Login drop down menu -->
                        <div id="LoginDropMenu" class="dropmenudiv">
                            <!-- <a href="DynamicPages/Login.aspx?ID=1">Administrator</a>-->
                            <a href="DynamicPages/InsuranceCompanies.aspx">Insurer</a><a href="http://m.fhpl.net/">
                                Employee</a><a href="http://m.fhpl.net/">HR</a> <a href="../FhplLogins/Corporate/Login.aspx">
                                    Corporate</a> <a href="../FhplLogins/Ecard/Login.aspx?Type=ecard">E-Card</a>
                            <a href="NetworkHospitals/ProviderLogin.aspx">Hospital</a>
                            <!-- <a href="https://mail.fhpl.net"
                                        class="noBordB" target="_blank">Email</a>-->
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div class="tabs">
                    <div class="ncl">
                    </div>
                    <ul>
                        <li><a href="index.html">Home</a> </li>
                        <li><a href="about-us.html">About Us</a></li>
                        <li><a href="forms.html">Forms</a></li>
                        <li><a href="call-center.html">Call Center</a></li>
                        <li><a href="DynamicPages/Careers.aspx">Careers</a></li>
                        <li><a href="faqs.html">Faqs</a></li>
                        <li class="none"><a href="contact-us.html">Contact Us</a> </li>
                    </ul>
                    <div class="ncr">
                    </div>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="bannerInner relative">
            <h4>
                Integration of best technology with process</h4>
            <div class="iso2">
                An ISO/IEC 27001:2013 and ISO 9001:2008 Certified TPA</div>
        </div>
        <!--header-->
        <div class="clear">
        </div>
        <!--content-->
        <div class="content">
            <!-- <div class="leftColInner" style="background-image: url(images/aboutus1.jpg);">-->
            <div class="leftColInner">
                <div class="head">
                    <h1>
                        Customer Speaks</h1>
                    <div class="breadcrumb">
                        <a href="index.html">Home</a><span> / </span>Customer Speaks</div>
                </div>
               
                <br />
                <div class="clear">
                </div><p>
                <asp:DataList ID="dlstAppreciation"  RepeatColumns="1" Width=" 650px" 
                    runat="server" RepeatDirection="Horizontal" >
                    <ItemTemplate >
                     <div class="clear">
                </div>               
                            <%#Eval("appreciation")%>
                <p style="border-bottom: 1px; border-bottom-style: dashed;">
                </p>
                    
                    </ItemTemplate>
                </asp:DataList>
               
            </div>
            <!--Right Column-->
            <div class="rightCol">
                <!--<div class="row">
                    <div class="bg noPaddB">
                        <h3>
                            Login</h3>
                        <div class="login">
                            <div class="box">
                                <label class="left">
                                    Login Type</label>
                                <label class="floatL">
                                    <select name="select" id="select">
                                        <option>Administrator</option>
                                    </select></label>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="box">
                                <label class="left">
                                    USER ID</label>
                                <label class="floatL">
                                    <input type="text" />
                                </label>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="box">
                                <label class="left">
                                    PASSWORD</label>
                                <label class="floatL">
                                    <input type="text" />
                                </label>
                            </div>
                            <div class="clear">
                            </div>
                            <input name="Submit" type="image" class="floatR" style="border: none; height: 19px;
                                width: 32px; margin: 3px 0 0 0;" onclick="return check_form();" value="Submit"
                                src="images/go.gif" alt="submit" />
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <div class="curB">
                    </div>
                </div>
                <div class="clear">
                </div>-->
                <div class="emenumbers">
                    <img src="images/numbers.gif" alt="" /></div>
                <div class="row">
                    <div class="bg">
                        <h3>
                            thought of the day
                        </h3>
                        <div class="news">
                            <marquee onmouseout="this.start()" onmouseover="this.stop()" scrolldelay="35" scrollamount="1"
                                truespeed="1" direction="up">

    <p>
    
    <label id="lblThought"></label>
     <script type="text/javascript">
         ReplaceText();
     </script>

</p>
     <!-- <p>
<span class="dBlue">FHPL NEWS</span>  <span class="font11">01-Dec-11</span><br />
  We invite your comments and feed back on the website.</p>-->
    
    

<div class="clear"></div></marquee>
                            <div class="clear">
                            </div>
                            <!--  <div class="button floatR">
                                <a href="news-announcements.html"><span>View all news...</span></a>
                                <div class="cR">
                                </div>
                            </div>-->
                        </div>
                    </div>
                    <div class="curB">
                    </div>
                </div>
                <div class="row">
                    <div class="bg">
                        <h3>
                            Customer Speaks
                        </h3>
                        <div class="news">
                            <marquee onmouseout="this.start()" onmouseover="this.stop()" scrolldelay="35" scrollamount="1"
                                truespeed="1" direction="up">

    <p>
     <label id="lblCustomer"></label>
     <script type="text/javascript">
         CustomerText();
     </script>
    <a href="CustomerSpeaks.html" style="color: Blue;
                        text-decoration: none;">Read More..</a>
</p>
     
    

<div class="clear"></div>
</marquee>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <div class="curB">
                    </div>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <!--Footer-->
        <div id="footer">
            <div class="bottomnav">
                <div class="bBox">
                    <ul>
                        <li><a href="index.html">Home</a></li>
                        <li><a href="about-us.html">About Us</a></li>
                        <li><a href="forms.html">Forms</a></li>
                    </ul>
                </div>
                <div class="bBox">
                    <ul>
                        <li><a href="call-center.html">Call Center</a></li>
                        <li><a href="DynamicPages/Careers.aspx">Careers</a> </li>
                        <li><a href="faqs.html">FAQs</a></li>
                    </ul>
                </div>
                <div class="bBox">
                    <ul>
                        <li><a href="DynamicPages/ClaimOnlineIntimation.aspx">Claim Intimation</a></li>
                        <li><a href="calculators.html">Calculators</a><a href="#"></a></li>
                        <li><a href="DynamicPages/FeedBackCredentials.aspx">Feedback</a></li>
                    </ul>
                </div>
                <div class="bBox">
                    <ul>
                        <li><a href="DynamicPages/Enquiry.aspx">Enquiry </a></li>
                        <!-- <li><a href="service-request.html">Service Request</a></li>-->
                        <li><a href="sitemap.html">Sitemap</a></li>
                        <li><a href="contact-us.html">Contact Us</a></li>
                    </ul>
                </div>
                <!-- <div class="bBox">
                <ul>
                    <li><a href="contact-us.html">Contact Us</a></li>
                    <li><a href="disclaimer.html">Disclaimer</a></li>
                    <li><a href="privacy-policy.html">Privacy Policy</a></li>
                </ul>
            </div>-->
                <div class="bBox">
                    <ul>
                        <li><a href="DynamicPages/Grievance.aspx">Grievance</a></li>
                        <li><a href="NetworkHospitals/NWHospitals.aspx">Network Hospitals</a></li>
                    </ul>
                </div>
                <!--<div class="bBox">
                <div class="logos">
                    <a href="#"><img src="images/home.gif" alt="" /></a> 
                    <a href="http://www.fhpl.net/lichome"><img src="images/lic.gif" alt="" /></a>
                </div>
            </div>-->
                <div class="bBox noPaddR">
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="copyrights">
                © Family Health Plan (TPA) Ltd,, All Rights Reserved 2011</div>
            <div class="clear">
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript">
        cssdropdown.startchrome("chromemenu")
    </script>
</body>
</html>
