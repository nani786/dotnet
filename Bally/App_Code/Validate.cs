using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Validate
/// </summary>
public class Validate
{
    public Validate()
    {
        //
        // TODO: Add constructor logic here
        //
    }
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
            case "Fatherinlaw":
                r = 12;
                break;
            case "Motherinlaw":
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
}