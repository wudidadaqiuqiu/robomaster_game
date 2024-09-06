// using System;
using InterfaceDef;
using UnityEngine;
using UniRx;
using System;

namespace Robots {

    public class robot_transform : MonoBehaviour, IRobotComponent {
        private Subject<object> RobotSubject;
        private CharacterController characterController;
        private GameObject main_camera;
        private float smooth_velocity;
        private ground_check groundCheck;
        private Vector3 velo;  
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
            characterController = GetComponent<CharacterController>();
            groundCheck = GetComponentInChildren<ground_check>();
            velo = Vector3.zero;
            main_camera = GameObject.FindWithTag("MainCamera");
        }
        void Start() {
            robot_x.Init();
            robot_y.Init();
            yaw.Init();
            pitch.Init();
            chassis_yaw.Init();

            // server inner subscribe
            RobotSubject.Where(x => x is Robots.InputNetworkStruct)
                        .Subscribe(x => input_process((Robots.InputNetworkStruct)x)).AddTo(this);
        }

        void Update() {
            if (input == null) return;
            if (groundCheck == null) {
                Debug.Log("ground check is null");
                return;
            }
            Vector3 direction = Vector3.zero;
            if (input.keyboard_bits_get(keyboard_bits_order.W)) {
                // characterController.Move(robot_x.dirction() * Time.deltaTime * 5);
                direction += robot_x.dirction();
            }
            if (input.keyboard_bits_get(keyboard_bits_order.S)) {
                // characterController.Move(-robot_x.dirction() * Time.deltaTime * 5);
                direction -= robot_x.dirction();
            }
            if (input.keyboard_bits_get(keyboard_bits_order.A)) {
                // characterController.Move(robot_y.dirction() * Time.deltaTime * 5);
                direction += robot_y.dirction();
            }
            if (input.keyboard_bits_get(keyboard_bits_order.D)) {
                // characterController.Move(-robot_y.dirction() * Time.deltaTime * 5);
                direction -= robot_y.dirction();
            }
            direction.Normalize();
            if (direction.magnitude > 0.1f) {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + main_camera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smooth_velocity, 0.1f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 move_dir = Quaternion.Euler(0f, angle, 0f) * robot_x.dirction();
                characterController.Move(move_dir * Time.deltaTime * 5);
            }
            // Debug.Log(input.mouse_x);
            // yaw.Value -= input.mouse_x * Time.deltaTime * ProjectSettings.Params.mouse_sensitivity_hor;


            velo.y -= Time.deltaTime * 9.8f;
            if (groundCheck.IsGrounded() && velo.y < 0) {
                velo.y = 0;
            }
            characterController.Move(velo * Time.deltaTime);
            
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
        private Vector3 direction_;
        
        public void Init() {
            direction_ = Vector3.zero;
            property = _transform.GetType()
                        .GetProperty(property_name.Split('.')[0]);
            posrot = property
                        .GetValue(_transform, null);
            field_info = posrot.GetType()
                        .GetField(property_name.Split('.')[1]);
            Debug.Assert(field_info != null, "field is null, " + property_name);
            if (property_name.Split('.')[0] == "position") {
                if (property_name.Split('.')[1] == "x") {
                    direction_.x = is_reverse ? -1 : 1;
                } else if (property_name.Split('.')[1] == "y") {
                    direction_.y = is_reverse ? -1 : 1;
                } else if (property_name.Split('.')[1] == "z") {
                    direction_.z = is_reverse ? -1 : 1;
                }
                // Debug.Log(direction_);
            }
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

        public ref Vector3 dirction() {
            return ref direction_;
        }

    }

}