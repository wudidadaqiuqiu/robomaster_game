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
            Debug.Log("race begin!");

            timer = 0;
            isCountdown = false;
            isRace = true;
        }

        if (timer > race_time && isRace)
        {
            Debug.Log("race end!");

            timer = 0;
            isRace = false;
            isEnd = true;
        }
    }

    public float getTimer()
    {
        return timer;
    }

    public bool getRaceStatus()
    {
        return isRace;
    }
}
