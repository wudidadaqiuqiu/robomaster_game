using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Robot;

public class RobotSync : NetworkBehaviour
{
    public RobotCore core;

    private NetworkVariable<Vector3> net_position = new NetworkVariable<Vector3>();
    private NetworkVariable<Quaternion> net_rotation = new NetworkVariable<Quaternion>();
    private NetworkVariable<RobotInfoDynamic> net_info = new NetworkVariable<RobotInfoDynamic>();

    void Start()
    {
        core = GetComponent<RobotCore>();
    }

    
}
