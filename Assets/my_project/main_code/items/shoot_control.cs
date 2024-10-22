

using UnityEngine;
using Unity.Entities;
using InterfaceDef;

using UniRx;
using StructDef.Game;

using StructDef.TeamInfo;
using RefereeRelated;
using UnityEngine.SocialPlatforms;
using Unity.Transforms;

namespace RoboticItems
{
    class ShootControl : MonoBehaviour, IRobotComponent
    {
        private Subject<object> _subject;
        IdentityId id;
        private Entity shooter_entity;
        private EntityManager entity_manager;
        public ShooterComponentData _data;

        public GameObject position;

        void Start()
        {
            entity_manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            shooter_entity = entity_manager.CreateEntity(typeof(ShooterComponentData));
            _data.type = BulletType.None;
            entity_manager.SetComponentData(shooter_entity, _data);
            position = transform.Find("position").gameObject;
        }

        public virtual void SetParams()
        {
            _data.type = BulletType.Small;
            _data.delta_time = 0.1f;
        }

        private void SetData()
        {
            
            _data.position = position.transform.position;
            _data.direction = position.transform.forward;
            // Debug.Log(transform.localPosition);
        }

        private bool IsPosDirChange() {
            if ((position.transform.position - _data.position).magnitude > 0.001 ||
                (position.transform.forward - _data.direction).magnitude > 0.001)
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
                BulletType last_type = _data.type;
                if (((InputSyncData)x).shoot_mode == RobotShootMode.Normal && RefereeAllowed())
                {
                    SetParams();
                    
                } else {
                    _data.type = BulletType.None;
                }
                
                if (last_type == BulletType.None && _data.type == BulletType.None) {
                    return;
                }
                if (last_type != BulletType.None && _data.type != BulletType.None && !IsPosDirChange()) {
                    return;
                }
                Debug.Log($"Shoot by: {id}");
                if (_data.type != BulletType.None) {
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