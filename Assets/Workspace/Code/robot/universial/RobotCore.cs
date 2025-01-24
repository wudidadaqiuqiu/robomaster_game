using Robot;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RobotCore : MonoBehaviour
{
    public RobotState state;

    public Rigidbody rb;

    private Slider HP_slider;
    private TextMeshProUGUI HP_num;
    private TextMeshProUGUI HP_num_max;

    private float heat_cool = 0.02f;
    private float cool_counter = 0;
    private Slider heat_slider;

    private TextMeshProUGUI bullet_num;
    private TextMeshProUGUI grade_num;
    private TextMeshProUGUI group_tag;
    private TextMeshProUGUI index_tag;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        HP_slider = GameObject.Find("HP").GetComponent<Slider>();
        HP_num = GameObject.Find("HP_number").GetComponent<TextMeshProUGUI>();
        HP_num_max = GameObject.Find("HP_number_max").GetComponent<TextMeshProUGUI>();

        heat_slider = GameObject.Find("heat").GetComponent <Slider>();

        bullet_num = GameObject.Find("bullet_number").GetComponent<TextMeshProUGUI>();
        grade_num = GameObject.Find("grade_number").GetComponent<TextMeshProUGUI>();
        group_tag = GameObject.Find("group_tag").GetComponent<TextMeshProUGUI>();
        index_tag = GameObject.Find("index_tag").GetComponent<TextMeshProUGUI>();

        state = new RobotState(RobotType.Infantry1, RobotGroup.Blue);

        state.info_dynamic.HP = 800;
        state.info_fixed.max_HP = 1000;

        state.info_dynamic.heat = 0;
        state.info_fixed.max_heat = 1000;

        state.info_fixed.bullet_num = 10;

        state.info_dynamic.grade = 1;
        grade_ui_update();
        tag_ui_update();
    }

    void Update()
    {
        HP_ui_update();
        heat_ui_update();
        bullet_ui_update();
    }

    private void HP_ui_update()
    {
        HP_slider.value = state.get_HP_pro();
        HP_num.text = state.info_dynamic.HP.ToString();
        HP_num_max.text = "/" + state.info_fixed.max_HP.ToString();
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

    private void bullet_ui_update()
    {
        bullet_num.text = state.info_fixed.bullet_num.ToString();
    }

    private void grade_ui_update()
    {
        grade_num.text = state.info_dynamic.grade.ToString();
    }

    private void tag_ui_update()
    {
        if (state.group == RobotGroup.Blue)
        {
            group_tag.text = "蓝方";
        }
        else if (state.group == RobotGroup.Red)
        {
            group_tag.text = "红方";
        }

        index_tag.text = ((int)state.type).ToString();
    }

    public void shoot_heat_update()
    {
        if (state.info_dynamic.heat <= state.info_fixed.max_heat - 50)
        {
            state.info_dynamic.heat += 50;
        }
    }

    public void shoot_bullet_update()
    {
        if (state.info_fixed.bullet_num > 0)
        {
            state.info_fixed.bullet_num--;
        }
    }

    public bool can_shoot()
    {
        if (state.info_dynamic.heat > state.info_fixed.max_heat - 50)
        {
            Debug.Log("超热量");
            return false;
        }

        if (state.info_fixed.bullet_num == 0)
        {
            Debug.Log("没有子弹");
            return false;
        }

        return true;    
    }
}
