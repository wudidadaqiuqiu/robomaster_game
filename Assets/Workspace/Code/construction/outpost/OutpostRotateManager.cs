using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostRotateManager : MonoBehaviour
{
    public static OutpostRotateManager Instance;

    private ConfigManager config_manager;
    private TimeManager time_manager;

    [SerializeField] private Transform rotateTransform;

    private bool isRotate = true;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        time_manager = TimeManager.Instance;
        config_manager = ConfigManager.Instance;
    }

    private void Update()
    {
        if (!time_manager.is_race())
        {
            return;
        }

        if (time_manager.get_timer() > config_manager.rotate_stop_time && isRotate)
        {
            isRotate = false;
            MessageManager.Instance.add_message("前哨站自然停止旋转");
        }

        if (time_manager.get_timer() < config_manager.rotate_stop_time || !isRotate)
        {
            rotateTransform.localPosition = Vector3.zero;
            rotateTransform.rotation = Quaternion.Euler(0, time_manager.get_timer() * config_manager.outpost_rotate_speed, 0);
        }
    }

    public bool getRotateStatus()
    {
        return isRotate;
    }
}
