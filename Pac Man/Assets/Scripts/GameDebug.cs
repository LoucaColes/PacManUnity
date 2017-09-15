using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDebug : MonoBehaviour
{
    private bool m_ghostsDisabled;

    // Use this for initialization
    private void Start()
    {
        m_ghostsDisabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        DisableGhosts();
        ClearPellets();
    }

    private void DisableGhosts()
    {
        if (Input.GetKeyDown(KeyCode.D))
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
    }

    private void ClearPellets()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.m_gameManager.MoveToNextLevel(); ;
        }
    }
}