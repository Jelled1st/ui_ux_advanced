using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReservationObject", menuName = "ScriptableObjects/Reservation", order = 2)]
public class Reservation : ScriptableObject , IComparable
{
    public string madeBy;
    public DateTime startTime;
    public int startDateYear = DateTime.Now.Year;
    public int startDateMonth = DateTime.Now.Month;
    public int startDateDay = DateTime.Now.Day;
    public int startDateHour = DateTime.Now.Hour;
    public int startDateMinute = DateTime.Now.Minute;
    public DateTime endTime;
    public int endDateYear = DateTime.Now.Year;
    public int endDateMonth = DateTime.Now.Month;
    public int endDateDay = DateTime.Now.Day;
    public int endDateHour = DateTime.Now.Hour;
    public int endDateMinute = DateTime.Now.Minute;

    public Reservation(DateTime pStart, DateTime pEnd)
    {
        madeBy = "";
        startTime = pStart;
        endTime = pEnd;
    }

    public Reservation(string pBy, DateTime pStart, DateTime pEnd)
    {
        madeBy = pBy;
        startTime = pStart;
        endTime = pEnd;
    }

    public bool Init()
    {
        startTime = new DateTime(startDateYear, startDateMonth, startDateDay, startDateHour, startDateMinute, 0);
        endTime = new DateTime(endDateYear, endDateMonth, endDateDay, endDateHour, endDateMinute, 0);
        RestrictDate(startTime);
        RestrictDate(endTime);

        if (endTime <= startTime) return false;
        else return true;
    }

    override public string ToString()
    {
        return "Reservation by: " + madeBy + " starts at: " + startTime.ToString() + " ends at: " + endTime.ToString();
    }

    static public DateTime RestrictDate(DateTime date)
    {
        int month = date.Month;
        int day = date.Day;
        int hour = date.Hour;
        int minute = date.Minute;
        int second = date.Second;

        if (month > 12) month = 1;
        if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
        {
            if (day > 31)
            {
                day = 1;
            }
        }
        else if (month == 2)
        {
            if (day > 28)
            {
                day = 1;
            }
        }
        else
        {
            if (day > 30)
            {
                day = 1;
            }
        }
        if (hour > 24) hour = 1;
        if (minute > 60) minute = 0;
        if (second > 60) second = 0;
        date = new DateTime(date.Year, month, day, hour, minute, second);
        return date;
    }

    static public DateTime GetDateTimeAfterTime(DateTime dt0, DateTime dt1)
    {
        int year = dt0.Year;
        int month = dt0.Month;
        int day = dt0.Day;
        int hour = dt0.Hour + dt1.Hour;
        int minute = dt0.Minute + dt1.Minute;
        int second = dt0.Second + dt1.Second;
        if(second >= 60)
        {
            minute++;
            second -= 60;
        }
        if(minute >= 60)
        {
            hour++;
            minute -= 60;
        }
        if(hour >= 24)
        {
            day++;
            hour -= 24;
        }
        if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
        {
            if (day > 31)
            {
                day = 1;
                month++;
            }
        }
        else if (month == 2)
        {
            if (day > 28)
            {
                day = 1;
                month++;
            }
        }
        else
        {
            if (day > 30)
            {
                day = 1;
                month++;
            }
        }

        if(month > 12)
        {
            month = 1;
            year++;
        }
        return new DateTime(year, month, day, hour, minute, second);
    }

    public Reservation Copy()
    {
        Debug.Log("before copy: " + startTime.ToString() + " End " + endTime.ToString());

        DateTime start = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);
        DateTime end = new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, endTime.Second);
        
        Reservation reservation = new Reservation(madeBy, start, end);

        Debug.Log("Copy: " + reservation.startTime.ToString() + " End " + reservation.endTime.ToString());

        return reservation;
    }

    static public String TimeToString(DateTime datetime)
    {
        return "" + datetime.Hour + ":" + datetime.Minute;
    }

    public int CompareTo(object obj)
    {
        if (obj is Reservation)
        {
            Reservation rObj = (Reservation)obj;
            return this.startTime.CompareTo(rObj.startTime);
            //int retV = 0;
            //string time = "" + startTime.Hour + "" + startTime.Minute;
            //return Int32.Parse(time);
            //if (startTime > rObj.startTime) retV = 0;
            //else retV = 1;
            //return retV;
        }
        else return 0;
        throw new NotImplementedException();
    }
}
