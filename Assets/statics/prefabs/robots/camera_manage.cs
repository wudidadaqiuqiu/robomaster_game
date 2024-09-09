using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using StructDef.Game;

namespace Robots {
public class camera_manage : NetworkBehaviour
{
    [SerializeField] private GameObject camera_;
    private StateStore state_store;
    private Transform third_person;
    private CinemachineVirtualCamera f_camera;
    private CinemachineFreeLook t_camera;
    private Transform first_person;

    public void Awake() {
        third_person = camera_.transform.Find("thirdperson");
        first_person = camera_.transform.Find("firstperson");
        if (third_person == null || first_person == null) {
            Debug.LogError("Virtual Camera not found");
        } else {
            t_camera = third_person.GetComponent<CinemachineFreeLook>();
            f_camera = first_person.GetComponent<CinemachineVirtualCamera>();
        }

        state_store = GetComponent<StateStore>();
    }
    public override void OnNetworkSpawn()
    {
        if (camera_ != null)
        {
            // Debug.Log("Player camera found: " + fpcamera.name);
            if (IsOwner) 
            {
                camera_.SetActive(true);
                t_camera.enabled = true;
                f_camera.enabled = false;
                state_store.state.vision_mode = t_camera.enabled ?
                    RobotVisionMode.third_person : RobotVisionMode.first_person;
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
            // Cursor.lockState = CursorLockMode.Locked;
            t_camera.enabled = !t_camera.enabled;
            f_camera.enabled = !f_camera.enabled;
            state_store.state.vision_mode = t_camera.enabled ?
                RobotVisionMode.third_person : RobotVisionMode.first_person;
        }
    }
}
}