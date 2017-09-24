using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDebug : MonoBehaviour
{
    private bool m_debugEnabled;
    private bool m_ghostsDisabled;
    public Canvas m_debugCanvas;

    public KeyCode m_ghostDisableCode;
    public KeyCode m_clearLevelCode;
    public KeyCode m_killPacManCode;

    // Use this for initialization
    private void Start()
    {
        m_debugEnabled = false;
        m_debugCanvas.enabled = false;
        m_ghostsDisabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        DebugMode();
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.m_gameManager.GameOver();
        }
    }

    private void DebugMode()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                m_debugEnabled = !m_debugEnabled;
                m_debugCanvas.enabled = m_debugEnabled;
            }
        }

        if (m_debugEnabled)
        {
            if (Input.GetKeyDown(m_ghostDisableCode))
            {
                DisableGhosts();
            }
            if (Input.GetKeyDown(m_clearLevelCode))
            {
                ClearLevel();
            }
            if (Input.GetKeyDown(m_killPacManCode))
            {
                PacManMovement.m_pacman.KillPacMan();
            }
        }
    }

    private void DisableGhosts()
    {
        m_ghostsDisabled = !m_ghostsDisabled;
        if (m_ghostsDisabled)
        {
            GameObject[] t_ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            for (int i = 0; i < t_ghosts.Length; i++)
            {
                t_ghosts[i].GetComponent<Ghosts>().enabled = false;
            }
        }
        else
        {
            GameObject[] t_ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            for (int i = 0; i < t_ghosts.Length; i++)
            {
                t_ghosts[i].GetComponent<Ghosts>().enabled = true;
            }
        }
    }

    private void ClearLevel()
    {
        GameManager.m_gameManager.MoveToNextLevel();
    }
}