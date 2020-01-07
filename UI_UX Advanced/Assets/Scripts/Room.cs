using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum RoomFunction
{
    STANDARD = 0,
    OPENWORKSPACE,
    TECHNICAL,
}

public enum RoomItems
{
    BEAMER,
    SOLDERINGSTATION,
    WOODWORKINGITEMS,
}

[CreateAssetMenu(fileName = "RoomObject", menuName = "ScriptableObjects/Room", order = 1)]
public class Room : ScriptableObject
{
    public void Init()
    {
        availableItems.Sort();
        
        reservations.Sort();
        for (int i = 0; i < reservations.Count; i++)
        {
            if (!reservations[i].Init())
            {
                reservations.RemoveAt(i);
                i--;
                Debug.Log("End before Start");
            }
            else Debug.Log(reservations[i].ToString());
        }
    }

    public String RoomNumber;
    public int size;
    public RoomFunction function;
    public List<RoomItems> availableItems;
    public List<Reservation> reservations;

    public Room Copy()
    {
        Room room = new Room();
        room.size = size;
        room.function = function;
        room.availableItems = new List<RoomItems>(availableItems);
        room.reservations = new List<Reservation>(reservations);

        return room;
    }
}
