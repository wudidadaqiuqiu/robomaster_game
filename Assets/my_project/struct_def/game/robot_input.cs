using Unity.Netcode;
using System;
using UniRx;
using Unity.VisualScripting;

namespace StructDef.Game {
    public enum keyboard_bits_order : int {
        W = 0,
        S = 1,
        A = 2,
        D = 3,
        Shift = 4,
        Ctrl = 5,
        Q = 6,
        E = 7,
        R = 8,
        F = 9,
        G = 10,
        Z = 11,
        X = 12,
        C = 13,
        V = 14,
        B = 15
    }


    public enum mouse_button_order : int {
        Left = 0,
        Right = 1,
        Middle = 2,
        X1 = 3,
        X2 = 4
    }

    public class InputSyncData : INetworkSerializable
    {
        public RobotShootMode shoot_mode;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref shoot_mode);
        }
        // 无参构造函数
        public InputSyncData() 
        {
            shoot_mode = RobotShootMode.None; // 默认值
        }

        public InputSyncData(RobotShootMode shoot_mode) { 
            this.shoot_mode = shoot_mode;
        }
        
        public bool Equal(ref InputSyncData other) {
            if (other == null) {
                return false;
            }
            return shoot_mode == other.shoot_mode;
        }

    }
    // class 不是struct
    public class InputNetworkData : INetworkSerializable, IEquatable<InputNetworkData>
    {
        
        private ushort _keyboard_bits;
        public byte _mouse_button_bits;
        private float _mouse_x;
        private float _mouse_y;
        // private float _camera_rotate_y;

        internal float mouse_x {
            get => _mouse_x;
            set => _mouse_x = value;
        }

        internal float mouse_y {
            get => _mouse_y;
            set => _mouse_y = value;
        }
        // internal float camera_rotate_y {
        //     get => _camera_rotate_y;
        //     set => _camera_rotate_y = value;
        // }

        internal ushort keyboard_bits {
            get => _keyboard_bits;
            set => _keyboard_bits = value;
        }

        public void keyboard_bits_set(keyboard_bits_order bit, bool value) {
            if (value) {
                _keyboard_bits |= (ushort)(1 << (int)bit);
            }
            else {
                _keyboard_bits &= (ushort)~(1 << (int)bit);
            }
        }

        public void SetMouseButtonBits(mouse_button_order bit, bool value) {
            if (value) {
                _mouse_button_bits |= (byte)(1 << (int)bit);
            }
            else {
                _mouse_button_bits &= (byte)~(1 << (int)bit);
            }
        }

        public bool keyboard_bits_get(keyboard_bits_order bit) {
            return (_keyboard_bits & (ushort)(1 << (int)bit)) != 0;
        }

        public bool GetMouseButtonBits(mouse_button_order bit) {
            return (_mouse_button_bits & (byte)(1 << (int)bit)) != 0;
        }

        public bool Equals(InputNetworkData o) {
            return _keyboard_bits == o._keyboard_bits && _mouse_x == o.mouse_x && 
                        _mouse_y == o.mouse_y && _mouse_button_bits == o._mouse_button_bits;
        }

        public void assign(InputNetworkData o) {
            _keyboard_bits = o.keyboard_bits;
            _mouse_x = o.mouse_x;
            _mouse_y = o.mouse_y;
            _mouse_button_bits = o._mouse_button_bits;
        }

        // 重写 Equals 方法
        public override bool Equals(object obj)
        {
            if (obj is InputNetworkData)
            {
                // InputNetworkStruct p = (InputNetworkStruct)obj;
                // return _keyboard_bits == p._keyboard_bits && _mouse_x == p.mouse_x && 
                //         _mouse_y == p.mouse_y && _camera_rotate_y == p.camera_rotate_y;
                return Equals((InputNetworkData)obj);
            }
            return false;
        }

        // 重写 GetHashCode 方法
        public override int GetHashCode()
        {
            return (_keyboard_bits.ToString() + '_' + _mouse_x.ToString() + '_' +
                    _mouse_y.ToString() + '_' + _mouse_button_bits.ToString()).GetHashCode();
        }


        public static bool operator ==(InputNetworkData p1, InputNetworkData p2)
        {
            if (p1 is null && p2 is null) return true;
            if (p1 is null || p2 is null) return false;
            return p1.Equals(p2);
        }

        
        public static bool operator !=( InputNetworkData p1,  InputNetworkData p2)
        {
            if (p1 is null && p2 is null) return false;
            if (p1 is null || p2 is null) return true;
            return !p1.Equals(p2);
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _keyboard_bits);
            serializer.SerializeValue(ref _mouse_button_bits);
            serializer.SerializeValue(ref _mouse_x);
            serializer.SerializeValue(ref _mouse_y);
        }

        public InputSyncData ConvertToSyncData() {
            if (GetMouseButtonBits(mouse_button_order.Left))
                return new InputSyncData(RobotShootMode.Normal);
            else 
                return new InputSyncData(RobotShootMode.None);
        }
    }
}