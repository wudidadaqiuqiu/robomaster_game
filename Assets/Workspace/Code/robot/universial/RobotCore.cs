using Robot;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotCore : MonoBehaviour
{
    private RobotState state;

    public Rigidbody rb;

    private GameObject HP_slider_ui;
    private Slider HP_slider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        HP_slider_ui = GameObject.Find("HP");
        HP_slider = HP_slider_ui.GetComponent<Slider>();

        state = new RobotState(RobotType.Infantry1, RobotGroup.Blue);
        state.info_dynamic.HP = 800;
        state.info_fixed.max_HP = 1000;
    }

    void Update()
    {
        state.info_dynamic.HP -= Time.deltaTime * 5f;

        HP_ui_update();
    }

    private void HP_ui_update()
    {
        HP_slider.value = state.get_HP_pro();
    }
}
