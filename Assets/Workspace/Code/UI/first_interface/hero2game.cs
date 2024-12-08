using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hero2start : MonoBehaviour
{
    public GameObject start_interface;
    public GameObject hero_selection;

    public Button Button_1;
    public Button Button_2;
    public Button Button_3;
    public Button Button_4;
    public Button Button_5;


    void Start()
    {
        Button_1.onClick.AddListener(tran);
        Button_2.onClick.AddListener(tran);
        Button_3.onClick.AddListener(tran);
        Button_4.onClick.AddListener(tran);
        Button_5.onClick.AddListener(tran);
    }

    void Update()
    {

    }

    void tran()
    {
        start_interface.SetActive(true);
        hero_selection.SetActive(false);
    }
}
