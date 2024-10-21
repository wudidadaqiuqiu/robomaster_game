using UnityEngine;
using Unity.Entities;
using StructDef.Game;

namespace RoboticItems
{
    public class BulletAuthoring : MonoBehaviour
    {
        public GameObject small_bullet_prefab;
        public GameObject big_bullet_prefab;
    }


    public class BulletBaker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new BulletGlobalData
            {
                small_bullet = GetEntity(authoring.small_bullet_prefab, TransformUsageFlags.Dynamic),
                big_bullet = GetEntity(authoring.big_bullet_prefab, TransformUsageFlags.Dynamic),
                small_scale = 0.017f,
                big_scale = 0.042f,
            });
            // Debug.Log("ShooterBaker");
            // GetComponent<ShootControl>().SetEntity(entity);
        }
    }
}