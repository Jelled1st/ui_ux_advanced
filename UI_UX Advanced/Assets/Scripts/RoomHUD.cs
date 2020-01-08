using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomHUD : MonoBehaviour
{
    public Room room;

    // Start is called before the first frame update
    void Start()
    {
        if (room == null)
        {
            Debug.LogError("No Room Inizialized");
        }
        else
        {
            room = room.Copy();
            room.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public List<Reservation> GetReservationsForDate(DateTime pDate)
    {
        if (room.reservations.ContainsKey(pDate)) return room.reservations[pDate];
        else return null;
    }

    public Reservation GetReservation(DateTime pDateTime)
    {
        DateTime date = pDateTime.Date;
        if(room.reservations.ContainsKey(date))
        {
            for(int i = 0; i < room.reservations[date].Count; i++)
            {
                if (room.reservations[date][i].startTime == pDateTime) return room.reservations[date][i];
            }
        }
        return null;
    }
}
