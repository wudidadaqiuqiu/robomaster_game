using System.Collections.Generic;
using Robots;
using StructDef.TeamInfo;
using UnityEngine;

namespace RefereeRelated
{

    public class Referee : MonoBehaviour
    {
        private static Referee _instance;
        private static Dictionary<IdentityId, List<StateStore>> robots = new();
        public Dictionary<IdentityId, List<StateStore>> Robots => robots; // 添加此属性以访问机器人的字典
        private static readonly object _lock = new object();

        private Referee() { }

        public static Referee Singleton
        {
            get
            {
                // 双重检查锁定以实现线程安全
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = FindObjectOfType<Referee>();
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject("Referee");
                            _instance = obj.AddComponent<Referee>();
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddRobot(StateStore robot, IdentityId id)
        {
            lock (_lock)     // 确保线程安全
            {
                if (!robots.ContainsKey(id))
                {
                    robots[id] = new List<StateStore>(); // 初始化列表
                }
                robots[id].Add(robot);
                Debug.Log("Added robot with id " + id);
            }
        }

        public List<StateStore> GetRobots(IdentityId id)
        {
            lock (_lock) // 确保线程安全
            {
                if (robots.TryGetValue(id, out var robotList))
                {
                    return robotList;
                }
                return new List<StateStore>(); // 返回空列表，如果没有找到
            }
        }

        public bool AllowToShoot(IdentityId id) {
            // return true;
            if (robots.TryGetValue(id, out var robotList)) {
                if (robotList.Count == 1) {
                    return true;
                } else {
                    return false;
                }
            }
            return false;
        }
    }

}