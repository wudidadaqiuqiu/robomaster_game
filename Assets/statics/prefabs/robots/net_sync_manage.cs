using UnityEngine;
using Unity.Netcode;

namespace Robots
{


public class net_transform : NetworkBehaviour
{
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private void Awake() {
        _playerState = new NetworkVariable<PlayerNetworkState>();
    }

        private void Update() {
        if (!IsServer)
            ConsumeState();
        else
            TransmitState();
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
    private float _posX, _posZ;
    private short _rotY;

    internal Vector3 Position {
        get => new(_posX, 0, _posZ);
        set {
            _posX = value.x;
            _posZ = value.z;
        }
    }

    internal Vector3 Rotation {
        get => new(0, _rotY, 0);
        set => _rotY = (short)value.y;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref _posX);
        serializer.SerializeValue(ref _posZ);

        serializer.SerializeValue(ref _rotY);
    }
}


}