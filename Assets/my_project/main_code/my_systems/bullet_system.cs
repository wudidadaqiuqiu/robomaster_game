using StructDef.Game;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

namespace MySystems {
    public partial struct BulletSystem : ISystem
    {
        private void OnUpdate(ref SystemState state)
        {
            // 获取当前时间
            float deltaTime = SystemAPI.Time.DeltaTime;

                    // 获取所有带有 BulletComponentData 和 Translation 的实体
            var entityQuery = SystemAPI.QueryBuilder()
                .WithAll<BulletComponentData>()
                .WithAll<LocalTransform>()
                .Build();
            
            foreach (var entity in entityQuery.ToEntityArray(AllocatorManager.Temp)) {
                var transform = state.EntityManager.GetComponentData<LocalTransform>(entity);
                var bulletData = state.EntityManager.GetComponentData<BulletComponentData>(entity);

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
        public bool CanShoot(float deltaTime)
        {
            ShooterData.ValueRW.delta_time -= deltaTime;
            if (ShooterData.ValueRO.delta_time <= 0 && ShooterData.ValueRO.type != BulletType.None)
            {
                ShooterData.ValueRW.delta_time = 1.0f; // 重置发射间隔
                return true;
            }
            return false;
        }
    }


[BurstCompile]
public partial struct BulletShooterSystem : ISystem
{
    // public void OnCreate(ref SystemState state) { }

    // public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 获取 deltaTime
        float deltaTime = SystemAPI.Time.DeltaTime;

        // 获取 BulletEntityData (Singleton)
        var bulletEntityData = SystemAPI.GetSingleton<BulletEntityData>();

        // 创建一个 EntityCommandBuffer
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // 遍历所有 ShooterComponentData 实体，使用 ShooterAspect
        foreach (var (shooter, entity) in SystemAPI.Query<ShooterAspect>().WithEntityAccess())
        {
            // 判断是否可以发射子弹
            if (shooter.CanShoot(deltaTime))
            {
                Entity bulletPrefab = shooter.ShooterData.ValueRO.type == BulletType.Small
                    ? bulletEntityData.small_bullet
                    : bulletEntityData.big_bullet;
                float scale = shooter.ShooterData.ValueRO.type == BulletType.Small
                    ? bulletEntityData.small_scale
                    : bulletEntityData.big_scale;
                
                if (bulletPrefab != Entity.Null)
                {
                    // 实例化子弹
                    Entity bulletInstance = ecb.Instantiate(bulletPrefab);
                    
                    ecb.SetComponent(bulletInstance, new BulletComponentData
                    {
                        remain_life_time = 5,
                        velocity = shooter.ShooterData.ValueRO.direction * 30,
                    });

                    // Debug.Log((shooter.ShooterData.ValueRO.direction * 30).magnitude);
                    // 设置子弹的初始位置和方向
                    ecb.SetComponent(bulletInstance, new LocalTransform
                    {
                        Position = shooter.ShooterData.ValueRO.position,
                        // Rotation = quaternion.LookRotationSafe(shooter.ShooterData.ValueRO.direction, math.up()),
                        Scale = scale,
                    });

                    Debug.Log("Bullet fired!");
                }
            }
        }

        // 播放缓冲区命令
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
}