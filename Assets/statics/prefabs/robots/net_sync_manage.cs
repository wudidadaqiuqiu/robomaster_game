using UnityEngine;
using Unity.Netcode;
using UniRx;

namespace Robots
{


public class net_transform : NetworkBehaviour
{
    [SerializeField] private float _cheapInterpolationTime = 0.01f;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private StateStore state_store;
    private RobotTransform robot_transform;

    private void Awake() {
        _playerState = new NetworkVariable<PlayerNetworkState>();
        state_store = GetComponent<StateStore>();
        robot_transform = GetComponent<RobotTransform>();
    }

    void Start() {
        if (IsOwner) {
            if (!IsServer) {
                Observable.Interval(System.TimeSpan.FromSeconds(0.05))
                        .Subscribe(_ => {
                            TransmitStateServerRpc(state_store.get_struct());
                        }).AddTo(this);

                Observable.Interval(System.TimeSpan.FromSeconds(0.5))
                        .Where(_ => state_store.config.has_init)
                        .First() // 执行一次
                        .Subscribe(_ => {
                            TransmitInGameConfigServerRpc(state_store.get_ingame_config());
                            // Debug.Log("TransmitInGameConfigServerRpc");
                        }).AddTo(this);
            }
        }

        if (!IsServer && !IsOwner) {
            Observable.Interval(System.TimeSpan.FromSeconds(0.5))
                .Where(_ => !state_store.config.has_init)
                .Subscribe(_ => {
                    RequestInGameConfigServerRpc();
                }).AddTo(this);
        }
    }
        private void Update() {
        if (!IsServer)
            ConsumeState();
        else
            TransmitState();
    }
    
    [ServerRpc]
    private void TransmitStateServerRpc(StateStore.store_struct state) {
        state_store.my_struct_change(ref state);
    }


    [ServerRpc]
    private void TransmitInGameConfigServerRpc(StateStore.ingame_config config) {
        state_store.my_ingame_coonfig_change(ref config);
    }

    // 非owner 客户端发出请求
    [ServerRpc(RequireOwnership = false)]
    private void RequestInGameConfigServerRpc() {
        if (state_store.config.has_init)
            TransmitInGameConfigClientRpc(state_store.get_ingame_config());
    }

    [ClientRpc]
    private void TransmitInGameConfigClientRpc(StateStore.ingame_config config) {
        state_store.my_ingame_coonfig_change(ref config);
        // Debug.Log(config.team_Info.camp);
    }

    #region Transmit State

    private void TransmitState() {
        if (!IsServer) return;
        var state = new PlayerNetworkState {
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles,
            Pitch = robot_transform.pitch,
        };

        _playerState.Value = state;
    }

    private void update_player_state(ref PlayerNetworkState state) {
        state.Position = transform.position;
        state.Rotation = transform.rotation.eulerAngles;
        // state_store.struct_change(ref state.state_store_struct);
    }
    #endregion

    #region Interpolate State

    private Vector3 _posVel;
    private float _rotVelY;
    private float _pitchVel;

    private void ConsumeState() {
        // Here you'll find the cheapest, dirtiest interpolation you'll ever come across. Please do better in your game
        transform.position = Vector3.SmoothDamp(transform.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime);
        
        transform.rotation = Quaternion.Euler(
            0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _playerState.Value.Rotation.y, ref _rotVelY, _cheapInterpolationTime), 0);
        // Debug.Log(_playerState.Value.pitch);

        Quaternion targetrotation = robot_transform.pitch.get_target_localrotation(_playerState.Value.pitch);
        robot_transform.pitch._transform.localRotation = Quaternion.Slerp(robot_transform.pitch._transform.localRotation, targetrotation, _cheapInterpolationTime);
        
        // robot_transform.pitch.Value = Mathf.SmoothDamp(robot_transform.pitch.Value, _playerState.Value.pitch, ref _pitchVel, _cheapInterpolationTime);
    }
    #endregion
}


struct PlayerNetworkState : INetworkSerializable {
    private float _posX, _posY,_posZ;
    private float _rotY;
    private float _pitch;
    // public StateStore.store_struct state_store_struct;


    internal Vector3 Position {
        get => new(_posX, _posY, _posZ);
        set {
            _posX = value.x;
            _posY = value.y;
            _posZ = value.z;
        }
    }

    internal Vector3 Rotation {
        get => new(0, _rotY, 0);
        set => _rotY = value.y;
    }

    internal transform_property Pitch {
        // get => new(0, _rotY, 0);
        set => _pitch = value.Value;
    }

    internal float pitch {
        get => _pitch;
    }

    
    // internal StateStore.store_struct StateStore {
    //     set => state_store_struct = value;
    // }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref _posX);
        serializer.SerializeValue(ref _posY);
        serializer.SerializeValue(ref _posZ);

        serializer.SerializeValue(ref _rotY);
        serializer.SerializeValue(ref _pitch);
        // serializer.SerializeValue(ref state_store_struct.state.vision_mode);
    }
}


}