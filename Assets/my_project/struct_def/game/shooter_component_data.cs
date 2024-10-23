using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StructDef.Game {
    [System.Serializable]
    public struct ShooterComponentData : IComponentData {
        public BulletType type;
        public Vector3 position;
        public Vector3 velocity;
        // 子弹间隔发射计时
        public float delta_time;
        public float speed;
    }
    public enum BulletType : byte {
        None,
        Small,
        Big
    }


    public struct BulletGlobalData  : IComponentData {
        public Entity small_bullet;
        public Entity big_bullet;
        public float small_scale;
        public float big_scale;
    }

    [System.Serializable]
    public struct BulletData : IComponentData {
        public float3 velocity;
        public float remain_life_time;
    }
}