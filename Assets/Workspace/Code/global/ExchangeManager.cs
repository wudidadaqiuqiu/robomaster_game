using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeManager : MonoBehaviour
{
    public static ExchangeManager Instance;

    [SerializeField] private GameObject exchange_ui;

    [SerializeField] private Button small_selection1;
    [SerializeField] private Button small_selection2;
    [SerializeField] private Button small_selection3;

    [SerializeField] private Button big_selection1;
    [SerializeField] private Button big_selection2;
    [SerializeField] private Button big_selection3;

    private RobotSync[] syncs;

    private TimeManager time_manager;
    private EconomyManager economy_manager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        exchange_ui.SetActive(false);

        time_manager = TimeManager.Instance;
        economy_manager = EconomyManager.Instance;

        small_selection1.onClick.AddListener(() => button_selection(10, 10));
        small_selection2.onClick.AddListener(() => button_selection(50, 50));
        small_selection3.onClick.AddListener(() => button_selection(100, 100));
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
            if (exchange_ui.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = exchange_ui.activeSelf;
        }
    }

    public bool is_exchange()
    {
        return exchange_ui.activeSelf;
    }

    private void button_selection(int bullet, int gold)
    {
        syncs = FindObjectsOfType<RobotSync>();
        foreach (RobotSync sync in syncs)
        {
            if (!sync.IsOwner)
            {
                continue;
            }

            if (sync.core.state.group == Robot.RobotGroup.Blue && economy_manager.blue_gold >= gold)
            {
                economy_manager.blue_gold -= gold;
                sync.core.state.info_fixed.bullet_num += bullet;
                return;
            }
            else
            {
                Debug.Log("金币不足");
            }

            if (sync.core.state.group == Robot.RobotGroup.Red && economy_manager.red_gold >= gold)
            {
                economy_manager.red_gold -= gold;
                sync.core.state.info_fixed.bullet_num += bullet;
                return;
            }
            else
            {
                Debug.Log("金币不足");
            }
        }
    }
}
