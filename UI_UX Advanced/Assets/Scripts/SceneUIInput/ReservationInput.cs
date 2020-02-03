using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReservationInput : MonoBehaviour
{
    [SerializeField] Text dayTxt;
    [SerializeField] Text monthTxt;
    [SerializeField] Text yearTxt;
    [SerializeField] Text hour0Txt;
    [SerializeField] Text min0Txt;
    [SerializeField] Text hour1Txt;
    [SerializeField] Text min1Txt;

    [SerializeField] GameObject beamer;
    [SerializeField] GameObject Computers;
    [SerializeField] GameObject Smartboard;

    [SerializeField] Dropdown roomSizeDropdown;
    [SerializeField] Dropdown buildingDropdown;
    [SerializeField] Dropdown floorDropdown;

    [SerializeField] GameObject invalidDatePopUp;

    RoomSize roomSize = RoomSize.TEAMSIZE;
    Building building = Building.ANY;
    int floor = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            PassReservationToHandler();
        }
    }

    public void OnSearch()
    {
        DateTime start;
        DateTime end;
        if(GetDateTimeFromText(out start) && GetEndDateTimeFromText(out end))
        {
            Reservation res = new Reservation("You", start, end);
            List<RoomItems> requiredItems = new List<RoomItems>();
            if (beamer.activeSelf) requiredItems.Add(RoomItems.BEAMER);
            if (Computers.activeSelf) requiredItems.Add(RoomItems.COMPUTERS);
            if (Smartboard.activeSelf) requiredItems.Add(RoomItems.SMARTBOARD);
            CompleteHandler.GetInstance().SetReservation(res, roomSize, building, floor-1, requiredItems);
            SceneManager.LoadScene("Nathalie's scene (sceme 4 the results page)");
        }
    }

    public void PassReservationToHandler()
    {
        DateTime start;
        DateTime end;
        if (GetDateTimeFromText(out start) && GetEndDateTimeFromText(out end))
        {
            Reservation res = new Reservation("You", start, end);
            List<RoomItems> requiredItems = new List<RoomItems>();
            if (beamer.activeSelf) requiredItems.Add(RoomItems.BEAMER);
            if (Computers.activeSelf) requiredItems.Add(RoomItems.COMPUTERS);
            if (Smartboard.activeSelf) requiredItems.Add(RoomItems.SMARTBOARD);
            CompleteHandler.GetInstance().SetReservation(res, roomSize, building, floor - 1, requiredItems);
        }
    }

    public void SetRoomSizeDropdownValue()
    {
        roomSize = (RoomSize)roomSizeDropdown.value;
    }

    public void SetBuildingDropdownValue()
    {
        building = (Building)buildingDropdown.value;
    }

    public void SetFloorDropdownValue()
    {
        floor = floorDropdown.value;
    }

    bool GetDateTimeFromText(out DateTime dateTime)
    {
        int year;
        int month;
        int day;
        int hour;
        int min;

        if (Int32.TryParse(yearTxt.text, out year))
        {
            if (Int32.TryParse(monthTxt.text, out month))
            {
                if (Int32.TryParse(dayTxt.text, out day))
                {
                    if (Int32.TryParse(hour0Txt.text, out hour))
                    {
                        if (Int32.TryParse(min0Txt.text, out min))
                        {
                            if (Helper.isValidDateTime(year, month, day, hour, min, 0))
                            {
                                dateTime = new DateTime(year, month, day, hour, min, 0);
                                return true;
                            }
                            else invalidDatePopUp.SetActive(true);
                        }
                    }
                }
            }
        }
        dateTime = new DateTime(2020, 1, 1, 0, 0, 0);
        return false;
    }

    bool GetEndDateTimeFromText(out DateTime dateTime)
    {

        int year;
        int month;
        int day;
        int hour;
        int min;

        if (Int32.TryParse(yearTxt.text, out year))
        {
            if (Int32.TryParse(monthTxt.text, out month))
            {
                if (Int32.TryParse(dayTxt.text, out day))
                {
                    if (Int32.TryParse(hour1Txt.text, out hour))
                    {
                        if (Int32.TryParse(min1Txt.text, out min))
                        {
                            if (Helper.isValidDateTime(year, month, day, hour, min, 0))
                            {
                                dateTime = new DateTime(year, month, day, hour, min, 0);
                                return true;
                            }
                            else invalidDatePopUp.SetActive(true);
                        }
                    }
                }
            }
        }
        dateTime = new DateTime(2020, 1, 1, 0, 0, 0);
        return false;
    }
}
