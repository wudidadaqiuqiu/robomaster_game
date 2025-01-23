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

    private Slider HP_slider;

    private float heat_cool = 0.02f;
    private float cool_counter = 0;
    private Slider heat_slider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        HP_slider = GameObject.Find("HP").GetComponent<Slider>();
        heat_slider = GameObject.Find("heat").GetComponent <Slider>();

        state = new RobotState(RobotType.Infantry1, RobotGroup.Blue);

        state.info_dynamic.HP = 800;
        state.info_fixed.max_HP = 1000;

        state.info_dynamic.heat = 0;
        state.info_fixed.max_heat = 1000;
    }

    void Update()
    {
        HP_ui_update();
        heat_ui_update();
    }

    private void HP_ui_update()
    {
        HP_slider.value = state.get_HP_pro();
    }

    private void heat_ui_update()
    {
        if (cool_counter > heat_cool && state.info_dynamic.heat > 0)
        {
            cool_counter = 0;
            state.info_dynamic.heat--;
        }

        cool_counter += Time.deltaTime;
        heat_slider.value = state.get_heat_pro();
    }

    public void shoot_heat_update()
    {
        if (state.info_dynamic.heat <= state.info_fixed.max_heat - 50)
        {
            state.info_dynamic.heat += 50;
        }
    }

    public bool can_shoot()
    {
        if (state.info_dynamic.heat > state.info_fixed.max_heat - 50)
        {
            Debug.Log("≥¨»»¡ø");
            return false;
        }
        return true;    
    }
}
