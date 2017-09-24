using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class MainMenuHud : MonoBehaviour
{
    public int m_playerId;
    private Player m_player;

    public Text m_modeText;
    private int m_index;

    public InputField m_userNameField;
    public InputField m_passwordField;
    public InputField m_chatRoomField;

    private string m_username;
    private string m_password;
    private string m_chatroom;

    // Use this for initialization
    private void Start()
    {
        m_player = ReInput.players.GetPlayer(m_playerId);
        m_index = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.m_gameManager)
        {
            if (GameManager.m_gameManager.GetState() == GameManager.GameState.MAIN)
            {
                m_modeText.text = GameManager.m_gameManager.GetGameType().ToString();
                if (m_player.GetButtonDown("MoveLeft"))
                {
                    if (m_index > 0)
                    {
                        m_index--;
                        GameManager.m_gameManager.SetGameType(m_index);
                    }
                }
                if (m_player.GetButtonDown("MoveRight"))
                {
                    if (m_index < (int)GameManager.GameType.COUNT)
                    {
                        m_index++;
                        GameManager.m_gameManager.SetGameType(m_index);
                    }
                }

                if (GameManager.m_gameManager.GetGameType() == GameManager.GameType.TWITCH)
                {
                    m_userNameField.enabled = true;
                    m_userNameField.gameObject.SetActive(true);
                    m_passwordField.enabled = true;
                    m_passwordField.gameObject.SetActive(true);
                    m_chatRoomField.enabled = true;
                    m_chatRoomField.gameObject.SetActive(true);
                }
                else
                {
                    m_userNameField.enabled = false;
                    m_userNameField.gameObject.SetActive(false);
                    m_passwordField.enabled = false;
                    m_passwordField.gameObject.SetActive(false);
                    m_chatRoomField.enabled = false;
                    m_chatRoomField.gameObject.SetActive(false);
                }

                if (m_player.GetButtonDown("Accept"))
                {
                    if (GameManager.m_gameManager.GetGameType() == GameManager.GameType.TWITCH)
                    {
                        m_username = m_userNameField.text;
                        m_password = m_passwordField.text;
                        m_chatroom = m_chatRoomField.text;
                        GameManager.m_gameManager.SetTwitchData(m_username, m_password, m_chatroom);
                    }
                    PlayGame();
                }
                if (m_player.GetButtonDown("Return"))
                {
                    ExitGame();
                }
            }
        }
    }

    public void PlayGame()
    {
        GameManager.m_gameManager.ChangeState(GameManager.GameState.SETUP);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}