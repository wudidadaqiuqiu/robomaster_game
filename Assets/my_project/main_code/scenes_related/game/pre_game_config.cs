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
    

public class PreGameConfiger : MonoBehaviour
{
    [SerializeField] private Button hostbutton;
    [SerializeField] private Button clientbutton;
    public Camera pregamecamera;
    public Camera ingamecamera;
    public GameObject net_start_ui;
    public ProjectSettings.InGameConfig config;

    void Start()
    {
        // Debug.Log("buttons start");
        pregamecamera.enabled = true;
        net_start_ui.SetActive(true);
        // pregamecanvas.enabled = true;

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

    void DisableAfterNetStart() {
        net_start_ui.SetActive(false);
        // pregamecanvas.enabled = false;
        pregamecamera.enabled = false;
        pregamecamera.GetComponent<AudioListener>().enabled = false;

        ingamecamera.enabled = true;
        ingamecamera.GetComponent<AudioListener>().enabled = true;
    }
    public void OnHostButtonClick()
    {
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "yaml", false)[0];
        ClientChangeConfig(path);

        Debug.Log("Starting Host...");
        NetworkManager.Singleton.StartHost();
        DisableAfterNetStart();
    }

    public void OnClientButtonClick()
    {
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "yaml", false)[0];
        ClientChangeConfig(path);

        Debug.Log("Starting Client...");
        NetworkManager.Singleton.StartClient();
        DisableAfterNetStart();
    }

    void ClientChangeConfig(string path) {
        string content = File.ReadAllText(path);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();
        config = deserializer.Deserialize<ProjectSettings.InGameConfig>(content);
        
        Player.PlayerConfigManager.Singleton.LoadConfig(config);

        Debug.Log(config.ip + config.port);
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = config.ip;
        transport.ConnectionData.Port = config.port;
    }
}

}