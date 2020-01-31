using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class SelectReservationInput : MonoBehaviour
{
    const float sliderPrecisionDevider = 7.0f;
    const float sliderPrecisionOverlap = 30.0f;

    [SerializeField] Text dayDateText;
    [SerializeField] Text timeText;
    [SerializeField] Text roomText;
    [SerializeField] Text roomSizeText;
    [FormerlySerializedAs("sliderLowest")] [SerializeField] GameObject sliderLow;
    [FormerlySerializedAs("sliderHighest")] [SerializeField] GameObject sliderHigh;
    [SerializeField] RectTransform selectedTime;
    RectTransform sliderLowRect;
    RectTransform sliderHighRect;
    Reservation requestedReservation;
    List<Room> availableRooms;
    GameObject selectedSlider = null;
    RectTransform selectedSliderRect = null;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        requestedReservation = CompleteHandler.GetInstance().GetRequestedReservation();
        SetTextDateTime(requestedReservation);
        availableRooms = CompleteHandler.GetInstance().GetAvailableRooms();

        if (sliderLow != null) sliderLowRect = sliderLow.GetComponent<RectTransform>();
        if (sliderHigh != null) sliderHighRect = sliderHigh.GetComponent<RectTransform>();
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
                if (hit.transform.gameObject == sliderLow)
                {
                    mousePos = Input.mousePosition;
                    selectedSlider = sliderLow;
                    selectedSliderRect = sliderLowRect;
                }
                else if (hit.transform.gameObject == sliderHigh)
                {
                    mousePos = Input.mousePosition;
                    selectedSlider = sliderHigh;
                    selectedSliderRect = sliderHighRect;
                }
            }

            if(selectedSlider != null)
            {
                Vector2 newMouse = Input.mousePosition;
                Vector2 mouseMove = newMouse - mousePos;
                Debug.Log(mouseMove.ToString());
                mousePos = newMouse;
                selectedSliderRect.localPosition = selectedSliderRect.localPosition + new Vector3(mouseMove.x*1.25f, 0, 0);
            }

            //make sure end does not end up in front of front and vice versa
            if (sliderLow == selectedSlider && sliderLowRect.localPosition.x > sliderHighRect.localPosition.x - sliderPrecisionOverlap)
            {
                sliderLowRect.localPosition = new Vector3(sliderHighRect.localPosition.x - sliderPrecisionOverlap, sliderLowRect.localPosition.y, sliderLowRect.localPosition.z);
            }
            if (sliderHigh == selectedSlider && sliderHighRect.localPosition.x < sliderLowRect.localPosition.x + sliderPrecisionOverlap)
            {
                sliderHighRect.localPosition = new Vector3(sliderLowRect.localPosition.x + sliderPrecisionOverlap, sliderHighRect.localPosition.y, sliderHighRect.localPosition.z);
            }
            float width = sliderHighRect.localPosition.x - sliderLowRect.localPosition.x - (sliderPrecisionOverlap);
            Debug.Log("new width is " + width);
            selectedTime.sizeDelta = new Vector2(width, selectedTime.sizeDelta.y);
            selectedTime.localPosition = new Vector3(sliderLowRect.localPosition.x + width/2+sliderPrecisionOverlap/2, selectedTime.localPosition.y, selectedTime.localPosition.z );
            
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
