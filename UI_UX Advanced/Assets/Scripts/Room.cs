using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum RoomSize
{
    ANY = 0,
    TEAMSIZE,
    LABSIZE,
    LECTURESIZE,
}

public enum RoomItems
{
    BEAMER,
    COMPUTERS,
    SMARTBOARD,
    SOLDERINGSTATION,
    WOODWORKINGITEMS,
}

public enum Building
{
    ANY,
    EPYDROST,
    FORUM,
    WOLVECAMP,
}

[CreateAssetMenu(fileName = "RoomObject", menuName = "ScriptableObjects/Room", order = 1)]
public class Room : ScriptableObject
{
    public void Init(bool setDateToday = true)
    {
        reservations = new Dictionary<DateTime, List<Reservation>>();
        if (availableItems != null) availableItems.Sort();
        else availableItems = new List<RoomItems>();

        if (inspectorReservations == null) return;

        if (inspectorReservations != null) inspectorReservations.Sort();
        else inspectorReservations = new List<Reservation>();
        for (int i = 0; i < inspectorReservations.Count; i++)
        {
            if (!inspectorReservations[i].Init())
            {
                inspectorReservations.RemoveAt(i);
                i--;
                Debug.Log("End before Start");
            }
            else
            {
                if (!setDateToday)
                {
                    DateTime date = inspectorReservations[i].startTime.Date;
                    if (!reservations.ContainsKey(date)) reservations.Add(date, new List<Reservation>());
                    reservations[date].Add(inspectorReservations[i]);
                    Debug.Log(inspectorReservations[i].ToString());
                }
                else
                {
                    DateTime date = DateTime.Today;
                    Reservation reservation = inspectorReservations[i].Copy();
                    reservation.startTime = new DateTime(date.Year, date.Month, date.Day, reservation.startTime.Hour, reservation.startTime.Minute, reservation.startTime.Second);
                    reservation.endTime = new DateTime(date.Year, date.Month, date.Day, reservation.endTime.Hour, reservation.endTime.Minute, reservation.endTime.Second);
                    if (!reservations.ContainsKey(date)) reservations.Add(date, new List<Reservation>());
                    reservations[date].Add(reservation);
                    Debug.Log(reservation.ToString());
                }
            }
        }
    }

    public String roomNumber;
    public RoomSize size = RoomSize.LABSIZE;
    public List<RoomItems> availableItems;
    public List<Reservation> inspectorReservations;
    public Dictionary<DateTime, List<Reservation>> reservations;
    public Building building;
    public int floor = 0;

    public Room Copy()
    {
        Room room = new Room();
        room.roomNumber = roomNumber;
        room.size = size;
        room.building = building;
        room.floor = floor;
        room.availableItems = new List<RoomItems>();
        for(int i = 0; i < availableItems.Count; ++i)
        {
            room.availableItems.Add(availableItems[i]);
        }
        if (inspectorReservations != null)
        {
            room.inspectorReservations = new List<Reservation>();
            for (int i = 0; i < inspectorReservations.Count; ++i)
            {
                room.inspectorReservations.Add(inspectorReservations[i]);
            }
        }
        if (reservations != null)
        {
            room.reservations = new Dictionary<DateTime, List<Reservation>>(reservations);
            foreach (DateTime dt in reservations.Keys)
            {
                //add DateTime, new List<>
                room.reservations.Add(dt, new List<Reservation>());
                for (int i = 0; i < reservations[dt].Count; ++i)
                {
                    //add all reservations in the list
                    room.reservations[dt].Add(reservations[dt][i]);
                }
            }
        }

        return room;
    }

    public List<Reservation> GetAvailableReservationsForDate(DateTime pDate)
    {
        List<Reservation> output = new List<Reservation>();
        if (reservations.ContainsKey(pDate.Date))
        {
            List<Reservation> res = new List<Reservation>(reservations[pDate.Date]);
            TimeSpan minResTime = new TimeSpan(0, 14, 59);
            for (int i = 0; i < res.Count - 1; i++)
            {
                TimeSpan ts = res[i + 1].startTime - res[i].endTime;
                if(ts >= minResTime)
                {
                    DateTime start = new DateTime(pDate.Year, pDate.Month, pDate.Day, res[i].endTime.Hour, res[i].endTime.Minute, res[i].endTime.Second);
                    DateTime end = new DateTime(pDate.Year, pDate.Month, pDate.Day, res[i+1].startTime.Hour, res[i + 1].startTime.Minute, res[i + 1].startTime.Second);
                    Reservation toAdd = new Reservation(start, end);
                    output.Add(toAdd);
                }
            }
        }
        else return null;
        return output;
    }

    public static string RoomSizeToString(RoomSize roomSize)
    {
        switch(roomSize)
        {
            case RoomSize.ANY:
                return "Labroom";
            case RoomSize.LABSIZE:
                return "Labroom";
            case RoomSize.LECTURESIZE:
                return "Lecture room";
            case RoomSize.TEAMSIZE:
                return "Team room";
            default:
                return "Labroom";
        }
    }

    public override String ToString()
    {
        return "Room " + roomNumber + " with size " + size;
    }
}
