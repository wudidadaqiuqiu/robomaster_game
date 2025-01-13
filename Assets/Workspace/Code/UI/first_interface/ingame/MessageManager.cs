using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private GameObject message_box;
    [SerializeField] private GameObject message_prefab;

    static public MessageManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        add_message("ÄãºÃ£¬ÂÜ²·×Ó");
    }

    public void add_message(string text)
    {
        GameObject new_message = Instantiate(message_prefab, message_box.transform);
        TextMeshProUGUI message_text = new_message.GetComponent<TextMeshProUGUI>();
        message_text.text = text;
    }
}
