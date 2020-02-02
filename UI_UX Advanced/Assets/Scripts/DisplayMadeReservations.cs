using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DisplayMadeReservations : MonoBehaviour
{
    List<ReservationDisplay> reservationDisplays;
    [FormerlySerializedAs("reservationDisplay")][SerializeField] GameObject reservationDisplayPrefab;

    // Start is called before the first frame update
    void Start()
    {
        reservationDisplays = new List<ReservationDisplay>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Room fake = new Room();
            fake.building = Building.EPYDROST;
            fake.floor = 2;
            fake.inspectorReservations = new List<Reservation>();
            fake.reservations = new Dictionary<DateTime, List<Reservation>>();
            fake.size = RoomSize.LABSIZE;
            fake.roomNumber = "G2.30";
            DateTime t = DateTime.Today;

            DateTime st = new DateTime(t.Year, t.Month, t.Day, 10, 30, 0);
            DateTime end = st.AddHours(1);
            Reservation res = new Reservation(st, end);
            CompleteHandler.GetInstance().CreateReservation(res, fake);
            //for(int i = 0; i < 2; ++i)
            //{
            //    Reservation some = new Reservation(st, end);
            //    CompleteHandler.GetInstance().CreateReservation(some, fake);
            //}

            Init();
        }
    }

    public void Init()
    {
        for(int i = 0; i < reservationDisplays.Count; ++i)
        {
            Destroy(reservationDisplays[i].gameObject);
        }
        reservationDisplays.Clear();
        List<Reservation> res = CompleteHandler.GetInstance().GetMadeReservationsAsList();
        if (res == null) return;
        Debug.Log("making display for " + res.Count);
        for(int i = 0; i < res.Count; ++i)
        {
            addDisplay(CompleteHandler.GetInstance().GetMadeReservations()[res[i]], res[i]);
        }
    }

    private void addDisplay(Room room, Reservation res)
    {
        GameObject displayGo = GameObject.Instantiate(reservationDisplayPrefab, this.gameObject.transform);
        ReservationDisplay display = displayGo.GetComponent<ReservationDisplay>();
        reservationDisplays.Add(display);
        display.Set(room, res);
        display.SetText();

        float width = display.GetSize().x;
        float height = display.GetSize().y;

        if(reservationDisplays.Count % 2 == 0)
        {
            display.transform.localPosition += new Vector3(width*1.1f, 0, 0);
        }
        display.transform.localPosition -= new Vector3(0, height * 1.2f * (int)((reservationDisplays.Count - 1) / 2), 0);
    }
}
