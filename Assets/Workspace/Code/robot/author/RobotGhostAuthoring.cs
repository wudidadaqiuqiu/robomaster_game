using StructDef.Game;
using Unity.Entities;
using UnityEngine;

namespace RoboticItems
{
    public class RobotGhostAuthoring : MonoBehaviour
    {
        
    }

    public struct RobotGhostComponent : IComponentData {
        public uint collision_cnt;
    }

    public class RobotGhostBaker : Baker<RobotGhostAuthoring>
    {
        public override void Bake(RobotGhostAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<RobotGhostComponent>(entity);
        }
    }
}