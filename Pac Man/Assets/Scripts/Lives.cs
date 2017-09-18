using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public int m_lives;
    private int m_livesOriginal;

    // Use this for initialization
    private void Start()
    {
        GameManager.m_gameManager.GetGameHud().UpdateLivesText(m_lives);
        m_livesOriginal = m_lives;
    }

    public void UpdateLives(int _amount)
    {
        m_lives += _amount;
        GameManager.m_gameManager.GetGameHud().UpdateLivesText(m_lives);
        if (m_lives <= 0)
        {
            m_lives = m_livesOriginal;
            GameManager.m_gameManager.GetGameHud().UpdateLivesText(m_lives);
            GameManager.m_gameManager.GameOver();
        }
    }
}