using Unity.Burst;
using UnityEngine;

namespace Player {
    class DeveloperConfigManager : MonoBehaviour {
        private static DeveloperConfigManager _instance;
        private static readonly object _lock = new object();
        public ProjectSettings.GameConfig gameConfig;


        public static DeveloperConfigManager Singleton
        {
            get
            {
                // 双重检查锁定以实现线程安全
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = FindObjectOfType<DeveloperConfigManager>();
                        if (_instance == null)
                        {
                            Debug.LogError("DeveloperConfigManager is not found!");
                            GameObject obj = new GameObject("developer_config_manager");
                            _instance = obj.AddComponent<DeveloperConfigManager>();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}