using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutpostManager : ArmorObject
{
    [SerializeField] private Slider state_slider;

    public override void small_hit()
    {
        base.small_hit();
        update_HP();
        Debug.Log("小弹击中前哨站装甲板");
    }

    private void update_HP()
    {
        state_slider.value = HP / max_HP;
    }
}
