using Unity.Burst;
using UnityEngine;

namespace Player {
    class DeveloperConfigManager : MonoBehaviour {
        private static DeveloperConfigManager Instance;
        private static readonly object _lock = new object();
        public ProjectSettings.GameConfig gameConfig;


        public static DeveloperConfigManager Singleton
        {
            get
            {
                // 双重检查锁定以实现线程安全
                if (Instance == null)
                {
                    lock (_lock)
                    {
                        Instance = FindObjectOfType<DeveloperConfigManager>();
                        if (Instance == null)
                        {
                            Debug.LogError("DeveloperConfigManager is not found!");
                            GameObject obj = new GameObject("developer_config_manager");
                            Instance = obj.AddComponent<DeveloperConfigManager>();
                        }
                    }
                }
                return Instance;
            }
        }
    }
}