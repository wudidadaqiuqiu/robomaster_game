using System;
using UnityEngine;
using Unity.Netcode;
using StructDef.Game;
using UniRx;

namespace Robots {
    public class StateStore: MonoBehaviour {
        public RobotState state;
        public float camera_rotate_y;

        // [System.Serializable]
        public struct store_struct : INetworkSerializable{
            public RobotState state;
            public float camera_rotate_y;
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                serializer.SerializeValue(ref state.vision_mode);
                serializer.SerializeValue(ref camera_rotate_y);
            }

        }

        public void struct_change(ref store_struct s) {
            s.state = state;
            s.camera_rotate_y = camera_rotate_y;
        }

        public void my_struct_change(ref store_struct s) {
            state = s.state;
            camera_rotate_y = s.camera_rotate_y;
        }

        public store_struct get_struct() {
            store_struct s = new store_struct();
            struct_change(ref s);
            return s;
        }
    }
}