using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class camera_manage : NetworkBehaviour
{
    [SerializeField] private Camera fpcamera;
    [SerializeField] private AudioListener fplistener;

    public override void OnNetworkSpawn()
    {
        if (fpcamera != null)
        {
            Debug.Log("Player camera found: " + fpcamera.name);
            if (IsOwner) 
            {
                fpcamera.enabled = true;
            }
            else
            {
                fpcamera.enabled = false;
            }
        }
        else
        {
            Debug.LogError("No camera found on the player!");
        }

        if (fplistener != null) 
        {
            Debug.Log("Player listener found: " + fplistener.name);
            if (IsOwner)
            {
                fplistener.enabled = true;
            }
            else
            {
                fplistener.enabled = false;
            }
        }
    }
}
