using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StructDef.Game {
    [System.Serializable]
    public struct ShooterComponentData : IComponentData {
        public Entity bullet_prefab;
        public ShooterType type;
        public Vector3 position;
        public Vector3 direction;
        // 计时
        public float delta_time;
    }

    public enum ShooterType : byte {
        None,
        Small,
        Big
    }

    public struct BulletComponentData : IComponentData {
        public float3 velocity;
        public float remain_life_time;
    }
}