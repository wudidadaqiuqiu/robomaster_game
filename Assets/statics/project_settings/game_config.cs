using System;
using Newtonsoft.Json;

namespace ProjectSettings {
    public struct GameConfig {
        public readonly static string main_scene_path = "scenes/game/game";

        public readonly static bool ros_debug = true;

        public readonly static bool unity_debug = true;

        public readonly static string game_config_tag = "config";
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
    }
}