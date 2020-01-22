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
    [SerializeField] TextMeshProUGUI _roomNumber;
    [SerializeField] GameObject[] _dayPanels;

    // Start is called before the first frame update
    void Start()
    {
        _roomUI.SetActive(false);
        _standardUI.SetActive(true);
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
            _roomNumber.text = _selectedRoom.room.roomNumber;

            for(int i = 0; i < _dayPanels.Length; ++i)
            {
                GameObject empty = new GameObject("Day & Date");
                TextMeshProUGUI dayDate = empty.AddComponent<TextMeshProUGUI>();
                dayDate.text = DateTimeExtension.DateToString(DateTimeExtension.GetDateOfLastDay((DayOfWeek)(i + 1)));
                empty.transform.parent = _dayPanels[i].transform;
            }
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
        UpdateUI();
    }
}
