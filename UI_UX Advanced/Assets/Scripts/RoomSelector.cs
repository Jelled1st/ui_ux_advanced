using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelector : MonoBehaviour
{
    [SerializeField] UIHandler ui;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                RoomHUD room;
                if (raycastHit.transform.gameObject.TryGetComponent<RoomHUD>(out room))
                {
                    Debug.Log(room.GetReservation(0).ToString());
                }
            }
        }
    }
}
