namespace ProjectSettings {
    public struct GameConfig {
        public readonly static string main_scene_path = "scenes/game/game";

        public readonly static bool ros_debug = true;

        public readonly static bool unity_debug = true;
    }

    public struct RosConfig {
        public readonly static string ros_ip = "127.0.0.1";
        public readonly static int ros_port = 10000;
        public readonly static string input_keyboard_bits_topic = "unity/input/keyboard_bits";
    }

    public class ClientConfig {
        public string ip;
        public ushort port;
    }
}