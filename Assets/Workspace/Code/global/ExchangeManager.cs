using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeManager : MonoBehaviour
{
    public static ExchangeManager Instance;

    [SerializeField] private GameObject exchange_ui;

    private TimeManager time_manager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        time_manager = TimeManager.Instance;
    }

    private void Update()
    {
        control_exchange();
    }

    public void control_exchange()
    {
        if (Input.GetKeyDown(KeyCode.P) && time_manager.is_race())
        {
            exchange_ui.SetActive(!exchange_ui.activeSelf);
        }
    }

    public bool is_exchange()
    {
        return exchange_ui.activeSelf;
    }
}
