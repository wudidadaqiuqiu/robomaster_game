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

            //�������ӻ��150���üӳ�
            if (i == 5)
            {
                Debug.Log("˫��������Ȼ����150");
                blue_gold += 150;
                red_gold += 150;
            }
            //ǰ�����ÿ���ӻ��100���üӳ�
            else
            {
                Debug.Log("˫��������Ȼ����50");
                blue_gold += 50;
                red_gold += 50;
            }
            isTriggers[i] = true;
            break;
        }
    }
}
