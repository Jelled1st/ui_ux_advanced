using System;
using System.Collections.Generic;
using UnityEngine;

public class DateTimeExtension : MonoBehaviour
{

    static public DateTime GetDateOfLastDay(DayOfWeek day)
    {
        DateTime today = DateTime.Today;
        if (today.DayOfWeek == day) return today;
        else if (today.DayOfWeek > day) return GetDateDaysAgo(today, today.DayOfWeek - day);
        else return GetDateInDays(today, day - today.DayOfWeek);
    }

    static public int GetDaysInMonth(int month)
    {
        if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
        {
            return 31;
        }
        else if (month == 2)
        {
            return 28;
        }
        else
        {
            return 30;
        }
    }

    static public DateTime GetDateDaysAgo(DateTime date, int daysAgo)
    {
        int newDays = date.Day - daysAgo;
        int newMonth = date.Month;
        int newYear = date.Year;
        while(newDays <= 0)
        {
            newMonth--;
            if (newMonth <= 0)
            {
                newYear--;
                newMonth = 12;
            }
            newDays = GetDaysInMonth(newMonth) + newDays; // new days was either 0 or negative, adding it will subtract or stay the same
        }
        return new DateTime(newYear, newMonth, newDays, date.Hour, date.Minute, date.Second);
    }

    static public DateTime GetDateInDays(DateTime date, int days)
    {
        int newDays = date.Day + days;
        int newMonth = date.Month;
        int newYear = date.Year;
        while (newDays > GetDaysInMonth(newMonth))
        {
            newDays -= GetDaysInMonth(newMonth);
            newMonth++;
            if(newMonth > 12)
            {
                newYear++;
                newMonth = 1;
            }
        }
        return new DateTime(newYear, newMonth, newDays, date.Hour, date.Minute, date.Second);
    }


    static public String TimeToString(DateTime time)
    {
        return "" + time.Hour + ":" + time.Minute;
    }

    static public string DateToString(DateTime date)
    {
        return "" + date.Day + "/" + date.Month;
    }
}
