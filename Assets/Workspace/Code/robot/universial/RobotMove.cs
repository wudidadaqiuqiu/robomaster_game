using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    private RobotCore core;
    private ExchangeManager exchange_manager;
    private TimeManager time_manager;

    void Start()
    {
        core = GetComponent<RobotCore>();
        exchange_manager = ExchangeManager.Instance;
        time_manager = TimeManager.Instance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        move();
    }

    private void move()
    {
        if (exchange_manager.is_exchange() || !time_manager.is_race())
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(0, 0, 0);
        direction += transform.forward * vertical;
        direction += transform.right * horizontal;

        core.rb.AddForce(direction.normalized * 20.0f, ForceMode.Force);
    }
}
