using System;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static string MonthToString(int month)
    {
        switch (month)
        {
            case 1:
                return "January";
            case 2:
                return "February";
            case 3:
                return "March";
            case 4:
                return "April";
            case 5:
                return "May";
            case 6:
                return "June";
            case 7:
                return "July";
            case 8:
                return "August";
            case 9:
                return "September";
            case 10:
                return "October";
            case 11:
                return "November";
            case 12:
                return "December";
            default:
                return "" + month;
        }
    }

    public static void SetDateInDateTime(DateTime dateTime, int year, int month, int day)
    {
        dateTime = new DateTime(year, month, day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }

    public static void SetTimeInDateTime(DateTime dateTime, int hour, int min, int sec)
    {
        dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, min, sec);
    }

    public static string HourMinToString(int hour, int min)
    {
        string ret;

        if(min < 10)
        {
            ret = "" + hour + ":0" + min;
        }
        else
        {
            ret = "" + hour + ":" + min;
        }

        return ret;
    }

    public static bool IsValidDate(int year, int month, int day)
    {
        DateTime t = DateTime.Today;
        if (year < t.Year) return false;
        if (month < 1 || month > 12) return false;
        if (day < 1 || day > 31) return false;
        return true;
    }

    public static bool isValidTime(int hour, int min, int sec)
    {
        if (hour < 0 || hour > 24) return false;
        if (min < 0 || min > 60) return false;
        if (sec < 0 || sec > 60) return false;
        return true;
    }

    public static bool isValidDateTime(int year, int month, int day, int hour, int min, int sec)
    {
        return IsValidDate(year, month, day) && isValidTime(hour, min, sec);
    }
}
