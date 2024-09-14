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
    private void Awake() {
        _playerState = new NetworkVariable<PlayerNetworkState>();
        state_store = GetComponent<StateStore>();
    }

    void Start() {
        if (IsOwner) {
            if (!IsServer) {
                Observable.Interval(System.TimeSpan.FromSeconds(0.05))
                        .Subscribe(_ => {
                            TransmitStateServerRpc(state_store.get_struct());
                        }).AddTo(this);
            }
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

    #region Transmit State

    private void TransmitState() {
        if (!IsServer) return;
        var state = new PlayerNetworkState {
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
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

    private void ConsumeState() {
        // Here you'll find the cheapest, dirtiest interpolation you'll ever come across. Please do better in your game
        transform.position = Vector3.SmoothDamp(transform.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime);
        
        transform.rotation = Quaternion.Euler(
            0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _playerState.Value.Rotation.y, ref _rotVelY, _cheapInterpolationTime), 0);
    }
    #endregion
}


struct PlayerNetworkState : INetworkSerializable {
    private float _posX, _posY,_posZ;
    private float _rotY;
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

    // internal StateStore.store_struct StateStore {
    //     set => state_store_struct = value;
    // }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref _posX);
        serializer.SerializeValue(ref _posY);
        serializer.SerializeValue(ref _posZ);

        serializer.SerializeValue(ref _rotY);
        // serializer.SerializeValue(ref state_store_struct.state.vision_mode);
    }
}


}