using Unity.Entities;

namespace StructDef.Game {
    public struct RobotEntityGlobalData : IComponentData {
        public Entity infantry1_entity;
    }

    public enum RobotType {
        Infantry1,
    }
}