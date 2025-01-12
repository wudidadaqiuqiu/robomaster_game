using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float life_time;
    private float life_counter = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 8.0f, ForceMode.Force);
    }

    void Update()
    {
        if (life_counter > life_time)
        {
            Destroy(gameObject);
        }
        life_counter += Time.deltaTime;
    }
}
