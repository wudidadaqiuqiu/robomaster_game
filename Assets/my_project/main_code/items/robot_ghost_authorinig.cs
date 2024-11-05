using StructDef.Game;
using Unity.Entities;
using UnityEngine;

namespace RoboticItems
{
    public class RobotGhostAuthoring : MonoBehaviour
    {
        
    }

    public struct RobotGhostTag : IComponentData {
        
    }

    public class RobotGhostBaker : Baker<RobotGhostAuthoring>
    {
        public override void Bake(RobotGhostAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<RobotGhostTag>(entity);
        }
    }
}