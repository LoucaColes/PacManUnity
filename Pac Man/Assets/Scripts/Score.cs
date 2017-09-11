using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int m_score;

    // Use this for initialization
    private void Start()
    {
        m_score = 0;
        GameManager.m_gameManager.GetGameHud().UpdateScoreText(m_score);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public int GetScore()
    {
        return m_score;
    }

    public void UpdateScore(int _scoreValue)
    {
        m_score += _scoreValue;
        GameManager.m_gameManager.GetGameHud().UpdateScoreText(m_score);
    }
}