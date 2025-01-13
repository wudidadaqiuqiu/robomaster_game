using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePrefab : MonoBehaviour
{
    [SerializeField] private float life_time;
    private float life_counter = 0;

    void Update()
    {
        if (life_counter > life_time)
        {
            Destroy(gameObject);
        }
        life_counter += Time.deltaTime;
    }
}
