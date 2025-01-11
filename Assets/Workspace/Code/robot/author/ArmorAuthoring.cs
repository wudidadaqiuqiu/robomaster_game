using StructDef.Game;
using Unity.Entities;
using UnityEngine;

namespace RoboticItems
{
    public class ArmorAuthoring : MonoBehaviour
    {
        
    }


    public class ArmorBaker : Baker<ArmorAuthoring>
    {
        public override void Bake(ArmorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new ArmorData
            {
                collision_count = 0,
            });
        }
    }
}