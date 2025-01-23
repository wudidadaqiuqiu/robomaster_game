using UnityEngine;
using UnityEngine.UI;

public enum ShootMode
{
    sigle_shoot = 0,
    muti_shoot = 1,
};

public class RobotShoot : MonoBehaviour
{
    private RobotCore core;

    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject shoot_mode_ui;
    [SerializeField] private Transform shoot_pos;
    [SerializeField] private float shoot_gap;

    private ShootMode shoot_mode = ShootMode.sigle_shoot;
    private Image shoot_mode_image;
    private float shoot_counter = 0;

    void Start()
    {
        core = GetComponent<RobotCore>();

        shoot_mode_ui = GameObject.Find("shoot_mode");
        shoot_mode_image = shoot_mode_ui.GetComponent<Image>();

        shoot_mode_image.color = Color.green;
    }

    void Update()
    {
        shoot();
        shift_mode();

        if (shoot_counter < shoot_gap)
        {
            shoot_counter += Time.deltaTime;
        }
    }

    private void shoot()
    {
        switch (shoot_mode)
        {
            case ShootMode.sigle_shoot:
                if (Input.GetKeyDown(KeyCode.Mouse0) && core.can_shoot())
                {
                    Instantiate(prefab, shoot_pos.position, shoot_pos.rotation);
                    core.shoot_heat_update();
                    shoot_counter = 0;
                }
                break;
            case ShootMode.muti_shoot:
                if (Input.GetKey(KeyCode.Mouse0) && shoot_counter >= shoot_gap && core.can_shoot())
                {
                    Instantiate(prefab, shoot_pos.position, shoot_pos.rotation);
                    core.shoot_heat_update();
                    shoot_counter = 0;
                }
                break;
        }
    }

    private void shift_mode()
    {
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
