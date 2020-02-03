using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReservationDisplay : MonoBehaviour
{
    Room room;
    Reservation reservation;
    [SerializeField] Text roomText;
    [SerializeField] Text roomSizeText;
    [SerializeField] Text dayDateText;
    [SerializeField] Text timeDisplay;
    [SerializeField] Button mapButton;
    [SerializeField] Button editButton;
    [SerializeField] Button deleteButton;
    [SerializeField] RectTransform background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Room pRoom, Reservation res, GameObject popUp, Button pDeleteButton)
    {
        this.room = pRoom;
        reservation = res;
        deleteButton.onClick.AddListener(
            delegate {
                popUp.SetActive(true);
            });
        pDeleteButton.onClick.AddListener(
            delegate
            {
                CompleteHandler.GetInstance().DeleteReservation(reservation);
                this.GetComponentInParent<DisplayMadeReservations>().Init();
            });
        editButton.onClick.AddListener(
            delegate
            {
                CompleteHandler.GetInstance().SetEditReservation(reservation, room);
                SceneManager.LoadScene("Nathalie's scene (sceme 4 the results page)");
            });
    }

    public void SetText()
    {
        roomText.text = room.roomNumber;
        roomSizeText.text = Room.RoomSizeToString(room.size);
        DateTime t = reservation.startTime;
        dayDateText.text = t.DayOfWeek.ToString() + " " + t.Day + " " + Helper.MonthToString(t.Month) + " " + t.Year;
        timeDisplay.text = Helper.HourMinToString(t.Hour, t.Minute) + " - " + Helper.HourMinToString(reservation.endTime.Hour, reservation.endTime.Minute);
    }

    public Vector2 GetSize()
    {
        return new Vector2(background.sizeDelta.x * this.transform.localScale.x, background.sizeDelta.y * this.transform.localScale.y); ;
    }
}
