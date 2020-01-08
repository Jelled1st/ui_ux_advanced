using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomDropDownMenu : MonoBehaviour
{
    [SerializeField] GameObject _mainButton;
    Button _button;
    public List<string> options;
    public List<UnityEvent> eventsToCallOnOption;
    List<GameObject> _optionbuttons;

    // Start is called before the first frame update
    void Start()
    {
        if(_mainButton != null)
        {
            _mainButton.TryGetComponent<Button>(out _button);
            _optionbuttons = new List<GameObject>();
            if (options != null && options.Count != 0) _mainButton.GetComponentInChildren<TextMeshProUGUI>().text = options[0];
        }
    }

    public void ShowDropDownMenu()
    {
        ClearOptionButtons();
        if (options == null) return;
        GameObject toCopy = GameObject.Instantiate<GameObject>(_mainButton);
        toCopy.SetActive(false);
        for (int i = 0; i < options.Count; i++)
        {
            GameObject nButton = GameObject.Instantiate<GameObject>(toCopy);
            nButton.SetActive(true);
            nButton.transform.SetParent(_mainButton.transform, false);
            RectTransform tr = nButton.GetComponent<RectTransform>();
            tr.position = new Vector3(tr.position.x, _mainButton.transform.position.y-(tr.sizeDelta.y/2+5)*(i+1));
            nButton.GetComponentInChildren<TextMeshProUGUI>().text = options[i];

            nButton.GetComponent<Button>().onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            int indexToPass = i; // this is weird but just passing 'i' into the delegate won't work, so just use some extra memory ("Screams in agony")
            nButton.GetComponent<Button>().onClick.AddListener(delegate { SwitchToOption(indexToPass); });
            _optionbuttons.Add(nButton);
        }
        Destroy(toCopy);
    }

    private void ClearOptionButtons()
    {
        for(int i = 0; i < _optionbuttons.Count; i++)
        {
            Destroy(_optionbuttons[i]);
        }
        _optionbuttons.Clear();
    }

    public void SwitchToOption(int index)
    {
        ClearOptionButtons();
        _mainButton.GetComponentInChildren<TextMeshProUGUI>().text = options[index];
        eventsToCallOnOption[index].Invoke();
    }
}
