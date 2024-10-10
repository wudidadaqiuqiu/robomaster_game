using UnityEngine;
using Unity.Netcode;
using InterfaceDef;
using UniRx;
using StructDef.Game;

namespace Robots {
    class light_manage : NetworkBehaviour, IRobotComponent {
        private Subject<object> _subject;
        private StateStore state_store;

        public void SetSubject(Subject<object> subject) {
            _subject = subject;
            
        }

        public void Awake() {
            state_store = GetComponent<StateStore>();
        }
        
        public void Start() {
            // if (IsServer) {
                Observable.Interval(System.TimeSpan.FromSeconds(0.04))
                .Where(_ => state_store.config.has_init)
                .First()
                .Subscribe(_ => {
                    var s = new LightData {
                        color = state_store.config.team == "red" ? LightColor.red : LightColor.blue,
                        state = LightState.on 
                    };
                    _subject.OnNext(s);
                    // Debug.Log(state_store.config);
                }).AddTo(this);
            // }
        }
    }
}