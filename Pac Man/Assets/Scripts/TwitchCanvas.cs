using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwitchCanvas : MonoBehaviour
{
    public Text m_chatText;

    public static TwitchCanvas m_instance;

    private void Start()
    {
        if (!m_instance)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateChatText(string _message)
    {
        m_chatText.text = _message;
    }
}