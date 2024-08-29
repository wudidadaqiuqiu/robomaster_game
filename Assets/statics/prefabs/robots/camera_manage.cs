using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Robots {
public class camera_manage : NetworkBehaviour
{
    [SerializeField] private GameObject camera_;

    public override void OnNetworkSpawn()
    {
        if (camera_ != null)
        {
            // Debug.Log("Player camera found: " + fpcamera.name);
            if (IsOwner) 
            {
                camera_.SetActive(true);
            }
            else
            {
                camera_.SetActive(false);
            }
        }
    }
}
}