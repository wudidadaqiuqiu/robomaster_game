using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inter2settings : MonoBehaviour
{
    public GameObject first_interface;
    public GameObject control;

    public Button Button_settings;

    void Start()
    {
        Button_settings.onClick.AddListener(shift);
    }

    void Update()
    {

    }
    void shift()
    {
        first_interface.SetActive(false);
        control.SetActive(true);
    }
}
