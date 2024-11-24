using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMachineManager : MonoBehaviour
{
    public static EnergyMachineManager Instance;

    private TimeManager timeManager;

    public float[] time_node = new float[5];
    public bool[] isTriggers = new bool[5];
    public float sustain_time;

    public float rotate_timer = 0;

    private bool isRotate = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeManager = TimeManager.Instance;
    }

    void Update()
    {
        if (!timeManager.getRaceStatus())
        {
            return;
        }

        for (int i = 0; i < time_node.Length; i++)
        {
            if (timeManager.getTimer() < time_node[i] || isTriggers[i])
            {
                continue;
            }

            if (i == 0 || i == 1)
            {
                Debug.Log("С�������ؿ�ʼ��ת");
            }
            else
            {
                Debug.Log("���������ؿ�ʼ��ת");
            }

            isRotate = true;
            isTriggers[i] = true;
            break;
        }

        if (isRotate)
        {
            rotate_timer += Time.deltaTime;

            if (rotate_timer > sustain_time)
            {
                rotate_timer = 0;
                isRotate = false;
                Debug.Log("��������ֹͣ��ת");
            }
        }
    }
}
