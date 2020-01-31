using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectReservationInput : MonoBehaviour
{
    const float sliderPrecisionDevider = 7.0f;
    const float sliderPrecisionOverlap = 3.0f;

    [SerializeField] Text dayDateText;
    [SerializeField] Text timeText;
    [SerializeField] Text roomText;
    [SerializeField] Text roomSizeText;
    [SerializeField] GameObject sliderLowest;
    [SerializeField] GameObject sliderHighest;
    [SerializeField] RectTransform selectedTime;
    Reservation requestedReservation;
    List<Room> availableRooms;
    GameObject selectedSlider = null;
    Vector2 mousePos;

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
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (selectedSlider == null && Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject == sliderLowest)
                {
                    mousePos = Input.mousePosition;
                    selectedSlider = sliderLowest;
                }
                else if (hit.transform.gameObject == sliderHighest)
                {
                    mousePos = Input.mousePosition;
                    selectedSlider = sliderHighest;
                }
            }

            if(selectedSlider != null)
            {
                Vector2 newMouse = Input.mousePosition;
                Vector2 mouseMove = newMouse - mousePos;
                Debug.Log(mouseMove.ToString());
                mousePos = newMouse;
                selectedSlider.transform.position = selectedSlider.transform.position + new Vector3(mouseMove.x/ sliderPrecisionDevider, 0, 0);
            }

            //make sure end does not end up in front of front and vice versa
            if (sliderLowest.transform.position.x > sliderHighest.transform.position.x - sliderPrecisionOverlap)
            {
                sliderLowest.transform.position = new Vector3(sliderHighest.transform.position.x - sliderPrecisionOverlap, sliderLowest.transform.position.y, sliderLowest.transform.position.z);
            }
            if (sliderHighest.transform.position.x < sliderLowest.transform.position.x+ sliderPrecisionOverlap)
            {
                sliderHighest.transform.position = new Vector3(sliderLowest.transform.position.x+ sliderPrecisionOverlap, sliderHighest.transform.position.y, sliderHighest.transform.position.z);
            }

            selectedTime.sizeDelta = new Vector2((sliderHighest.transform.position.x - sliderLowest.transform.position.x)*10, selectedTime.sizeDelta.y);
            
        }
        else selectedSlider = null;
    }

    public void SetTextDateTime(Reservation reservation)
    {
        if (reservation == null) return;
        //dayDateText = Thursday 20 February 2020
        dayDateText.text = "" + reservation.startTime.DayOfWeek.ToString() + " " + reservation.startTime.Day + " " + Helper.MonthToString(reservation.startTime.Month) + " " + reservation.startTime.Year;
        //timeText = 11:45 - 14:00
        timeText.text = "" + reservation.startTime.Hour + ":" + reservation.startTime.Minute + " - " + reservation.endTime.Hour + ":" + reservation.endTime.Minute;

    }
}
