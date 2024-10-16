using System;
using UnityEngine;
using Unity.Netcode;
using StructDef.Game;
using UniRx;
using StructDef.TeamInfo;
using RefereeRelated;

namespace Robots {
    public class StateStore: MonoBehaviour {
        public RobotState state;
        public float camera_rotate_y;

        public ProjectSettings.InGameConfig config;

        void Start() {
            Observable.Interval(System.TimeSpan.FromSeconds(0.1))
            .Where(_ => config.has_init)
            .First()
            .Subscribe(_ => {
                Referee.Singleton.AddRobot(this, config.id);
            }).AddTo(this);
        }

        // [System.Serializable]
        public struct StoreStruct : INetworkSerializable{
            public RobotState state;
            public float camera_rotate_y;
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                serializer.SerializeValue(ref state.vision_mode);
                serializer.SerializeValue(ref camera_rotate_y);
            }

        }

        public struct IngameConfig : INetworkSerializable {
            public TeamInfo team_Info;
            
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                serializer.SerializeValue(ref team_Info.camp);
                serializer.SerializeValue(ref team_Info.id);
            }
        }

        public void ChangeStoreStruct(ref StoreStruct s) {
            s.state = state;
            s.camera_rotate_y = camera_rotate_y;
        }

        public void ChangeMyStruct(ref StoreStruct s) {
            state = s.state;
            camera_rotate_y = s.camera_rotate_y;
        }

        public StoreStruct GetStruct() {
            StoreStruct s = new StoreStruct();
            ChangeStoreStruct(ref s);
            return s;
        }

        public void ChangeIngameConfig(ref IngameConfig s) {
            s.team_Info.camp = config.team == "red" ? CampInfo.camp_red : CampInfo.camp_blue;
            s.team_Info.id = config.id;
        }

        public void ChangeMyIngameConfig(ref IngameConfig s) {
            config.id = s.team_Info.id;
            config.team = s.team_Info.camp == CampInfo.camp_red ? "red" : "blue";
            config.has_init = true;
            // Debug.Log("my_ingame_coonfig_change");
        }

        public IngameConfig GetIngameConfig() {
            IngameConfig s = new IngameConfig();
            ChangeIngameConfig(ref s);
            return s;
        }
    }
}