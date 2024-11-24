using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartValueManager : MonoBehaviour
{
    public static DartValueManager Instance;

    public int blue_dart_shoot_time = 0;
    public int red_dart_shoot_time = 0;

    public float[] time_node = new float[2];
    public bool[] isTriggers = new bool[2];

    private TimeManager timeManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeManager = TimeManager.Instance;
    }

    private void Update()
    {
        if (!timeManager.getRaceStatus())
        {
            return;
        }

        for (int i = 0; i < time_node.Length; i++)
        {
            if (timeManager.getTimer() < time_node[i] || isTriggers[i])
            {
                continue;
            }

            blue_dart_shoot_time++;
            red_dart_shoot_time++;
            Debug.Log("双方各获得一次飞镖发射机会");

            isTriggers[i] = true;
            break;
        }
    }
}
