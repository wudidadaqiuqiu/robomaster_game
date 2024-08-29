using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

namespace Robots {
public class camera_manage : NetworkBehaviour
{
    [SerializeField] private GameObject camera_;

    private Transform third_person;
    private Transform first_person;

    public void Awake() {
        third_person = camera_.transform.Find("thirdperson");
        first_person = camera_.transform.Find("firstperson");
        if (third_person == null || first_person == null) {
            Debug.LogError("Virtual Camera not found");
        }
    }
    public override void OnNetworkSpawn()
    {
        if (camera_ != null)
        {
            // Debug.Log("Player camera found: " + fpcamera.name);
            if (IsOwner) 
            {
                camera_.SetActive(true);
                third_person.GetComponent<CinemachineVirtualCamera>().enabled = true;
                first_person.GetComponent<CinemachineVirtualCamera>().enabled = false;

            }
            else
            {
                camera_.SetActive(false);
            }
        }
    }

    void Update() {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.F3)) {
            third_person.GetComponent<CinemachineVirtualCamera>().enabled = 
                !third_person.GetComponent<CinemachineVirtualCamera>().enabled;
            first_person.GetComponent<CinemachineVirtualCamera>().enabled = 
                !first_person.GetComponent<CinemachineVirtualCamera>().enabled;
        }
    }
}
}