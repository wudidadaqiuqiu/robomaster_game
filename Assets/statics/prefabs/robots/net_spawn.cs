using UnityEngine;
using Unity.Netcode;

namespace Robots
{

    public class net_spawn : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                transform.position = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
            }
        }
    }
}