using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this handler will always exist and information can be passed to it
public class CompleteHandler : MonoBehaviour
{
    static CompleteHandler instance;

    [SerializeField] List<Room> rooms;

    Reservation wantToReserve;
    RoomSize reservationSize;
    List<RoomItems> reservationRequirements;
    Building reservationBuilding;
    int floor;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetReservation(Reservation res, RoomSize size, Building building, int floor = -1, List<RoomItems> requiredItems = null)
    {
        wantToReserve = res;
        reservationSize = size;
        reservationRequirements = requiredItems;
        reservationBuilding = building;
        this.floor = floor;
        Debug.Log("Set reservation");
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
