using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shift_first2start: MonoBehaviour
{
    public GameObject first_interface;
    public GameObject hero_selection;

    public Button Button_mp;

    void Start()
    {
        Button_mp.onClick.AddListener(tran);
    }

    void Update()
    {
        
    }

    void tran()
    {
        first_interface.SetActive(false);
        hero_selection.SetActive(true);

        select_charge.Instance.mode = 2;
    }
}
