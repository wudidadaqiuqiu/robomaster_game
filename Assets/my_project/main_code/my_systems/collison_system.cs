using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using StructDef.Game;
using UnityEngine;
using RoboticItems;
// using System.Diagnostics;

namespace MySystems
{
    // [BurstCompile]
    public struct ArmorCollisionEventJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<RobotGhostTag> robotGhostGroup;
        [ReadOnly] public ComponentLookup<BulletTag> BulletGroup;
        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;
            Debug.Log("in job");
            // Debug.Log("Collision detected between ");
            // Debug.Log(entityA.ToFixedString());
            // Debug.Log(entityB.);
            // if 
            // DebugComponents(entityA);
            // Debug.Log(ArmorGroup.)
            if (BulletGroup.HasComponent(entityA)) {
                // Debug.Log("Bullet collision detected");
                if (robotGhostGroup.HasComponent(entityB))
                {
                    Debug.Log("Armor and Bullet collision detected");
                    // var armor_data = ArmorGroup[entityB];
                    // armor_data.collision_count++;
                    // ArmorGroup[entityB] = armor_data;
                }
            }

            if (BulletGroup.HasComponent(entityB)) {
                // Debug.Log("Bullet collision detected");
                if (robotGhostGroup.HasComponent(entityA))
                {
                    Debug.Log("Armor and Bullet collision detected");
                    // var armor_data = ArmorGroup[entityB];
                    // armor_data.collision_count++;
                    // ArmorGroup[entityB] = armor_data;
                }
            }

            // if (ArmorGroup.HasComponent(entityA)) {
            //     Debug.Log("Armor collision detected");
            // }

            
            // if (ArmorGroup.HasComponent(entityA) && BulletGroup.HasComponent(entityB))
            // {
            //     var armor_data = ArmorGroup[entityA];
            //     armor_data.collision_count++;
            //     ArmorGroup[entityA] = armor_data;
            //     Debug.Log("Armor collided with bullet");
            // }

            // if (ArmorGroup.HasComponent(entityB) && BulletGroup.HasComponent(entityA))
            // {
            //     var armor_data = ArmorGroup[entityB];
            //     armor_data.collision_count++;
            //     ArmorGroup[entityB] = armor_data;
            //     Debug.Log("Armor collided with bullet");
            // }
        }

        void DebugComponents(Entity entity)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            if (entityManager.HasComponent<BulletTag>(entity))
            {
                // var component = entityManager.GetComponentData<BulletTag>(entity);
                Debug.Log($"Entity has YourComponentType: BulletTag");
                if (!BulletGroup.HasComponent(entity))
                {
                    Debug.LogWarning("Entity has component: BulletTag but not BulletGroup");
                    // Debug.Log($"Entity has component: BulletGroup");
                } else {
                    Debug.Log($"BulletGroup has component: BulletTag");
                }
            }

            // 获取所有组件类型
            var componentTypes = entityManager.GetComponentTypes(entity);
            foreach (var componentType in componentTypes)
            {
                Debug.Log($"Entity has component: {componentType}");
            }
        }
    }

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ArmorCollisionEventSystem : ISystem
    {
        private ComponentLookup<RobotGhostTag> robotGhostGroup;
        private ComponentLookup<BulletTag> bulletGroup;

        public void OnCreate(ref SystemState state)
        {
            robotGhostGroup = state.GetComponentLookup<RobotGhostTag>(true);
            bulletGroup = state.GetComponentLookup<BulletTag>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            // 更新 ComponentLookup
            robotGhostGroup.Update(ref state);
            bulletGroup.Update(ref state);
            // Debug.Log("ArmorCollisionEventSystem OnUpdate");
            var armor_detect_job = new ArmorCollisionEventJob
            {
                robotGhostGroup = robotGhostGroup,
                BulletGroup = bulletGroup,
            };
            
            state.Dependency = armor_detect_job.Schedule(
                SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
    }

}