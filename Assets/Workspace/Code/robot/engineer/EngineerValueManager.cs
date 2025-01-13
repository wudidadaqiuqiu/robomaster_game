using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerValueManager : MonoBehaviour
{
    public static EngineerValueManager Instance;

    public float initial_defence_increase;
    public float initial_defence_increase_time;

    private bool isInitialDefenceIncrease = true;

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
        if (!timeManager.is_race())
        {
            return;
        }

        if (timeManager.get_timer() > initial_defence_increase_time && isInitialDefenceIncrease)
        {
            isInitialDefenceIncrease = false;
            MessageManager.Instance.add_message("工程机器人初始防御增益失效");
        }
    }
}
