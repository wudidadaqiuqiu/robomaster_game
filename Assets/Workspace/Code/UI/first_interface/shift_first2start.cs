using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shift_first2start : MonoBehaviour
{
    public GameObject first_interface;
    public GameObject start_interface;

    public Button Button_mp;

    void Start()
    {
        Button_mp.onClick.AddListener(shift);
    }

    void Update()
    {
        
    }
    void shift()
    {
        first_interface.SetActive(false);
        start_interface.SetActive(true);
    }
}
