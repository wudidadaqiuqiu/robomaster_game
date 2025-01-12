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
                //GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(ProjectSettings.GameConfig.game_config_tag);
                //Debug.Assert(gameObjects.Length == 1);
                state_store.config = PreGameManager.Instance.config;
                // Debug.Log("config to state store success");
            }

            if (!IsOwner) Destroy(GetComponent<InputManager>());
        }
    }
}