using Robot;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCore : MonoBehaviour
{
    private RobotState state;

    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
}
