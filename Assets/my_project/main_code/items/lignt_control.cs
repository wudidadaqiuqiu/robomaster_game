using UniRx;
using System;
using UnityEngine;
using InterfaceDef;
using StructDef.Game;

namespace RoboticItems {

    public class LigntControl : MonoBehaviour, IRobotComponent {
        private Subject<object> _subject;

        Material light_material;
        Color normal_color;
        bool twink_temp;
        private IDisposable in_twink;

        public void SetSubject(Subject<object> subject) {
            _subject = subject;
            _subject.Where(x => x is LightData).Subscribe(x => {
                on_light_state_change((LightData)x);
                // Debug.Log("light data subscribe");
            }).AddTo(this);
        }
        
        private void Awake() {
            light_material = GetComponent<Renderer>().material;
            normal_color = light_material.color;
            // Debug.LogFormat("normal color: {0}", normal_color);
        }

        private void Start() {
            light_material.EnableKeyword("_EMISSION");
        }

        private void OnDestroy() {
            stop_twink();
        }
        private void material_light(bool on) {
            light_material.SetColor("_EmissionColor", 
                on ? normal_color * 10.0f : normal_color * 0.0f);
        }

        private void on_light_state_change(LightData data) {
            if (data.state != LightState.twink) {
                stop_twink();
            }
            switch (data.state) {
                case LightState.bright:
                    material_light(true);
                    break;
                case LightState.off:
                    material_light(false);
                    break;
                case LightState.twink:
                    start_twink();
                    break;
                default:
                    break;
            }
        }

        private void stop_twink() {
            in_twink?.Dispose();
            in_twink = null;
        }

        private void start_twink() {
            stop_twink();
            twink_temp = true;
            in_twink = Observable
                .Interval(TimeSpan.FromSeconds(0.05f))
                .Take(2)
                .Subscribe(x => {
                    twink_temp = !twink_temp;
                    material_light(twink_temp);
                })
                .AddTo(this);
        }
    }
}