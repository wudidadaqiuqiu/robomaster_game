// using System;
using InterfaceDef;
using UnityEngine;
using UniRx;
using System;

namespace Robots {

    public class robot_transform : MonoBehaviour, IRobotComponent {
        private Subject<object> RobotSubject;
        public transform_property robot_x;
        public transform_property robot_y;

        public transform_property yaw;
        public transform_property pitch;
        public transform_property chassis_yaw;
        
        private Robots.InputNetworkStruct input;
        public void SetSubject(Subject<object> subject) {
            RobotSubject = subject;
        }

        void Awake() {
        }
        void Start() {
            robot_x.Init();
            robot_y.Init();
            yaw.Init();
            pitch.Init();
            chassis_yaw.Init();
            
            RobotSubject.Where(x => x is Robots.InputNetworkStruct)
                        .Subscribe(x => input_process((Robots.InputNetworkStruct)x)).AddTo(this);
        }

        void Update() {
            if (input == null) return;
            if (input.keyboard_bits_get(keyboard_bits_order.W)) {
                robot_x.Value += Time.deltaTime * 5;
            }
            if (input.keyboard_bits_get(keyboard_bits_order.S))
                robot_x.Value -= Time.deltaTime * 5;
            if (input.keyboard_bits_get(keyboard_bits_order.A))
                robot_y.Value += Time.deltaTime * 5;
            if (input.keyboard_bits_get(keyboard_bits_order.D))
                robot_y.Value -= Time.deltaTime * 5;
        }
        void input_process(Robots.InputNetworkStruct _input) {
            // if (ProjectSettings.GameConfig.unity_debug)
            //     Debug.Log("input process sub");
            input = _input;
        }
    }

    [System.Serializable]
    public class transform_property {
        [SerializeField] private Transform _transform;
        [SerializeField] private string property_name;
        [SerializeField] private bool is_reverse = false;

        private object posrot;
        private System.Reflection.PropertyInfo property;
        private System.Reflection.FieldInfo field_info;
        
        
        public void Init() {
            property = _transform.GetType()
                        .GetProperty(property_name.Split('.')[0]);
            posrot = property
                        .GetValue(_transform, null);
            field_info = posrot.GetType()
                        .GetField(property_name.Split('.')[1]);
            Debug.Assert(field_info != null, "field is null, " + property_name);
        }
        public float Value {
            get {
                Debug.Assert(field_info != null);
                if (field_info == null) return 0;
                // Debug.LogFormat("get: field is not null, {0}", property_name);
                return (float)field_info.GetValue(property.GetValue(_transform, null)) * (is_reverse ? -1 : 1);
            }
            set {
                Debug.Assert(field_info != null);
                if (field_info == null) return;
                // Debug.LogFormat("set: field is not null, {0}", property_name);
                posrot = property.GetValue(_transform, null);
                field_info.SetValue(posrot, value * (is_reverse ? -1 : 1));
                // Debug.Log((Vector3)posrot);
                property.SetValue(_transform, posrot, null);
            }
        }

    }

}