using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHud : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
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