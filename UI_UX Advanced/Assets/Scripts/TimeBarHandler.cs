using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimeBarHandler : MonoBehaviour
{
    const float sliderPrecisionDevider = 7.0f;
    const float timeBarDisplacement = -15;

    [FormerlySerializedAs("sliderLowest")] [SerializeField] GameObject sliderLow;
    [FormerlySerializedAs("sliderHighest")] [SerializeField] GameObject sliderHigh;
    [SerializeField] RectTransform selectedTime;
    [SerializeField] RectTransform timeBar;
    [FormerlySerializedAs("RedArea1")] [SerializeField] RectTransform redAreaStart;
    [FormerlySerializedAs("RedArea2")] [SerializeField] RectTransform redAreaEnd;
    [SerializeField] Text[] timeBarTimes;
    [SerializeField] Text selectedTimeStart;
    [SerializeField] Text selectedTimeEnd;
    RectTransform sliderLowRect;
    RectTransform sliderHighRect;
    GameObject selectedSlider = null;
    RectTransform selectedSliderRect = null;
    Vector2 mousePos;
    Reservation requestedReservation;
    Reservation selectedTimeSlot;
    bool activated = false;
    float timeBarLength;
    float sliderLength;
    float lowestTimePosition = 0;
    float highestTimePosition = 0;
    float oneHourBarLength;
    float startPositionBar;
    int startHour = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (sliderLow != null)
        {
            sliderLowRect = sliderLow.GetComponent<RectTransform>();
            sliderLength = sliderLowRect.sizeDelta.x;
        }
        if (sliderHigh != null)
        {
            sliderHighRect = sliderHigh.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && Input.GetMouseButton(0))
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

            if (selectedSlider != null)
            {
                Vector2 newMouse = Input.mousePosition;
                Vector2 mouseMove = newMouse - mousePos;
                mousePos = newMouse;
                selectedSliderRect.localPosition = selectedSliderRect.localPosition + new Vector3(mouseMove.x * 1.25f, 0, 0);
            }

            //make sure end does not end up in front of front and vice versa
            if (sliderLow == selectedSlider && sliderLowRect.localPosition.x > sliderHighRect.localPosition.x - sliderLength/2 + timeBarDisplacement)
            {
                sliderLowRect.localPosition = new Vector3(sliderHighRect.localPosition.x - sliderLength/2 + timeBarDisplacement, sliderLowRect.localPosition.y, sliderLowRect.localPosition.z);
            }
            if (sliderHigh == selectedSlider && sliderHighRect.localPosition.x < sliderLowRect.localPosition.x + sliderLength/2 + timeBarDisplacement)
            {
                sliderHighRect.localPosition = new Vector3(sliderLowRect.localPosition.x + sliderLength/2 + timeBarDisplacement, sliderHighRect.localPosition.y, sliderHighRect.localPosition.z);;
            }

            if (sliderLow == selectedSlider && sliderLowRect.localPosition.x < lowestTimePosition - sliderLength/2 + timeBarDisplacement)
            {
                sliderLowRect.localPosition = new Vector3(lowestTimePosition - sliderLength/2 + timeBarDisplacement, sliderLowRect.localPosition.y, sliderLowRect.localPosition.z);
            }
            if (sliderHigh == selectedSlider && sliderHighRect.localPosition.x > highestTimePosition + sliderLength/2 + timeBarDisplacement)
            {
                sliderHighRect.localPosition = new Vector3(highestTimePosition + sliderLength/2 + timeBarDisplacement, sliderHighRect.localPosition.y, sliderHighRect.localPosition.z);
            }
            
            float width = sliderHighRect.localPosition.x - sliderLowRect.localPosition.x - (sliderLength);
            selectedTime.sizeDelta = new Vector2(width, selectedTime.sizeDelta.y);
            selectedTime.localPosition = new Vector3(sliderLowRect.localPosition.x + width / 2 + sliderLength / 2, selectedTime.localPosition.y, selectedTime.localPosition.z);
            
            SetSliderSelectedTimeText();
        }
        else selectedSlider = null;
    }

    public void SetReservation(Reservation selected, Reservation available)
    {
        activated = true;
        requestedReservation = selected;
        InitializeBar(available);
    }

    private void InitializeBar(Reservation available)
    {
        timeBarLength = timeBar.GetComponent<RectTransform>().sizeDelta.x;

        int hour = available.startTime.Hour;
        if(available.startTime.Minute == 0)
        {
            hour--;
        }
        //set timebar time texts
        startHour = hour;
        timeBarTimes[0].text = "" + hour + ":00";
        for(int i = 1; i < timeBarTimes.Length; ++i)
        {
            timeBarTimes[i].text = "" + (hour + i) + ":00";
        }

        oneHourBarLength = (timeBarLength / (timeBarTimes.Length - 1));
        startPositionBar = timeBar.localPosition.x - timeBarLength / 2.0f + sliderLength / 2;

        //set begin position of sliders
        //the begin position will be on the filled in time of the reservation
        float timeDistance = 0;
        for (int i = 0; i < timeBarTimes.Length; ++i)
        {
            if (requestedReservation.startTime.Hour + i == requestedReservation.startTime.Hour)
            {
                int add = 0;
                if (available.startTime.Minute == 0) add++;
                timeDistance = i + add + (requestedReservation.startTime.Minute / 60.0f);
                break;
            }
        }
        sliderLowRect.localPosition = new Vector3(startPositionBar + oneHourBarLength * timeDistance, sliderLowRect.localPosition.y, sliderLowRect.localPosition.z);

        for (int i = 0; i < timeBarTimes.Length; ++i)
        {
            if(requestedReservation.startTime.Hour + i == requestedReservation.endTime.Hour)
            {
                int add = 0;
                if (available.startTime.Minute == 0) add++;
                timeDistance = i + add + (requestedReservation.endTime.Minute/60.0f);
                break;
            }
        }
        sliderHighRect.localPosition = new Vector3(startPositionBar + oneHourBarLength * timeDistance, sliderHighRect.localPosition.y, sliderHighRect.localPosition.z);

        lowestTimePosition = startPositionBar;
        if (available.startTime.Minute == 0)
        {
            lowestTimePosition += oneHourBarLength;
            redAreaStart.sizeDelta = new Vector2(oneHourBarLength, redAreaStart.sizeDelta.y);
        }
        else
        {
            lowestTimePosition += available.startTime.Minute / 60 * oneHourBarLength;
            redAreaStart.sizeDelta = new Vector2(available.startTime.Minute / 60 * oneHourBarLength, redAreaStart.sizeDelta.y);
        }

        highestTimePosition = startPositionBar;
        int diff = available.endTime.Hour - available.startTime.Hour;
        int toAdd = 0;
        if (available.startTime.Minute == 0) toAdd++;
        highestTimePosition += (diff + toAdd + (available.endTime.Minute / 60.0f)) * oneHourBarLength;
        redAreaEnd.sizeDelta = new Vector2(timeBarLength - (diff + toAdd + (available.endTime.Minute / 60.0f)) * oneHourBarLength, redAreaEnd.sizeDelta.y);
        
        redAreaStart.localPosition = new Vector3(startPositionBar + redAreaStart.sizeDelta.x/2+timeBarDisplacement, redAreaStart.localPosition.y, redAreaStart.localPosition.z);
        redAreaEnd.localPosition = new Vector3(startPositionBar + timeBarLength - redAreaEnd.sizeDelta.x/2+timeBarDisplacement, redAreaStart.localPosition.y, redAreaStart.localPosition.z);

        SetSliderSelectedTimeText();
    }

    private void SetSliderSelectedTimeText()
    {
        DateTime start = GetTimeFromX(sliderLowRect.localPosition.x + sliderLength);
        selectedTimeStart.text = Helper.HourMinToString(start.Hour, start.Minute);

        DateTime end = GetTimeFromX(sliderHighRect.localPosition.x);
        selectedTimeEnd.text = Helper.HourMinToString(end.Hour, end.Minute);

        selectedTimeSlot = new Reservation(start, end);
    }

    private DateTime GetTimeFromX(float x)
    {
        x -= startPositionBar + timeBarDisplacement + 10;

        float time = x / oneHourBarLength;
        int hour = (int)time;
        int min = (int)((time - hour) * 60);
        while(min % 5 != 0)
        {
            min--;
        }
        if (min < 0) min = 0;
        
        return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Month, hour+startHour, min, 0);
    }

    public Reservation GetSelectedTime()
    {
        return selectedTimeSlot;
    }
}
