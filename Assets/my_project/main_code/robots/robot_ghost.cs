using StructDef.Game;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Robots {
    public class RobotGhost  : MonoBehaviour {
        public RobotType robotType;
        private EntityManager entityManager;
        public RobotEntityGlobalData robot_global_data;

        private Entity robot;

        void Start() {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (entityManager.CreateEntityQuery(typeof(RobotEntityGlobalData)).TryGetSingleton<RobotEntityGlobalData>(out robot_global_data))
                robot = entityManager.Instantiate(robot_global_data.infantry1_entity);
            else {
                robot = Entity.Null;
            }
        }

        void Update() {
            if (robot != Entity.Null) {
                entityManager.SetComponentData(robot, new LocalTransform {
                    Position = transform.position,
                    Rotation = transform.rotation,
                    Scale = 1,
                });
            }
        }

        void OnDestroy() {
            if (robot != Entity.Null) {
                entityManager.DestroyEntity(robot);
            }
        }
    }
}