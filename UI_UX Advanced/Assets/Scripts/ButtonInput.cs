using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInput : MonoBehaviour
{
    public void DebugPress()
    {
        Debug.Log("Debug press!");
    }

    public void SwitchToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void TurnGameObjectOn(GameObject go)
    {
        go.SetActive(true);
    }

    public void TurnGameObjectOff(GameObject go)
    {
        go.SetActive(false);
    }

    public void SwichActiveState(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }
}
