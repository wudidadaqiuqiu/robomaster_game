using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [SerializeField] private TextMeshProUGUI blue_ui;
    [SerializeField] private TextMeshProUGUI red_ui;

    //debug
    public float[] time_node = new float[6];
    public bool[] isTriggers = new bool[6];

    public int blue_gold;
    public int red_gold;

    private TimeManager timeManager;
    private ConfigManager configManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeManager = TimeManager.Instance;
        configManager = ConfigManager.Instance;

        blue_gold = configManager.init_blue_gold;
        red_gold = configManager.init_red_gold;
    }

    private void Update()
    {
        if (!timeManager.is_race())
        {
            return;
        }

        ui_update();
        natureIncrease();
    }

    private void ui_update()
    {
        blue_ui.text = blue_gold.ToString();
    }

    private void natureIncrease()
    {
        for (int i = 0; i < time_node.Length; i++)
        {
            if (timeManager.get_timer() < time_node[i] || isTriggers[i])
            {
                continue;
            }

            //第六分钟获得150经济加成
            if (i == 5)
            {
                MessageManager.Instance.add_message("双方经济自然增长150");
                blue_gold += 150;
                red_gold += 150;
            }
            //前五分钟每分钟获得100经济加成
            else
            {
                MessageManager.Instance.add_message("双方经济自然增长50");
                blue_gold += 50;
                red_gold += 50;
            }
            isTriggers[i] = true;
            break;
        }
    }
}
