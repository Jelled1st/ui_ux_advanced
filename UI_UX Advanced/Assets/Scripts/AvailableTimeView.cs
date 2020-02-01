using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableTimeView : MonoBehaviour
{
    [SerializeField] public Room room;
    [SerializeField] public Reservation reservation;
    [SerializeField] Text day;
    [SerializeField] Text date;
    [SerializeField] Text roomNumber;
    [SerializeField] Text time;
    [SerializeField] Text charsAndTimeFrame;
    [SerializeField] GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoomReservation(Room room, Reservation res)
    {
        this.room = room;
        this.reservation = res;
    }

    public void SetAllText()
    {
        day.text = reservation.startTime.DayOfWeek.ToString();
        date.text = reservation.startTime.Day + " " + Helper.MonthToString(reservation.startTime.Month);
        roomNumber.text = room.roomNumber;
        time.text = reservation.startTime.Hour + ":" + reservation.startTime.Minute + " - " + reservation.endTime.Hour + ":" + reservation.endTime.Minute;
        TimeSpan timeframe = reservation.endTime - reservation.startTime;
        charsAndTimeFrame.text = "36 | " + timeframe.Hours + ":" + timeframe.Minutes;
    }
}
