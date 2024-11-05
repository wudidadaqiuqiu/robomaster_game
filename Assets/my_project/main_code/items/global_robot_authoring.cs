using StructDef.Game;
using Unity.Entities;
using UnityEngine;

namespace RoboticItems
{
    public class RobotAuthoring : MonoBehaviour
    {
        public GameObject infantry1_prefab;
    }


    public class RobotBaker : Baker<RobotAuthoring>
    {
        public override void Bake(RobotAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new RobotEntityGlobalData
            {
                infantry1_entity = GetEntity(authoring.infantry1_prefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}