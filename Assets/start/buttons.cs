using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{
    [SerializeField] private Button start;


    void Start()
    {
        Debug.Log("buttons start");
        // 检查是否已分配按钮
        if (start != null)
        {
            // Debug.Log("按钮已分配");
            // 为按钮的 onClick 事件添加一个监听器
            start.onClick.AddListener(OnStartButtonClick);
        }
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("game/game");
    }
}
