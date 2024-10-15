

using UnityEngine;

using InterfaceDef;

using UniRx;
using StructDef.Game;
using Unity.Entities;

namespace RoboticItems
{
    class ShootControl : MonoBehaviour, IRobotComponent
    {
        private Subject<object> _subject;
        [SerializeField] private Entity shooter_entity;
        private EntityManager entity_manager;
        private ShooterComponentData _data;

        void Start()
        {
            entity_manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            shooter_entity = entity_manager.CreateEntity(typeof(ShooterComponentData));
            _data.shoot_delta_time = -1;
            entity_manager.SetComponentData(shooter_entity, _data);
        }

        public virtual void SetParams()
        {
            _data.speed = ProjectSettings.Params.bullet17mm_speed;
            _data.shoot_delta_time = ProjectSettings.Params.bullet17mm_delta_time;
        }

        private void SetData()
        {
            _data.position = transform.position;
            _data.direction = -transform.forward;
        }

        public void SetSubject(Subject<object> subject)
        {
            _subject = subject;

            _subject.Where(x => x is InputNetworkData)
            .Subscribe(x =>
            {
                if (((InputNetworkData)x).GetMouseButtonBits(mouse_button_order.Left) && RefereeAllowed())
                {
                    SetParams();
                    SetData();
                } else {
                    _data.shoot_delta_time = -1;
                }

                if (entity_manager != null && shooter_entity != Entity.Null) {
                    entity_manager.SetComponentData(shooter_entity, _data);
                } else {
                    Debug.Log("entity_manager or shooter_entity is null");
                }

            }).AddTo(this);
        }

        private bool RefereeAllowed()
        {
            return true;
        }
    }
}