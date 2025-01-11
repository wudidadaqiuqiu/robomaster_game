using StructDef.Game;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Player;
using System.Reflection.Emit;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;


namespace MySystems {
    public readonly partial struct BulletAspect : IAspect
    {
        public readonly Entity entity;
        public readonly RefRW<BulletData> bullet_data;
        // public readonly RefRO<LocalTransform> local_transform;

        public void ProcessBulllet(EntityCommandBuffer ecb, float deltaTime) {
            // local_transform.ValueRW.Position += deltaTime * bullet_data.ValueRO.velocity;
            bullet_data.ValueRW.remain_life_time -= deltaTime;

            if (bullet_data.ValueRW.remain_life_time <= 0f)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }

    [BurstCompile]
    public partial struct ProcessBulletJob : IJobEntity
    {
        public EntityCommandBuffer ecb;
        public float deltaTime;
        
        [BurstCompile]
        public void Execute(BulletAspect aspect)
        {
            aspect.ProcessBulllet(ecb, deltaTime);
        }
    }

    [BurstCompile]
    public partial struct ProcessBulletSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton<BulletGlobalData>(out var bulletGlobalData)) {
                // 如果没有找到 BulletGlobalData，则不执行任何操作
                return;
            }

            // Get the SystemState of the physics system using the SystemHandle
            // var physicsSystemState = state.World.Unmanaged.GetExistingSystemState<ExportPhysicsWorld>();

            // 获取 ExportPhysicsWorld 系统中的 JobHandle
            // JobHandle checkDynamicBodyIntegrityHandle = physicsSystemState.Dependency;

            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            // 不同job不能同时访问LocalTransform 
            new ProcessBulletJob { 
                ecb = ecb, 
                deltaTime = SystemAPI.Time.DeltaTime,
            }.Run();
            
            // state.Dependency = JobHandle.CombineDependencies(processBulletJobHandle, state.Dependency);
            // processBulletJobHandle.Complete();
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            
        }
    }


    public readonly partial struct ShooterAspect : IAspect
    {
        // 组件数据
        public readonly RefRW<ShooterCmdData> ShooterData;
        public readonly RefRW<ShooterSystemData> ShooterProcessingData;

        // 用于检测子弹发射的逻辑
        public bool ShootIntervalProcess(float deltaTime)
        {
            ShooterProcessingData.ValueRW.delta_time -= deltaTime;
            // 在同一帧内对数据的访问要保证一致性，所以需要使用RW
            if (ShooterProcessingData.ValueRW.delta_time <= 0 && ShooterData.ValueRO.type != BulletType.None)
            {
                ShooterProcessingData.ValueRW.delta_time = ProjectSettings.GameConfig.shoot_delay_time; // 重置发射间隔
                // Debug.Log("发射子弹");
                return true;
            }
            return false;
        }

        public void ShootBullet(EntityCommandBuffer ecb, float deltaTime, ref BulletGlobalData bulletGlobalData) {
            if (ShootIntervalProcess(deltaTime))
            {
                Entity bulletPrefab = ShooterData.ValueRO.type == BulletType.Small
                    ? bulletGlobalData.small_bullet
                    : bulletGlobalData.big_bullet;
                float scale = ShooterData.ValueRO.type == BulletType.Small
                    ? bulletGlobalData.small_scale
                    : bulletGlobalData.big_scale;
                
                if (bulletPrefab != Entity.Null)
                {
                    // 实例化子弹
                    Entity bulletInstance = ecb.Instantiate(bulletPrefab);
                    
                    ecb.SetComponent(bulletInstance, new BulletData
                    {
                        // 不能使用Singleton 运行时改变，只能是编译时设置
                        remain_life_time = ProjectSettings.GameConfig.bullet_life_time,
                    });

                    ecb.SetComponent(bulletInstance, new PhysicsVelocity 
                    {
                        Linear = ShooterData.ValueRO.velocity,
                        Angular = float3.zero,
                    });

                    // 设置子弹的初始位置和方向
                    ecb.SetComponent(bulletInstance, new LocalTransform
                    {
                        Position = ShooterData.ValueRO.position,
                        Scale = scale, // 设置为一个有效的比例
                        Rotation = quaternion.identity,

                    });
                    // Debug.Log("Bullet fired!");
                    // Debug.Log("Bullet fired!");
                }
            }
        }
    }

    [BurstCompile]
    public partial struct ShootBulletJob : IJobEntity
    {
        public EntityCommandBuffer ecb;
        public float deltaTime;
        public BulletGlobalData bulletGlobalData;
        
        [BurstCompile]
        public void Execute(ShooterAspect aspect)
        {
            aspect.ShootBullet(ecb, deltaTime, ref bulletGlobalData);
        }
    }
    
    [BurstCompile]
    public partial struct ShootBulletSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton<BulletGlobalData>(out var bulletGlobalData)) {
                // 如果没有找到 BulletGlobalData，则不执行任何操作
                return;
            }
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            new ShootBulletJob { 
                ecb = ecb, 
                deltaTime = SystemAPI.Time.DeltaTime, 
                bulletGlobalData = bulletGlobalData 
            }.Run();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}