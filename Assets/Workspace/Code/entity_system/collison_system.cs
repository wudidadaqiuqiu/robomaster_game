using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using StructDef.Game;
using RoboticItems;

namespace MySystems
{
    [BurstCompile]
    public struct ArmorCollisionEventJob : ICollisionEventsJob
    {
        public ComponentLookup<RobotGhostComponent> robotGhostGroup;
        [ReadOnly] public ComponentLookup<BulletTag> BulletGroup;
        [BurstCompile]
        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;
            
            if (robotGhostGroup.HasComponent(entityA) && BulletGroup.HasComponent(entityB))
            {
                var robot_ghost = robotGhostGroup[entityA];
                robot_ghost.collision_cnt++;
                robotGhostGroup[entityA] = robot_ghost;
                // Debug.Log("Armor collided with bullet");
            }

            if (robotGhostGroup.HasComponent(entityB) && BulletGroup.HasComponent(entityA))
            {
                var robot_ghost = robotGhostGroup[entityB];
                robot_ghost.collision_cnt++;
                robotGhostGroup[entityB] = robot_ghost;
                // Debug.Log("Armor collided with bullet");
            }
        }
    }

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    public partial struct ArmorCollisionEventSystem : ISystem
    {
        private ComponentLookup<RobotGhostComponent> robotGhostGroup;
        private ComponentLookup<BulletTag> bulletGroup;

        public void OnCreate(ref SystemState state)
        {
            robotGhostGroup = state.GetComponentLookup<RobotGhostComponent>();
            bulletGroup = state.GetComponentLookup<BulletTag>(true);
        }
        [BurstCompile]
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