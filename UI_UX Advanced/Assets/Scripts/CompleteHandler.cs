using System;
using System.Collections.Generic;
using UnityEngine;

//this handler will always exist and information can be passed to it
public class CompleteHandler : MonoBehaviour
{
    static CompleteHandler instance;

    [SerializeField] List<Room> rooms;

    Reservation reservationInput;
    RoomSize reservationRoomSize;
    List<RoomItems> reservationItems;
    Building reservationBuilding;
    int reservationFloor;

    [SerializeField] List<Room> suitableRooms; 

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < rooms.Count; ++i)
        {
            rooms[i].Init();
        }
        suitableRooms = new List<Room>();
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) FindSuitableReservations();
        if (Input.GetKeyDown(KeyCode.F))
        {
            DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 30, 0);
            DateTime end = start.AddHours(1);
            SetReservation(new Reservation("VRZ", start, end));
        }
    }

    public void SetReservation(Reservation res, RoomSize size = RoomSize.LABSIZE, Building building = Building.ANY, int floor = -1, List<RoomItems> requiredItems = null)
    {
        reservationInput = res;
        reservationRoomSize = size;
        if (requiredItems == null) requiredItems = new List<RoomItems>();
        reservationItems = requiredItems;
        reservationBuilding = building;
        this.reservationFloor = floor;
        Debug.Log("Set reservation: " + reservationInput.ToString());
    }

    public void FindSuitableReservations()
    {
        for (int i = 0; i < rooms.Count; ++i)
        {
            if (reservationFloor != -1 && rooms[i].floor != reservationFloor) continue;
            if (reservationBuilding != Building.ANY && rooms[i].building != reservationBuilding) continue; // room is not in the right building/floor
            if (reservationRoomSize > rooms[i].size) continue; //room is not big enough

            if (reservationItems == null || reservationItems.Count <= 0)
            {
                bool containsEveryItem = true;
                //this loop can be optimized
                for (int j = 0; j < reservationItems.Count; ++j)
                {
                    bool containsItem = false;
                    for (int k = 0; k < rooms[i].availableItems.Count; ++k)
                    {
                        if (reservationItems[j] == rooms[i].availableItems[k]) containsItem = true;
                    }
                    if (!containsItem)
                    {
                        containsEveryItem = false;
                        break;
                    }
                }
                if (!containsEveryItem) continue; // does not contain the right items
            }

            Room room = new Room();
            room.Init();
            for (int j = 0; j < rooms[i].availableItems.Count; ++j)
            {
                room.availableItems.Add(rooms[i].availableItems[j]);
            }
            room.building = rooms[i].building;
            room.floor = rooms[i].floor;
            room.roomNumber = rooms[i].roomNumber;
            room.size = rooms[i].size;
            room.inspectorReservations = new List<Reservation>();
            room.reservations = new Dictionary<DateTime, List<Reservation>>();
            suitableRooms.Add(room);

            if (rooms[i].reservations == null) Debug.Log("debug statement");
            List<Reservation> availableTimes = rooms[i].GetAvailableReservationsForDate(reservationInput.startTime.Date);

            if (availableTimes == null || availableTimes.Count <= 0)
            {
                suitableRooms.Remove(room);
                Debug.Log("skipping adding available times");
                continue;
            }
            for (int j = 0; j < availableTimes.Count; ++j)
            {
                if (availableTimes[j].startTime <= reservationInput.startTime)
                {
                    Debug.Log("start time should be correct");
                    if (availableTimes[j].endTime >= reservationInput.endTime)
                    {
                        Debug.Log("so is endtime");
                        DateTime start = availableTimes[j].startTime;
                        if (!room.reservations.ContainsKey(availableTimes[j].startTime.Date))
                        { 
                            room.reservations.Add(availableTimes[j].startTime.Date, new List<Reservation>());
                        }
                        Debug.Log("Added available time as: " + availableTimes[j].ToString());
                        room.inspectorReservations.Add(availableTimes[j]);
                        room.reservations[start.Date].Add(availableTimes[j]);
                    }
                }
            }
            if(room.reservations.Count <= 0)
            {
                suitableRooms.Remove(room);
            }
        }
    }

    public static CompleteHandler GetInstance()
    {
        if(instance == null)
        {
            instance = new CompleteHandler();
        }
        return instance;
    }
}
