using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class time_ui_manager : MonoBehaviour
{
    [SerializeField] private GameObject time_ui;

    private TextMeshProUGUI time_text;
    private TimeManager time_manager;

    void Start()
    {
        time_manager = TimeManager.Instance;
        time_text = time_ui.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        int number = (int)time_manager.get_time_text();

        int min = number / 60;
        int sec = number % 60;

        time_text.text = min.ToString() + ":" + sec.ToString();
    }
}
