using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shift_first2game : MonoBehaviour
{
    public GameObject first_interface;
    public GameObject hero_selection;

    public Button Button_sp;

    void Start()
    {
        Button_sp.onClick.AddListener(shift);
    }

    void Update()
    {

    }
    void shift()
    {
       first_interface.SetActive(false);
        hero_selection. SetActive(true);
    }
}
