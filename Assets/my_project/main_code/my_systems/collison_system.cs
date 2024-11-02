using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using StructDef.Game;
using UnityEngine;
// using System.Diagnostics;

namespace MySystems
{
    // [BurstCompile]
    public struct ArmorCollisionEventJob : ICollisionEventsJob
    {
        public ComponentLookup<ArmorData> ArmorGroup;
        [ReadOnly] public ComponentLookup<BulletTag> BulletGroup;
        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;
            
            // Debug.Log("Collision detected between ");
            Debug.Log(entityA.ToFixedString());
            // if 
            if (ArmorGroup.HasComponent(entityA) && BulletGroup.HasComponent(entityB))
            {
                var armor_data = ArmorGroup[entityA];
                armor_data.collision_count++;
                ArmorGroup[entityA] = armor_data;
                Debug.Log("Armor collided with bullet");
            }

            if (ArmorGroup.HasComponent(entityB) && BulletGroup.HasComponent(entityA))
            {
                var armor_data = ArmorGroup[entityB];
                armor_data.collision_count++;
                ArmorGroup[entityB] = armor_data;
                Debug.Log("Armor collided with bullet");
            }
        }
    }

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ArmorCollisionEventSystem : ISystem
    {
        private ComponentLookup<ArmorData> armorGroup;
        private ComponentLookup<BulletTag> bulletGroup;

        public void OnCreate(ref SystemState state)
        {
            armorGroup = state.GetComponentLookup<ArmorData>();
            bulletGroup = state.GetComponentLookup<BulletTag>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            // 更新 ComponentLookup
            armorGroup.Update(ref state);
            bulletGroup.Update(ref state);
            // Debug.Log("ArmorCollisionEventSystem OnUpdate");
            var armor_detect_job = new ArmorCollisionEventJob
            {
                ArmorGroup = armorGroup,
                BulletGroup = bulletGroup,
            };
            
            state.Dependency = armor_detect_job.Schedule(
                SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
    }

}