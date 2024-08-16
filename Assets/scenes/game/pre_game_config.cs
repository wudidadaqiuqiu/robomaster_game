using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Game {
    

public class pre_game_config : MonoBehaviour
{
    [SerializeField] private Button hostbutton;
    [SerializeField] private Button clientbutton;
    public Camera pregamecamera;
    public Canvas pregamecanvas;

    void Start()
    {
        // Debug.Log("buttons start");
        pregamecamera.enabled = true;
        pregamecanvas.enabled = true;
        // ingamecamera.enabled = false;

        // 检查是否已分配按钮
        if (hostbutton != null)
        {
            // Debug.Log("按钮已分配");
            // 为按钮的 onClick 事件添加一个监听器
            hostbutton.onClick.AddListener(OnHostButtonClick);
        }

        if (clientbutton != null)
        {
            // Debug.Log("按钮已分配");
            // 为按钮的 onClick 事件添加一个监听器
            clientbutton.onClick.AddListener(OnClientButtonClick);
        }
    }

    public void OnHostButtonClick()
    {
        // 在这里添加你想要执行的逻辑
        Debug.Log("Starting Host...");
        NetworkManager.Singleton.StartHost();
        pregamecanvas.enabled = false;
        pregamecamera.enabled = false;
        pregamecamera.GetComponent<AudioListener>().enabled = false;
        // ingamecamera.enabled = true;
    }

    public void OnClientButtonClick()
    {
        // 在这里添加你想要执行的逻辑
        Debug.Log("Starting Client...");
        NetworkManager.Singleton.StartClient();
        pregamecanvas.enabled = false;
        pregamecamera.enabled = false;
        pregamecamera.GetComponent<AudioListener>().enabled = false;
        // ingamecamera.enabled = true;
    }
}

}