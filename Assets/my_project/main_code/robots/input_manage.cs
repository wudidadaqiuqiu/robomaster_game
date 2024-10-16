using UnityEngine;
using Unity.Netcode;
using Unity.Entities;
using ROS.ROSConnect;
using RosMessageTypes.Unity;
using InterfaceDef;

using UniRx;
using UnityEditor;
using StructDef.Game;

namespace Robots {
public class InputManage : NetworkBehaviour, IRobotComponent
{
    private Subject<object> _subject;

    [SerializeField] private bool _serverAuth;
    [SerializeField] private float delay = 0.1f;
    private InputNetworkData nowinput;
    private InputNetworkData lastinput; 

    public void SetSubject(Subject<object> subject) {
        _subject = subject;
    }

    private void Awake() {
        nowinput = new InputNetworkData();
        lastinput = new InputNetworkData();
    }

    public override void OnNetworkSpawn() {
        // if (IsServer && ProjectSettings.GameConfig.ros_debug) {
        //     ROSConnector.register_publisher<InputKeyboardBitsMsg>(ProjectSettings.RosConfig.input_keyboard_bits_topic);
        // }
    }

    private void Update()
    {

        // Debug.Log("input manage update");
        if (IsOwner)
        {
            // Debug.Log("owner update input");


            nowinput.keyboard_bits_set(keyboard_bits_order.W, Input.GetKey(KeyCode.W));
            nowinput.keyboard_bits_set(keyboard_bits_order.S, Input.GetKey(KeyCode.S));
            nowinput.keyboard_bits_set(keyboard_bits_order.A, Input.GetKey(KeyCode.A));
            nowinput.keyboard_bits_set(keyboard_bits_order.D, Input.GetKey(KeyCode.D));
            nowinput.keyboard_bits_set(keyboard_bits_order.Shift, Input.GetKey(KeyCode.LeftShift));
            nowinput.keyboard_bits_set(keyboard_bits_order.Ctrl, Input.GetKey(KeyCode.LeftControl));
            nowinput.keyboard_bits_set(keyboard_bits_order.Q, Input.GetKey(KeyCode.Q));
            nowinput.keyboard_bits_set(keyboard_bits_order.E, Input.GetKey(KeyCode.E));
            nowinput.keyboard_bits_set(keyboard_bits_order.R, Input.GetKey(KeyCode.R));
            nowinput.keyboard_bits_set(keyboard_bits_order.F, Input.GetKey(KeyCode.F));
            nowinput.keyboard_bits_set(keyboard_bits_order.G, Input.GetKey(KeyCode.G));
            nowinput.keyboard_bits_set(keyboard_bits_order.Z, Input.GetKey(KeyCode.Z));
            nowinput.keyboard_bits_set(keyboard_bits_order.X, Input.GetKey(KeyCode.X));
            nowinput.keyboard_bits_set(keyboard_bits_order.C, Input.GetKey(KeyCode.C));
            nowinput.keyboard_bits_set(keyboard_bits_order.V, Input.GetKey(KeyCode.V));
            nowinput.keyboard_bits_set(keyboard_bits_order.B, Input.GetKey(KeyCode.B));
            
            nowinput.SetMouseButtonBits(mouse_button_order.Left, Input.GetMouseButton(0));
            nowinput.SetMouseButtonBits(mouse_button_order.Right, Input.GetMouseButton(1));
            nowinput.SetMouseButtonBits(mouse_button_order.Middle, Input.GetMouseButton(2));

            nowinput.mouse_x = Input.GetAxis("Mouse X");
            nowinput.mouse_y = Input.GetAxis("Mouse Y");

            // nowinput.camera_rotate_y = main_camera.transform.eulerAngles.y;
            if (nowinput != lastinput) {
                InputPub();
                // if (!IsServer) {
                //     TransmitStateServerRpc(nowinput);
                // }
            }
            lastinput.assign(nowinput);
        }
    }


    private void InputPub() {
        // Debug.Log("server input pub");
        _subject.OnNext(nowinput);
        _subject.OnNext(nowinput.ConvertToSyncData());
    }


}

}