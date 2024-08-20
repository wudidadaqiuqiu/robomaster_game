using UnityEngine;
using Unity.Netcode;
using ROS.ROSConnect;
using RosMessageTypes.Unity;
using InterfaceDef;

using UniRx;

namespace Robots {
public class input_manage : NetworkBehaviour, IRobotComponent
{
    private Subject<object> RobotSubject;

    [SerializeField] private bool _serverAuth;
    [SerializeField] private float delay = 0.1f;

    private NetworkVariable<InputNetworkStruct> input;
    private InputNetworkStruct nowinput;
    private InputNetworkStruct lastinput; 
    
    public void SetSubject(Subject<object> subject) {
        RobotSubject = subject;
    }

    private void Awake() {
        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        input = new NetworkVariable<InputNetworkStruct>(writePerm: permission);
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
            // Debug.LogFormat("{0}, {1}", nowinput.keyboard_bits, lastinput.keyboard_bits);
            if (nowinput.keyboard_bits != lastinput.keyboard_bits) {
                if (IsServer) {
                    input.Value = nowinput;
                    input_pub();
                } else
                    TransmitStateServerRpc(nowinput);
            }
            lastinput.assign(nowinput);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(InputNetworkStruct state) {
        input.Value = state;
        if (ProjectSettings.GameConfig.unity_debug) {
            Debug.Log(state.keyboard_bits);
            Debug.Log("server receive input");
        }
        if (ProjectSettings.GameConfig.ros_debug)
        ROSConnector.publish(
            ProjectSettings.RosConfig.input_keyboard_bits_topic, 
            new InputKeyboardBitsMsg(state.keyboard_bits));

        input_pub();
    }

    private void input_pub() {
        Debug.Log("server input pub");
        RobotSubject.OnNext(input.Value);
    }


}

}