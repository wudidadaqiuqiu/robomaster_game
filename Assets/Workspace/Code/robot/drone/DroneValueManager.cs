using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneValueManager : MonoBehaviour
{
    public static DroneValueManager Instance;

    public float blue_aid_time = 0;
    public float red_aid_time = 0;
    public float initial_aid_time;
    public float nature_increase_aid_time;
    public float[] time_node = new float[6];
    public bool[] isTriggers = new bool[6];

    private TimeManager timeManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeManager = TimeManager.Instance;

        blue_aid_time = initial_aid_time;
        red_aid_time = initial_aid_time;
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

            blue_aid_time += nature_increase_aid_time;
            red_aid_time += nature_increase_aid_time;
            Debug.Log("双方各获得20s无人机支援时间");

            isTriggers[i] = true;
            break;
        }
    }
}
