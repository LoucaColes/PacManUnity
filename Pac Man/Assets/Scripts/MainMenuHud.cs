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
                if (m_player.GetButtonDown("Accept"))
                {
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