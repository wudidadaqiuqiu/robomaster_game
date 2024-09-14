using UnityEngine;
using Unity.Netcode;
using ROS.ROSConnect;
using RosMessageTypes.Unity;
using InterfaceDef;

using UniRx;
using UnityEditor;

namespace Robots {
public class input_manage : NetworkBehaviour, IRobotComponent
{
    private Subject<object> RobotSubject;

    [SerializeField] private bool _serverAuth;
    [SerializeField] private float delay = 0.1f;
    private InputNetworkStruct nowinput;
    private InputNetworkStruct lastinput; 
    
    public void SetSubject(Subject<object> subject) {
        RobotSubject = subject;
    }

    private void Awake() {
        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        nowinput = new InputNetworkStruct();
        lastinput = new InputNetworkStruct();
    }

    public override void OnNetworkSpawn() {
        if (IsServer && ProjectSettings.GameConfig.ros_debug) {
            ROSConnector.register_publisher<InputKeyboardBitsMsg>(ProjectSettings.RosConfig.input_keyboard_bits_topic);
        }
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
            nowinput.mouse_x = Input.GetAxis("Mouse X");
            nowinput.mouse_y = Input.GetAxis("Mouse Y");

            // nowinput.camera_rotate_y = main_camera.transform.eulerAngles.y;
            if (nowinput != lastinput) {
                if (IsServer) {
                    input_pub();
                } else
                    TransmitStateServerRpc(nowinput);
            }
            lastinput.assign(nowinput);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(InputNetworkStruct state) {
        nowinput = state;
        if (ProjectSettings.GameConfig.unity_debug) {
            // Debug.LogFormat("{0}, {1}, {2}", state.mouse_x, input.Value.camera_rotate_y, state.mouse_y);

            // Debug.Log("server receive input");
        }
        if (ProjectSettings.GameConfig.ros_debug)
        ROSConnector.publish(
            ProjectSettings.RosConfig.input_keyboard_bits_topic, 
            new InputKeyboardBitsMsg(state.keyboard_bits));

        input_pub();
    }

    private void input_pub() {
        // Debug.Log("server input pub");
        RobotSubject.OnNext(nowinput);
    }


}

}