using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    PreGameManager pregame_manager;
    ConfigManager config_manager;

    private bool isCountdown = true;
    private bool isRace = false;
    private bool isEnd = false;

    //debug
    public float timer = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pregame_manager = PreGameManager.Instance;
        config_manager = ConfigManager.Instance;
    }

    private void Update()
    {
        if (pregame_manager.getSartStatus() && !isEnd)
        {
            timer += Time.deltaTime;
        }

        if (timer > config_manager.countdown_time && isCountdown)
        {
            MessageManager.Instance.add_message("比赛开始!");

            timer = 0;
            isCountdown = false;
            isRace = true;
        }

        if (timer > config_manager.race_time && isRace)
        {
            MessageManager.Instance.add_message("比赛结束!");

            timer = 0;
            isRace = false;
            isEnd = true;
        }
    }

    public float get_timer()
    {
        return timer;
    }

    public float get_time_text()
    {
        if (isCountdown)
        {
            return config_manager.countdown_time - timer;
        }
        if (isRace)
        {
            return config_manager.race_time - timer;
        }
        return 0;
    }

    public bool is_countdown()
    {
        return isCountdown;
    }

    public bool is_race()
    {
        return isRace;
    }

    public bool is_end()
    {
        return isEnd;
    }
}
