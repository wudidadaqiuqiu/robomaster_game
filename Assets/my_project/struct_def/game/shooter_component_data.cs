using Unity.Entities;
using UnityEngine;

namespace StructDef.Game {
    public struct ShooterComponentData : IComponentData {
        public ShooterType type;
        public Vector3 position;
        public Vector3 direction;
    }

    public enum ShooterType : byte {
        None,
        Small,
        Big
    }
}