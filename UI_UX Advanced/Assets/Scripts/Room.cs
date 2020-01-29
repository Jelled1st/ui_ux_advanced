using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum RoomSize
{
    TEAMSIZE = 0,
    LABSIZE = 1,
    LECTURESIZE = 2,
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
    public void Init()
    {
        reservations = new Dictionary<DateTime, List<Reservation>>();
        availableItems.Sort();

        if (inspectorReservations == null) return;

        inspectorReservations.Sort();
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
                Debug.Log(inspectorReservations[i].ToString());
                DateTime date = inspectorReservations[i].startTime.Date;
                if(!reservations.ContainsKey(date)) reservations.Add(date, new List<Reservation>());
                reservations[date].Add(inspectorReservations[i]);
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
        room.availableItems = new List<RoomItems>(availableItems);
        if(inspectorReservations != null) room.inspectorReservations = new List<Reservation>(inspectorReservations);
        if(reservations != null) room.reservations = new Dictionary<DateTime, List<Reservation>>(reservations);

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

    public override String ToString()
    {
        return "Room " + roomNumber + " with size " + size;
    }
}
