using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRotate : MonoBehaviour
{
    private float Xrotation;
    private float Yrotation;

    [SerializeField] private Transform camera_trans;

    void Start()
    {

    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 50.0f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * 50.0f;

        Yrotation += mouseX;

        Xrotation += mouseY;
        Xrotation = Mathf.Clamp(Xrotation, -90, 90);

        transform.rotation = Quaternion.Euler(0, Yrotation + 180.0f, 0);
        camera_trans.rotation = Quaternion.Euler(Xrotation, Yrotation, 0);
    }
}
