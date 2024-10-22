using StructDef.Game;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Player;
using System.Reflection.Emit;

namespace MySystems {
    public readonly partial struct BulletAspect : IAspect
    {
        public readonly Entity entity;
        public readonly RefRW<BulletData> bullet_data;
        public readonly RefRW<LocalTransform> local_transform;

        public void ProcessBulllet(EntityCommandBuffer ecb, float deltaTime) {
            local_transform.ValueRW.Position += deltaTime * bullet_data.ValueRO.velocity;
            bullet_data.ValueRW.remain_life_time -= deltaTime;

            if (bullet_data.ValueRO.remain_life_time <= 0f)
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
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            new ProcessBulletJob { 
                ecb = ecb, 
                deltaTime = SystemAPI.Time.DeltaTime,
            }.Run();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    // public partial struct BulletSystem : ISystem
    // {
    //     private void OnUpdate(ref SystemState state)
    //     {
    //         if (!SystemAPI.TryGetSingleton<BulletGlobalData>(out var bulletGlobalData)) {
    //             // 如果没有找到 BulletGlobalData，则不执行任何操作
    //             return;
    //         }
    //         // 获取当前时间
    //         float deltaTime = SystemAPI.Time.DeltaTime;

    //         // 获取所有带有 BulletComponentData 和 Translation 的实体
    //         var entityQuery = SystemAPI.QueryBuilder()
    //             .WithAll<BulletData>()
    //             .WithAll<LocalTransform>()
    //             .Build();
            
    //         foreach (var entity in entityQuery.ToEntityArray(AllocatorManager.Temp)) {
    //             var transform = state.EntityManager.GetComponentData<LocalTransform>(entity);
    //             var bulletData = state.EntityManager.GetComponentData<BulletData>(entity);

    //             transform.Position += deltaTime * bulletData.velocity;
    //             bulletData.remain_life_time -= deltaTime;

    //             if (bulletData.remain_life_time <= 0f)
    //             {
    //                 state.EntityManager.DestroyEntity(entity);
    //                 continue;
    //             }

    //             state.EntityManager.SetComponentData(entity, transform);
    //             state.EntityManager.SetComponentData(entity, bulletData);
    //         }
    //     }
    // }

    public readonly partial struct ShooterAspect : IAspect
    {
        // 组件数据
        public readonly RefRW<ShooterComponentData> ShooterData;

        // 用于检测子弹发射的逻辑
        public bool ShootIntervalProcess(float deltaTime)
        {
            ShooterData.ValueRW.delta_time -= deltaTime;
            if (ShooterData.ValueRO.delta_time <= 0 && ShooterData.ValueRO.type != BulletType.None)
            {
                ShooterData.ValueRW.delta_time = ProjectSettings.GameConfig.shoot_delay_time; // 重置发射间隔
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
                        velocity = ShooterData.ValueRO.direction * ShooterData.ValueRO.speed,
                    });

                    // 设置子弹的初始位置和方向
                    ecb.SetComponent(bulletInstance, new LocalTransform
                    {
                        Position = ShooterData.ValueRO.position,
                        Scale = scale,
                    });

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