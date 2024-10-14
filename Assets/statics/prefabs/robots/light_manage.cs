using UnityEngine;
using Unity.Netcode;
using InterfaceDef;
using UniRx;
using StructDef.Game;

namespace Robots {
    class LightManage : NetworkBehaviour, IRobotComponent {
        private Subject<object> _subject;
        private StateStore state_store;

        public void SetSubject(Subject<object> subject) {
            _subject = subject;
            
        }

        public void Awake() {
            state_store = GetComponent<StateStore>();
        }
        
        public void Start() {
            // 执行一次
            Observable.Interval(System.TimeSpan.FromSeconds(0.5))
            .Where(_ => state_store.config.has_init)
            .First()
            .Subscribe(_ => {
                var s = new LightData {
                    color = state_store.config.team == "red" ? LightColor.red : LightColor.blue,
                    state = LightState.bright
                };
                _subject.OnNext(s);
                // Debug.Log(state_store.config);
            }).AddTo(this);
        }
    }
}