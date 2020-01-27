using UnityEngine;
using UnityEngine.SceneManagement;

public class WebURLScript : MonoBehaviour
{

    public string Url;

    public void Open()
    {
        Application.OpenURL("https://account.saxion.nl/");
    }
    

}