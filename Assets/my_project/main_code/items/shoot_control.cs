

using UnityEngine;

using InterfaceDef;

using UniRx;
using StructDef.Game;
using Unity.Entities;
using StructDef.TeamInfo;
using RefereeRelated;

namespace RoboticItems
{
    class ShootControl : MonoBehaviour, IRobotComponent
    {
        private Subject<object> _subject;
        IdentityId id;
        [SerializeField] private Entity shooter_entity;
        private EntityManager entity_manager;
        private ShooterComponentData _data;

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
        }

        private void SetData()
        {
            _data.position = transform.position;
            _data.direction = -transform.forward;
        }

        public void SetSubject(Subject<object> subject)
        {
            _subject = subject;
            _subject.Where(x => x is IdentityId)
            .Subscribe(x => id = (IdentityId)x)
            .AddTo(this);

            _subject.Where(x => x is InputNetworkData)
            .Subscribe(x =>
            {
                if (((InputNetworkData)x).GetMouseButtonBits(mouse_button_order.Left) && RefereeAllowed())
                {
                    SetParams();
                    SetData();
                } else {
                    _data.type = ShooterType.None;
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