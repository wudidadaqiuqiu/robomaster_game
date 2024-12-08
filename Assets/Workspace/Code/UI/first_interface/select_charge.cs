using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class select_charge : MonoBehaviour
{
    static public select_charge Instance;

    //mode = 1 µ•»À  //mode = 2 
    public int mode = 0;

    public Button start;

    public GameObject param_interface;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        start.onClick.AddListener(trans);
    }

    void trans()
    {
        if (mode == 2)
        {
            //param_interface.SetActive
            //gameObject.SetActive
        }
        else if (mode == 1)
        {
            ob
        }
    }
}
