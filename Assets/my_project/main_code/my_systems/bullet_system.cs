using StructDef.Game;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Player;

namespace MySystems {
    public partial struct BulletSystem : ISystem
    {
        private void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton<BulletGlobalData>(out var bulletGlobalData)) {
                // 如果没有找到 BulletGlobalData，则不执行任何操作
                return;
            }
            // 获取当前时间
            float deltaTime = SystemAPI.Time.DeltaTime;

            // 获取所有带有 BulletComponentData 和 Translation 的实体
            var entityQuery = SystemAPI.QueryBuilder()
                .WithAll<BulletData>()
                .WithAll<LocalTransform>()
                .Build();
            
            foreach (var entity in entityQuery.ToEntityArray(AllocatorManager.Temp)) {
                var transform = state.EntityManager.GetComponentData<LocalTransform>(entity);
                var bulletData = state.EntityManager.GetComponentData<BulletData>(entity);

                transform.Position += deltaTime * bulletData.velocity;
                bulletData.remain_life_time -= deltaTime;

                if (bulletData.remain_life_time <= 0f)
                {
                    state.EntityManager.DestroyEntity(entity);
                    continue;
                }

                state.EntityManager.SetComponentData(entity, transform);
                state.EntityManager.SetComponentData(entity, bulletData);
            }
        }
    }

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
    }


[BurstCompile]
public partial struct BulletShootingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<BulletGlobalData>(out var bulletGlobalData)) {
            // 如果没有找到 BulletGlobalData，则不执行任何操作
            return;
        }
        
        float deltaTime = SystemAPI.Time.DeltaTime;

        // 创建一个 EntityCommandBuffer
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // 遍历所有 ShooterComponentData 实体，使用 ShooterAspect
        foreach (var (shooter, entity) in SystemAPI.Query<ShooterAspect>().WithEntityAccess())
        {
            // 判断是否可以发射子弹
            if (shooter.ShootIntervalProcess(deltaTime))
            {
                Entity bulletPrefab = shooter.ShooterData.ValueRO.type == BulletType.Small
                    ? bulletGlobalData.small_bullet
                    : bulletGlobalData.big_bullet;
                float scale = shooter.ShooterData.ValueRO.type == BulletType.Small
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
                        velocity = shooter.ShooterData.ValueRO.direction * shooter.ShooterData.ValueRO.speed,
                    });

                    // 设置子弹的初始位置和方向
                    ecb.SetComponent(bulletInstance, new LocalTransform
                    {
                        Position = shooter.ShooterData.ValueRO.position,
                        Scale = scale,
                    });

                    // Debug.Log("Bullet fired!");
                }
            }
        }

        // 播放缓冲区命令
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
}