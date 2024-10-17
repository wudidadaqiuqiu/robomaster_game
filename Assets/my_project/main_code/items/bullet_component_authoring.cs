using UnityEngine;
using Unity.Entities;
using StructDef.Game;

namespace RoboticItems
{
    public class BulletDataAuthoring : MonoBehaviour
    {
        public BulletComponentData data;
    }


    public class BulletDataBaker : Baker<BulletDataAuthoring>
    {
        public override void Bake(BulletDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, authoring.data);
        }
    }
}