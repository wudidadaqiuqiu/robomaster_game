using UnityEngine;
using Unity.Netcode;
using UniRx;
using InterfaceDef;
using StructDef.Game;
using Unity.VisualScripting;

namespace Robots
{


public class NetTransform : NetworkBehaviour, IRobotComponent
{
    private Subject<object> _subject;
    [SerializeField] private float _cheapInterpolationTime = 0.01f;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private StateStore state_store;
    private RobotTransform robot_transform;

    private InputSyncData last_sync_data;

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
                            TransmitStateServerRpc(state_store.GetStruct());
                        }).AddTo(this);

                Observable.Interval(System.TimeSpan.FromSeconds(0.5))
                        .Where(_ => state_store.config.has_init)
                        .First() // 执行一次
                        .Subscribe(_ => {
                            TransmitInGameConfigServerRpc(state_store.GetIngameConfig());
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

        if (IsOwner && !IsServer) {
            // Debug.Log("subscribing to subject");
            _subject.Where(x => x is InputNetworkData)
            .Subscribe(x => {
                // Debug.Log("sending input");
                TransmitInputServerRpc((InputNetworkData)x);
            }).AddTo(this);
        }

        if (IsServer) {
            _subject.Where(x => x is InputNetworkData)
            .Subscribe(x => {
                if (!((InputNetworkData)x).ConvertToSyncData().Equal(ref last_sync_data)) {
                    last_sync_data = ((InputNetworkData)x).ConvertToSyncData();
                    TransmitInputSyncClientRpc(last_sync_data);
                }
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
    private void TransmitStateServerRpc(StateStore.StoreStruct state) {
        state_store.ChangeMyStruct(ref state);
    }


    [ServerRpc]
    private void TransmitInGameConfigServerRpc(StateStore.IngameConfig config) {
        state_store.ChangeMyIngameConfig(ref config);
    }

    // 非owner 客户端发出请求
    [ServerRpc(RequireOwnership = false)]
    private void RequestInGameConfigServerRpc() {
        if (state_store.config.has_init)
            TransmitInGameConfigClientRpc(state_store.GetIngameConfig());
    }

    [ServerRpc]
    private void TransmitInputServerRpc(InputNetworkData input) {
        _subject.OnNext(input);
        // Debug.Log("TransmitInputServerRpc");
        // TransmitInputSyncClientRpc(input.ConvertToSyncData());
    }
    // 会向所有客户端发出
    [ClientRpc]
    private void TransmitInGameConfigClientRpc(StateStore.IngameConfig config) {
        state_store.ChangeMyIngameConfig(ref config);
        // Debug.Log(config.team_Info.camp);
    }

    [ClientRpc]
    private void TransmitInputSyncClientRpc(InputSyncData data) {
        // Debug.Log("TransmitInputSyncClientRpc");
        _subject.OnNext(data);
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

        Quaternion targetrotation = robot_transform.pitch.GetTargetLocalRotation(_playerState.Value.pitch);
        robot_transform.pitch._transform.localRotation = Quaternion.Slerp(robot_transform.pitch._transform.localRotation, targetrotation, _cheapInterpolationTime);
        
        // robot_transform.pitch.Value = Mathf.SmoothDamp(robot_transform.pitch.Value, _playerState.Value.pitch, ref _pitchVel, _cheapInterpolationTime);
    }

    public void SetSubject(Subject<object> subject)
    {
        _subject = subject;
        // Debug.Log("set subject");
        // 双false 未初始化,别用
        // Debug.Log($"IsOwner: {IsOwner}, IsServer: {IsServer}");
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

    internal TransformProperty Pitch {
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