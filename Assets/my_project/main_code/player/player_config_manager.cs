using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerConfigManager : MonoBehaviour
    {
        private static PlayerConfigManager _instance;
        public ProjectSettings.InGameConfig game_config;
        private static readonly object _lock = new object();

        private PlayerConfigManager() { }

        public static PlayerConfigManager Singleton
        {
            get
            {
                // 双重检查锁定以实现线程安全
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = FindObjectOfType<PlayerConfigManager>();
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject("player_config_manager");
                            _instance = obj.AddComponent<PlayerConfigManager>();
                        }
                    }
                }
                return _instance;
            }
        }

        public void LoadConfig(ProjectSettings.InGameConfig config) {
            game_config = config;
        }


    }

}