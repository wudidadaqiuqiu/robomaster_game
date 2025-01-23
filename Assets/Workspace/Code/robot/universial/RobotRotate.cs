using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RobotRotate : MonoBehaviour
{
    private float Xrotation;
    private float Yrotation;

    private TextMeshProUGUI pitch_text;
    [SerializeField] private Transform pitch_trans;

    void Start()
    {
        pitch_text = GameObject.Find("pitch_text").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 50.0f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * 50.0f;

        Yrotation += mouseX;
        Xrotation += mouseY;

        if (Xrotation > 40)
        {
            Xrotation = 40;
        }
        if (Xrotation < -30)
        {
            Xrotation = -30;
        }
        Xrotation = Mathf.Clamp(Xrotation, -90, 90);

        transform.rotation = Quaternion.Euler(0, Yrotation + 180.0f, 0);
        pitch_trans.rotation = Quaternion.Euler(Xrotation, Yrotation, 0);

        pitch_text.text = "pitch:" + Xrotation.ToString("0.00");
    }
}
