using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    Room selectedRoom;
    [SerializeField] GameObject roomUI;
    [SerializeField] GameObject standardUI;
    [SerializeField] TextMeshProUGUI roomNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void UpdateUI()
    {
        if(selectedRoom != null)
        {
            roomUI.SetActive(true);
            standardUI.SetActive(false);

            roomNumber.text = "Room: " + selectedRoom.RoomNumber;
        }
        else
        {
            roomUI.SetActive(false);
            standardUI.SetActive(true);
        }
    }

    void SelectRoom(Room pSelected)
    {
        selectedRoom = pSelected;
        UpdateUI();
    }
}
