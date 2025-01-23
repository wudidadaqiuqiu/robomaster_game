using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private string owner_name;

    private GameObject armor_owner;

    private void Start()
    {
        armor_owner = GameObject.Find(owner_name);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8)
        {

        }
    }
}
