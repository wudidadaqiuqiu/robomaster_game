

using UnityEngine;
using Unity.Entities;
using InterfaceDef;

using UniRx;
using StructDef.Game;

using StructDef.TeamInfo;
using RefereeRelated;

namespace RoboticItems
{
    class ShootControl : MonoBehaviour, IRobotComponent
    {
        private Subject<object> _subject;
        IdentityId id;
        private Entity shooter_entity;
        private EntityManager entity_manager;
        public ShooterComponentData _data;

        void Start()
        {
            entity_manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            shooter_entity = entity_manager.CreateEntity(typeof(ShooterComponentData));
            _data.type = ShooterType.None;
            entity_manager.SetComponentData(shooter_entity, _data);
        }

        public virtual void SetParams()
        {
            _data.type = ShooterType.Small;
            _data.delta_time = 0.1f;
        }

        private void SetData()
        {
            _data.position = transform.position;
            _data.direction = -transform.forward;
        }

        private bool IsPosDirChange() {
            if ((transform.position - _data.position).magnitude > 0.001 || (-transform.forward - _data.direction).magnitude > 0.001)
            {
                return true;
            }
            return false;
        }
        public void SetSubject(Subject<object> subject)
        {
            _subject = subject;
            _subject.Where(x => x is IdentityId)
            .Subscribe(x => id = (IdentityId)x)
            .AddTo(this);

            _subject.Where(x => x is InputSyncData)
            .Subscribe(x =>
            {
                // Debug.Log($"Input in {id}");
                ShooterType last_type = _data.type;
                if (((InputSyncData)x).shoot_mode == RobotShootMode.Normal && RefereeAllowed())
                {
                    SetParams();
                    
                } else {
                    _data.type = ShooterType.None;
                }
                
                if (last_type == ShooterType.None && _data.type == ShooterType.None) {
                    return;
                }
                if (last_type != ShooterType.None && _data.type != ShooterType.None && !IsPosDirChange()) {
                    return;
                }
                Debug.Log($"Shoot by: {id}");
                if (_data.type != ShooterType.None) {
                    SetData();
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
            return Referee.Singleton.AllowToShoot(id);
        }
    }
}