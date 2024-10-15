using Unity.Entities;
using UnityEngine;

namespace StructDef.Game {
    public struct ShooterComponentData : IComponentData {
        public float speed;
        public Vector3 position;
        public Vector3 direction;
        public float shoot_delta_time;

    }
}