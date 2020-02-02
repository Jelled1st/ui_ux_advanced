using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectAvailableTimeSlot : MonoBehaviour
{
    [FormerlySerializedAs("AvailableTimePrefab")] [SerializeField] GameObject timeslotPrefab;
    List<AvailableTimeView> availableTimeViews;
    [SerializeField] ReservationTimeSelect timeInput;
    AvailableTimeView selectedTime;

    // Start is called before the first frame update
    void Start()
    {
        availableTimeViews = new List<AvailableTimeView>();
        LoadTimeSlots();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            LoadTimeSlots();
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                AvailableTimeView timeSlot;
                if(hit.transform.gameObject.TryGetComponent<AvailableTimeView>(out timeSlot))
                {
                    SetSelectedTime(timeSlot);
                }
            }
        }
    }

    public void LoadTimeSlots()
    {
        timeInput.SetTextDateTime(CompleteHandler.GetInstance().GetRequestedReservation());
        List<Room> rooms = CompleteHandler.GetInstance().GetAvailableRooms();
        DateTime date = CompleteHandler.GetInstance().GetRequestedDate();
        for(int i = 0; i < rooms.Count; ++i)
        {
            List<Reservation> reservations = rooms[i].reservations[date];
            for(int j = 0; j < reservations.Count; ++j)
            {
                AddTimeSlot(rooms[i], reservations[j]);
            }
        }
    }

    public void AddTimeSlot(Room room, Reservation res)
    {
        GameObject slot = GameObject.Instantiate(timeslotPrefab, this.transform);
        AvailableTimeView timeView = slot.GetComponent<AvailableTimeView>();
        timeView.SetRoomReservation(room, res);
        timeView.SetAllText();
        float height = timeslotPrefab.GetComponent<RectTransform>().rect.height;
        timeView.GetComponent<RectTransform>().localPosition += new Vector3(0, -height*1.5f* availableTimeViews.Count, 0); 
        availableTimeViews.Add(timeView);
    }

    public void SetSelectedTime(AvailableTimeView select)
    {
        selectedTime = select;
        timeInput.SetSelectedReservation(CompleteHandler.GetInstance().GetRequestedReservation(), select.reservation);
        timeInput.SetRoomTimeUI(select.room, select.reservation);
    }

    public void OnReserve()
    {
        Reservation timeSlot = timeInput.GetSelectedTime();
        Room room = selectedTime.room;
        //Room room = CompleteHandler.GetInstance().GetRoomWithNumber(selectedTime.room.roomNumber);
        //if(!room.reservations.ContainsKey(timeSlot.startTime.Date))
        //{
        //    room.reservations.Add(timeSlot.startTime.Date, new List<Reservation>());
        //}
        //room.reservations[timeSlot.startTime.Date].Add(timeSlot);
        Debug.Log("made reservation in room: " + room.roomNumber + " with reservation:\n" + timeSlot.ToString());
        CompleteHandler.GetInstance().CreateReservation(timeSlot, room);
        for(int i = 0; i < availableTimeViews.Count; ++i)
        {
            Destroy(availableTimeViews[i]);
        }
        availableTimeViews.Clear();
    }
}
