using MySystems;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.UI;

public enum ShootMode
{
    sigle_shoot = 0,
    muti_shoot = 1,
};

public class RobotShoot : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject shoot_mode_ui;
    [SerializeField] private Transform shoot_pos;

    private ShootMode shoot_mode = ShootMode.sigle_shoot;
    private Image shoot_mode_image;

    void Start()
    {
        shoot_mode_ui = GameObject.Find("shoot_mode");
        shoot_mode_image = shoot_mode_ui.GetComponent<Image>();

        shoot_mode_image.color = Color.green;
    }

    void Update()
    {
        switch (shoot_mode)
        {
            case ShootMode.sigle_shoot:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Instantiate(prefab, shoot_pos.position, shoot_pos.rotation);
                }
                break;
            case ShootMode.muti_shoot:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Instantiate(prefab, shoot_pos.position, shoot_pos.rotation);
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (shoot_mode == ShootMode.sigle_shoot)
            {
                shoot_mode = ShootMode.muti_shoot;
                shoot_mode_image.color = Color.blue;
            }
            else
            {
                shoot_mode = ShootMode.sigle_shoot;
                shoot_mode_image.color = Color.green;
            }
        }
    }
}
