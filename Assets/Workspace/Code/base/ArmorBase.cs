using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBase : MonoBehaviour
{
    [SerializeField] private string owner_name;

    private ArmorObject owner;

    private void Start()
    {
        owner = GameObject.Find(owner_name).GetComponent<ArmorObject>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8)
        {
            owner.small_hit();
        }
    }
}
