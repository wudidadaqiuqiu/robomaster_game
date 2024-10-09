using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using SFB;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Unity.Netcode.Transports.UTP;

namespace Game {
    

public class pre_game_config : MonoBehaviour
{
    [SerializeField] private Button hostbutton;
    [SerializeField] private Button clientbutton;
    public Camera pregamecamera;
    public Camera ingamecamera;
    public Canvas pregamecanvas;
    private ProjectSettings.ClientConfig clientConfig;

    void Start()
    {
        // Debug.Log("buttons start");
        pregamecamera.enabled = true;
        pregamecanvas.enabled = true;

        ingamecamera.enabled = false;
        ingamecamera.GetComponent<AudioListener>().enabled = false;

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
        
        ingamecamera.enabled = true;
        ingamecamera.GetComponent<AudioListener>().enabled = true;
    }

    public void OnClientButtonClick()
    {
        // 在这里添加你想要执行的逻辑
        Debug.Log("Starting Client...");
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "yaml", false)[0];
        client_change_config(path);

        NetworkManager.Singleton.StartClient();
        pregamecanvas.enabled = false;
        pregamecamera.enabled = false;
        pregamecamera.GetComponent<AudioListener>().enabled = false;

        ingamecamera.enabled = true;
        ingamecamera.GetComponent<AudioListener>().enabled = true;
        // ingamecamera.enabled = true;
    }

    void client_change_config(string path) {
        string content = File.ReadAllText(path);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();
        clientConfig = deserializer.Deserialize<ProjectSettings.ClientConfig>(content);
        Debug.Log(clientConfig.ip + clientConfig.port);
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = clientConfig.ip;
        transport.ConnectionData.Port = clientConfig.port;
    }
}

}