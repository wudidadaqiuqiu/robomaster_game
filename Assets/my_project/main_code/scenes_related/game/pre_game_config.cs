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
using Cinemachine;

public class PreGameManager : MonoBehaviour
{
    public static PreGameManager Instance;

    [SerializeField] private Button hostbutton;
    [SerializeField] private Button clientbutton;
    public Camera main_camera;
    public Transform camera_pos;
    public GameObject net_start_ui;
    public ProjectSettings.InGameConfig config;

    private bool isStart = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //net_start_ui.SetActive(true);
        main_camera.GetComponent<Transform>().transform.position = camera_pos.position;
        main_camera.GetComponent<Transform>().rotation = camera_pos.rotation;

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

    void DisableAfterNetStart() 
    {
        net_start_ui.SetActive(false);
    }

    #region button_callback
    public void OnHostButtonClick()
    {
        //var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "yaml", false)[0];
        var path = "Assets/player_net_config/host.yaml";
        ClientChangeConfig(path);

        Debug.Log("Starting Host...");
        NetworkManager.Singleton.StartHost();

        isStart = true;
        DisableAfterNetStart();
    }

    public void OnClientButtonClick()
    {
        //var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "yaml", false)[0];
        var path = "Assets/player_net_config/client1.yaml";
        ClientChangeConfig(path);

        Debug.Log("Starting Client...");
        NetworkManager.Singleton.StartClient();

        isStart = true;
        DisableAfterNetStart();
    }
    #endregion

    void ClientChangeConfig(string path) 
    {
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

    public bool getSartStatus()
    {
        return isStart;
    }
}