using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    //debug
    public float[] time_node = new float[6];
    public bool[] isTriggers = new bool[6];

    public int initial_gold = 400;

    public int blue_gold;
    public int red_gold;

    private TimeManager timeManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeManager = TimeManager.Instance;

        blue_gold = initial_gold;
        red_gold = initial_gold;
    }

    private void Update()
    {
        if (!timeManager.getRaceStatus())
        {
            return;
        }

        natureIncrease();
    }

    private void natureIncrease()
    {
        for (int i = 0; i < time_node.Length; i++)
        {
            if (timeManager.getTimer() < time_node[i] || isTriggers[i])
            {
                continue;
            }

            //第六分钟获得150经济加成
            if (i == 5)
            {
                Debug.Log("双方经济自然增长150");
                blue_gold += 150;
                red_gold += 150;
            }
            //前五分钟每分钟获得100经济加成
            else
            {
                Debug.Log("双方经济自然增长50");
                blue_gold += 50;
                red_gold += 50;
            }
            isTriggers[i] = true;
            break;
        }
    }
}
