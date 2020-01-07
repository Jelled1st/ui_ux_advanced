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
            for(int i = 0; i < room.reservations.Count; i++)
            {
                room.reservations[i].Init();
            }
            room = room.Copy();
            room.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public List<Reservation> GetReservations()
    {
        return room.reservations;
    }

    public Reservation GetReservation(DateTime pDateTime)
    {
        for(int i = 0; i < room.reservations.Count; i++)
        {
            if (room.reservations[i].startTime == pDateTime) return room.reservations[i];
        }
        return null;
    }

    public Reservation GetReservation(int index)
    {
        return room.reservations[index];
    }
}
