using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class UIHandler : MonoBehaviour
{
    RoomHUD _selectedRoom;
    [SerializeField] GameObject _roomUI;
    [SerializeField] GameObject _standardUI;
    [SerializeField] GameObject _createReservationUI;
    [SerializeField] TextMeshProUGUI _roomNumber;
    [SerializeField] TextMeshProUGUI _reservationsField;
    [SerializeField] CustomDropDownMenu _dropdown;

    // Start is called before the first frame update
    void Start()
    {
        _roomUI.SetActive(false);
        _standardUI.SetActive(true);
        _createReservationUI.SetActive(false);
        DebugMode();
    }

    void DebugMode()
    {
    }

    void Update()
    {

    }

    // Update is called once per frame
    void UpdateUI()
    {
        if(_selectedRoom != null)
        {
            _roomUI.SetActive(true);
            _standardUI.SetActive(false);

            _roomNumber.text = "Room: " + _selectedRoom.room.roomNumber;
            List<Reservation> reservations = _selectedRoom.GetReservationsForDate(DateTime.Today.Date);
            if (reservations == null)
            {
                Debug.Log("no reservations for today");
                return;
            }
            string res = "";
            for(int i = 0; i < reservations.Count; i++)
            {
                res += "Start:  " + Reservation.TimeToString(reservations[i].startTime) + "\n";
                res += "End:    " + Reservation.TimeToString(reservations[i].endTime) + "\n";
                res += "-------------------------------------------\n";
            }
            _reservationsField.text = res;
        }
        else
        {
            _roomUI.SetActive(false);
            _standardUI.SetActive(true);
        }
    }

    public void SelectRoom(RoomHUD pSelected)
    {
        _selectedRoom = pSelected;
        Debug.Log(_selectedRoom.room.ToString());
        UpdateUI();
    }

    public void CreateNewReservationUI()
    {
        _dropdown.options.Clear();
        _dropdown.eventsToCallOnOption.Clear();
        _createReservationUI.SetActive(true);
        List<Reservation> reservations = _selectedRoom.room.GetAvailableReservationsForDate(DateTime.Today.Date);
        if (reservations == null || reservations.Count == 0)
        {
            Debug.Log("N/A");
            return;
        }
        for(int i = 0; i < reservations.Count; i++)
        {
            Debug.Log(i);
            _dropdown.options.Add(Reservation.TimeToString(reservations[i].startTime));
            _dropdown.eventsToCallOnOption.Add(new UnityEvent());
            _dropdown.eventsToCallOnOption[i].AddListener(delegate { Debug.Log("Something to say"); });
        }
    }
}
