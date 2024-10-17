using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StructDef.Game {
    [System.Serializable]
    public struct ShooterComponentData : IComponentData {
        public BulletType type;
        public Vector3 position;
        public Vector3 direction;
        // 子弹间隔发射计时
        public float delta_time;
    }
    public enum BulletType : byte {
        None,
        Small,
        Big
    }


    public struct BulletEntityData  : IComponentData {
        public Entity small_bullet;
        public Entity big_bullet;
        public float small_scale;
        public float big_scale;
    }

    [System.Serializable]
    public struct BulletComponentData : IComponentData {
        public float3 velocity;
        public float remain_life_time;
    }
}