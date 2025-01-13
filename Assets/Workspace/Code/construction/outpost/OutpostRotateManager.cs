using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostRotateManager : MonoBehaviour
{
    public static OutpostRotateManager Instance;

    public float rotate_stop_time;

    private TimeManager timeManager;

    private bool isRotate = true;

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
        if (!timeManager.is_race())
        {
            return;
        }

        if (timeManager.get_timer() > rotate_stop_time && isRotate)
        {
            isRotate = false;
            MessageManager.Instance.add_message("前哨站自然停止旋转");
        }
    }

    public bool getRotateStatus()
    {
        return isRotate;
    }
}
