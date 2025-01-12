using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    private RobotCore core;

    void Start()
    {
        core = GetComponent<RobotCore>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(0, 0, 0);
        direction += core.transform.forward * vertical;
        direction += core.transform.right * horizontal;
        Debug.Log(direction);
        //core.rb.velocity = direction * 10.0f;
        //core.rb.AddForce(direction.normalized * 200.0f, ForceMode.Force);
        core.transform.position += direction.normalized * 10.0f * Time.deltaTime;
    }
}
