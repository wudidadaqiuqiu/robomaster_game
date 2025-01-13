using UnityEngine;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace Robots
{
    public class NetSpawn : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                transform.position = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
            }

            if (IsOwner)
            {
                var state_store = GetComponent<StateStore>();
                state_store.config = PreGameManager.Instance.config;
            }

            if (!IsOwner) Destroy(GetComponent<InputManager>());
        }
    }
}