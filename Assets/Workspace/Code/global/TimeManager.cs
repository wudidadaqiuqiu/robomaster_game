using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    PreGameManager pregame_manager;

    [SerializeField] private float countdown_time;
    [SerializeField] private float race_time;

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
    }

    private void Update()
    {
        if (pregame_manager.getSartStatus() && !isEnd)
        {
            timer += Time.deltaTime;
        }

        if (timer > countdown_time && isCountdown)
        {
            MessageManager.Instance.add_message("比赛开始!");

            timer = 0;
            isCountdown = false;
            isRace = true;
        }

        if (timer > race_time && isRace)
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
            return countdown_time - timer;
        }
        if (isRace)
        {
            return race_time - timer;
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
