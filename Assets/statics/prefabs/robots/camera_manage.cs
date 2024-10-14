using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using StructDef.Game;
using UniRx;

namespace Robots {
public class CameraManage : NetworkBehaviour
{
    [SerializeField] private GameObject camera_;
    // [SerializeField] private GameObject first_person_camera;
    private StateStore state_store;
    private Transform third_person;
    private CinemachineVirtualCamera f_camera;
    private CinemachineFreeLook t_camera;
    private GameObject main_camera;
    [SerializeField] private Transform first_person;

    public void Awake() {
        third_person = camera_.transform.Find("thirdperson");
        // first_person = transform.Find("firstperson");
        if (third_person == null || first_person == null) {
            Debug.LogError("Virtual Camera not found");
        } else {
            t_camera = third_person.GetComponent<CinemachineFreeLook>();
            f_camera = first_person.GetComponent<CinemachineVirtualCamera>();
        }

        state_store = GetComponent<StateStore>();
        main_camera = GameObject.FindWithTag("MainCamera");
    }

    void Start() {
        // if (IsServer)
        if (IsOwner) {
            Observable.Interval(System.TimeSpan.FromSeconds(0.05))
            .Subscribe(_ => {
                state_store.camera_rotate_y = main_camera.transform.eulerAngles.y;
            }).AddTo(this);            
        }
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner) {
            t_camera.enabled = true;
            f_camera.enabled = false;
            state_store.state.vision_mode = t_camera.enabled ?
                RobotVisionMode.third_person : RobotVisionMode.first_person;
        } else {
            t_camera.enabled = false;
            f_camera.enabled = false;
        }
    }

    void Update() {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.F3)) {
            Cursor.lockState = CursorLockMode.Locked;
            t_camera.enabled = !t_camera.enabled;
            f_camera.enabled = !f_camera.enabled;
            state_store.state.vision_mode = t_camera.enabled ?
                RobotVisionMode.third_person : RobotVisionMode.first_person;
        }
    }
}
}