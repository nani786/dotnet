using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mail;

/// <summary>
/// Summary description for Validate
/// </summary>
public class Validate
{
    SqlConnection sCon;
    SqlCommand sCmd;
    SqlDataReader sDr;
    SqlDataAdapter sDa;
    public Validate()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    string MailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
    string SendMail = ConfigurationManager.AppSettings["SendMail"].ToString();
    string SendMailCC = ConfigurationManager.AppSettings["SendMailCC"].ToString();
    string SendMailBcc = ConfigurationManager.AppSettings["SendMailBcc"].ToString();
    string SendMailTO = ConfigurationManager.AppSettings["SendMailTO"].ToString();
    public string DOBConvertion(string dob)
    {
        string m, d, y, dobvalue;
        string[] from;
        from = dob.Split('/');
        d = from[0];
        m = from[1];
        y = from[2];
        dobvalue = m + "/" + d + "/" + y;
        return dobvalue;
    }

    public int Agetype(string age)
    {
        int age2 = 0;
        switch (age)
        {
            case "Year(s)":
                age2 = 1;
                break;
            case "Month(s)":
                age2 = 2;
                break;
            case "Day(s)":
                age2 = 3;
                break;
        }
        return age2;
    }
    public int GetGenderCode(string gend)
    {
        int gencode = 0;
        switch (gend)
        {
            case "Female":
                gencode = 2;
                break;

            case "Male":
                gencode = 1;
                break;
        }
        return gencode;
    }
    public int GetRelationshipID(string relation)
    {
        int r = 0;
        switch (relation)
        {
            case "Father":
                r = 20;
                break;
            case "Mother":
                r = 3;
                break;
            case "Husband":
                r = 21;
                break;
            case "Wife":
                r = 18;
                break;
            case "Daughter":
                r = 5;
                break;
            case "Son":
                r = 4;
                break;
            case "Father in law":
                r = 12;
                break;
            case "Mother in law":
                r = 13;
                break;
        }
        return r;
    }
    public int GetSuminsuredID(string suminsured)
    {
        int r = 0;
        switch (suminsured)
        {
            case "100000":
                r = 1;
                break;
            case "125000":
                r = 2;
                break;
            case "150000":
                r = 3;
                break;
            case "175000":
                r = 4;
                break;
            case "200000":
                r = 5;
                break;
            case "250000":
                r = 6;
                break;
            case "300000":
                r = 7;
                break;
            case "350000":
                r = 8;
                break;
            case "400000":
                r = 9;
                break;
            case "450000":
                r = 10;
                break;
            case "500000":
                r = 11;
                break;
            case "700000":
                r = 12;
                break;
        }
        return r;
    }
    public void InsertTransactions(string Query, SqlConnection sCon)
    {
        try
        {
            sCmd = new SqlCommand(Query, sCon);
            sCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string ParentsAgeRange(int age)
    {
        string condition = String.Empty;
        if ((age >= 0) && (age <= 45))
            condition = "0-45";
        else if ((age >= 46) && (age <= 60))
            condition = "46-60";
        else if (age >= 60)
            condition = "Above-60";
        return condition;
    }

    public string EncryptData(string value)
    {
        byte[] encData_byte = new byte[value.Length];
        encData_byte = System.Text.Encoding.UTF8.GetBytes(value);
        value = Convert.ToBase64String(encData_byte);
        value = value + "Q";
        return value;
    }

    public string DecryptCode(string code)
    {
        code = code.Substring(0, code.Length - 1);
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        System.Text.Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(code);
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        code = new String(decoded_char);
        return code;
    }

    public void SendingMail(string Subject, string FromMailID, string TOMailID, string CCMailID, string BCCMailID, string Body)
    {
        if (SendMail.ToUpper() == "TRUE")
        {
            CCMailID = SendMailCC;
            BCCMailID = SendMailBcc;
        }
        else
        {
            TOMailID = SendMailTO;
            CCMailID = "";
            BCCMailID = "";
        }

        MailMessage msg = new MailMessage();
        msg.Subject = Subject;
        msg.To = TOMailID;
        msg.From = FromMailID;
        if (CCMailID != "")
            msg.Cc = CCMailID;
        if (BCCMailID != "")
            msg.Bcc = BCCMailID;
        msg.Body = Body;
        msg.BodyFormat = MailFormat.Html;
        SmtpMail.SmtpServer = MailServer;
        SmtpMail.Send(msg);
    }

    public string PremiumAmount(string AgeBand, int MemberCount)
    {
        if (MemberCount == 1)
        {
            if (AgeBand == "0-45")
            {
                return "2176";
            }
            else
                if (AgeBand == "46-60")
                {
                    return "2634";
                }
                else
                    if (AgeBand == "Above-60")
                    {
                        return "2977";
                    }
        }
        else
            if (MemberCount == 2)
            {
                if (AgeBand == "0-45")
                {
                    return "3435";
                }
                else
                    if (AgeBand == "46-60")
                    {
                        return "4236";
                    }
                    else
                        if (AgeBand == "Above-60")
                        {
                            return "4809";
                        }

            }
            else
                if (MemberCount > 2)
                {
                    if (AgeBand == "0-45")
                    {
                        return "4351";
                    }
                    else
                        if (AgeBand == "46-60")
                        {
                            return "5267";
                        }
                        else
                            if (AgeBand == "Above-60")
                            {
                                return "5954";

                            }

                }

        return "";

    }

}