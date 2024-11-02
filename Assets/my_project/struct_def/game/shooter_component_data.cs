using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StructDef.Game {
    [System.Serializable]
    public struct ShooterCmdData : IComponentData {
        public BulletType type;
        public Vector3 position;
        public Vector3 velocity;
    }

    [System.Serializable]
    public struct ShooterSystemData : IComponentData {
        // 子弹间隔发射计时
        public float delta_time;
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
        // public float3 velocity;
        public float remain_life_time;
    }

    public struct ArmorData : IComponentData {
        public int collision_count;
    }

    public struct BulletTag : IComponentData {
        
    }
}