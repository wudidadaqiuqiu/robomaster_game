using System;
using UnityEngine;
using Unity.Netcode;
using StructDef.Game;
using UniRx;
using StructDef.TeamInfo;

namespace Robots {
    public class StateStore: MonoBehaviour {
        public RobotState state;
        public float camera_rotate_y;

        public ProjectSettings.InGameConfig config;

        // [System.Serializable]
        public struct store_struct : INetworkSerializable{
            public RobotState state;
            public float camera_rotate_y;
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                serializer.SerializeValue(ref state.vision_mode);
                serializer.SerializeValue(ref camera_rotate_y);
            }

        }

        public struct ingame_config : INetworkSerializable {
            public team_info team_Info;
            
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                serializer.SerializeValue(ref team_Info.camp);
                serializer.SerializeValue(ref team_Info.id);
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

        public void ingame_config_change(ref ingame_config s) {
            s.team_Info.camp = config.team == "red" ? camp_info.camp_red : camp_info.camp_blue;
            s.team_Info.id = config.id;
        }

        public void my_ingame_coonfig_change(ref ingame_config s) {
            config.id = s.team_Info.id;
            config.team = s.team_Info.camp == camp_info.camp_red ? "red" : "blue";
            config.has_init = true;
            // Debug.Log("my_ingame_coonfig_change");
        }

        public ingame_config get_ingame_config() {
            ingame_config s = new ingame_config();
            ingame_config_change(ref s);
            return s;
        }
    }
}