using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class ReservationTimeSelect : MonoBehaviour
{
    const float sliderPrecisionDevider = 7.0f;
    const float sliderPrecisionOverlap = 30.0f;

    [SerializeField] Text dayDateText;
    [SerializeField] Text timeText;
    [SerializeField] Text roomText;
    [SerializeField] Text roomSizeText;
    [SerializeField] TimeBarHandler timeBar;
    [SerializeField] GameObject centralUI;
    Reservation selectedReservation;

    // Start is called before the first frame update
    void Start()
    {
        centralUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetSelectedReservation(Reservation selected, Reservation timeSlot)
    {
        selectedReservation = selected;
        timeBar.SetReservation(selected, timeSlot);
        centralUI.SetActive(true);
    }

    public void SetTextDateTime(Reservation reservation)
    {
        if (reservation == null) return;
        //dayDateText = Thursday 20 February 2020
        dayDateText.text = "" + reservation.startTime.DayOfWeek.ToString() + " " + reservation.startTime.Day + " " + Helper.MonthToString(reservation.startTime.Month) + " " + reservation.startTime.Year;
        //timeText = 11:45 - 14:00
        timeText.text = "" + Helper.HourMinToString(reservation.startTime.Hour, reservation.startTime.Minute) + " - " + Helper.HourMinToString(reservation.endTime.Hour, reservation.endTime.Minute);
    }

    public void SetRoomTimeUI(Room room, Reservation res)
    {
        roomText.text = "Room " + room.roomNumber;
        roomSizeText.text = Room.RoomSizeToString(room.size);
    }

    public Reservation GetSelectedTime()
    {
        return timeBar.GetSelectedTime();
    }
}
