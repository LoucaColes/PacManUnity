using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public int m_lives;

    // Use this for initialization
    private void Start()
    {
        GameManager.m_gameManager.GetGameHud().UpdateLivesText(m_lives);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UpdateLives(int _amount)
    {
        m_lives += _amount;
        GameManager.m_gameManager.GetGameHud().UpdateLivesText(m_lives);
        if (m_lives <= 0)
        {
            GameManager.m_gameManager.GameOver();
        }
    }
}