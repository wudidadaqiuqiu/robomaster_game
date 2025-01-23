using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Space]
    public float countdown_time;
    public float race_time;

    [Space]
    public float outpost_rotate_speed;
    public float rotate_stop_time;

    [Space]
    public int init_blue_gold;
    public int init_red_gold;
}
