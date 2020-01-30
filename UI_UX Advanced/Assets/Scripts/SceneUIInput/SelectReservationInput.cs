using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectReservationInput : MonoBehaviour
{
    [SerializeField] Text dayDateText;
    [SerializeField] Text timeText;
    [SerializeField] Text roomText;
    [SerializeField] Text roomSize;
    [SerializeField] 
    Reservation requestedReservation;
    List<Room> availableRooms;

    // Start is called before the first frame update
    void Start()
    {
        requestedReservation = CompleteHandler.GetInstance().GetRequestedReservation();
        SetTextDateTime(requestedReservation);
        availableRooms = CompleteHandler.GetInstance().GetAvailableRooms();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            requestedReservation = CompleteHandler.GetInstance().GetRequestedReservation();
            SetTextDateTime(requestedReservation);
        }
    }

    public void SetTextDateTime(Reservation reservation)
    {
        //dayDateText = Thursday 20 February 2020
        dayDateText.text = "" + reservation.startTime.DayOfWeek.ToString() + " " + reservation.startTime.Day + " " + Helper.MonthToString(reservation.startTime.Month) + " " + reservation.startTime.Year;
        //timeText = 11:45 - 14:00
        timeText.text = "" + reservation.startTime.Hour + ":" + reservation.startTime.Minute + " - " + reservation.endTime.Hour + ":" + reservation.endTime.Minute;

    }
}
