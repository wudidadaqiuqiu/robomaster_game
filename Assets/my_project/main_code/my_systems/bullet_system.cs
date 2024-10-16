using StructDef.Game;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

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

    // public partial struct BulletGenerateSystem  : ISystem
    // {
    //     private void OnUpdate(ref SystemState state)
    //     {

    //     }
    // }
}