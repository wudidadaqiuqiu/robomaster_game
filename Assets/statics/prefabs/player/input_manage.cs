using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.VisualScripting;
using ROS.ROSConnect;
using RosMessageTypes.Unity;

public class input_manage : NetworkBehaviour
{
    [SerializeField] private bool _serverAuth;
    [SerializeField] private float delay = 0.1f;

    private NetworkVariable<InputNetworkStruct> input;
    private InputNetworkStruct nowinput;
    private InputNetworkStruct lastinput; 
    

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
        if (IsOwner)
        {
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

            if (nowinput.keyboard_bits != lastinput.keyboard_bits) {
                if (IsServer)
                    input.Value = nowinput;
                else
                    TransmitStateServerRpc(nowinput);
            }
            lastinput = nowinput;
        }

        if (IsServer)
        {
            if (input.Value.keyboard_bits_get(keyboard_bits_order.W))
                transform.Translate(Vector3.forward * Time.deltaTime * 5);
            if (input.Value.keyboard_bits_get(keyboard_bits_order.S))
                transform.Translate(Vector3.back * Time.deltaTime * 5);
            if (input.Value.keyboard_bits_get(keyboard_bits_order.A))
                transform.Translate(Vector3.left * Time.deltaTime * 5);
            if (input.Value.keyboard_bits_get(keyboard_bits_order.D))
                transform.Translate(Vector3.right * Time.deltaTime * 5);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(InputNetworkStruct state) {
        input.Value = state;
        Debug.Log(state.keyboard_bits);
        if (ProjectSettings.GameConfig.ros_debug)
        ROSConnector.publish(
            ProjectSettings.RosConfig.input_keyboard_bits_topic, 
            new InputKeyboardBitsMsg(state.keyboard_bits));
    }



    private enum keyboard_bits_order : int {
        W = 0,
        S = 1,
        A = 2,
        D = 3,
        Shift = 4,
        Ctrl = 5,
        Q = 6,
        E = 7,
        R = 8,
        F = 9,
        G = 10,
        Z = 11,
        X = 12,
        C = 13,
        V = 14,
        B = 15
    }

    private struct InputNetworkStruct : INetworkSerializable 
    {
        
        private ushort _keyboard_bits;

        internal ushort keyboard_bits {
            get => _keyboard_bits;
            set => _keyboard_bits = value;
        }

        public void keyboard_bits_set(keyboard_bits_order bit, bool value) {
            if (value) {
                _keyboard_bits |= (ushort)(1 << (int)bit);
            }
            else {
                _keyboard_bits &= (ushort)~(1 << (int)bit);
            }
        }

        public bool keyboard_bits_get(keyboard_bits_order bit) {
            return (_keyboard_bits & (ushort)(1 << (int)bit)) != 0;
        }

        // 重写 Equals 方法
        public override bool Equals(object obj)
        {
            if (obj is InputNetworkStruct)
            {
                InputNetworkStruct p = (InputNetworkStruct)obj;
                return _keyboard_bits == p._keyboard_bits;
            }
            return false;
        }

        // 重写 GetHashCode 方法
        public override int GetHashCode()
        {
            return _keyboard_bits.GetHashCode();
        }


        public static bool operator ==(InputNetworkStruct p1, InputNetworkStruct p2)
        {
            return p1.Equals(p2);
        }

        
        public static bool operator !=( InputNetworkStruct p1,  InputNetworkStruct p2)
        {
            return !p1.Equals(p2);
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _keyboard_bits);
        }
    }
}
