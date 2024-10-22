using System;
using StructDef.TeamInfo;

namespace ProjectSettings {
    [Serializable]
    public struct GameConfig {
        public readonly static string main_scene_path = "scenes/game/game";

        public readonly static bool ros_debug = true;

        public readonly static bool unity_debug = true;

        public readonly static string game_config_tag = "config";
        public readonly static float shoot_delay_time = 0.05f;
        public readonly static float bullet_life_time = 30.0f;
    }

    public struct RosConfig {
        public readonly static string ros_ip = "127.0.0.1";
        public readonly static int ros_port = 10000;
        public readonly static string input_keyboard_bits_topic = "unity/input/keyboard_bits";
    }

    [Serializable]
    public class InGameConfig {
        public string ip;
        public ushort port;
        public string team;
        public ushort id;

        public bool has_init = false;

        public float mouse_sensitivity_hor;
        public float mouse_sensitivity_ver;
        public bool debug_mode;

        public CampInfo ToCampInfo() {
            CampInfo campinfo;
            if (team == "red") {
                campinfo = CampInfo.camp_red;
            } else if (team == "blue") {
                campinfo = CampInfo.camp_blue;
            } else {
                campinfo = CampInfo.camp_none;
            }
            return campinfo;
        }

        public int ToIntId() {
            CampInfo campInfo = ToCampInfo();
            return (int)campInfo * 100 + id;
        }

        public IdentityId ToIdentityId() {
            return new IdentityId(ToIntId());
        }
        
    }
}