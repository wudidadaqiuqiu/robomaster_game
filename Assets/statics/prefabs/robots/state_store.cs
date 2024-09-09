using System;
using UnityEngine;
using Unity.Netcode;
using StructDef.Game;


namespace Robots {
    public class StateStore: MonoBehaviour {
        public RobotState state;

        // [System.Serializable]
        public struct store_struct : INetworkSerializable{
            public RobotState state;

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                serializer.SerializeValue(ref state.vision_mode);
            }

        }

        public void struct_change(ref store_struct s) {
            s.state = state;
        }

        public void my_struct_change(ref store_struct s) {
            state = s.state;
        }

        public store_struct get_struct() {
            return new store_struct { state = state };
        }
    }
}