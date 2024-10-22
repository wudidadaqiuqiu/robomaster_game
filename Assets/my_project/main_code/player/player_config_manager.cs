using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Player
{

    public class PlayerConfigManager : MonoBehaviour
    {
        private static PlayerConfigManager _instance;
        public ProjectSettings.InGameConfig game_config;
        public GameObject config_panel;
        private bool is_config_panel_open = false;


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

        void Start() {
            is_config_panel_open = false;
            config_panel.SetActive(is_config_panel_open);

            Observable.Interval(System.TimeSpan.FromSeconds(0.5f))
                        .Where(_ => Input.GetKey(KeyCode.Escape))
                        .Subscribe(_ => {
                            is_config_panel_open = !is_config_panel_open;
                            config_panel.SetActive(is_config_panel_open);
                            if (is_config_panel_open) {
                                Cursor.lockState = CursorLockMode.Confined;
                            } else {
                                Cursor.lockState = CursorLockMode.Locked;
                            }
                        }).AddTo(this);
            
        }
    }

}