using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    public Text m_scoreText;
    public Text m_livesText;
    public Text m_levelText;

    // Use this for initialization
    private void Start()
    {
        m_scoreText.text = "";
        m_livesText.text = "";
        m_levelText.text = "";
    }

    public void UpdateScoreText(int _score)
    {
        m_scoreText.text = "Score: " + _score;
    }

    public void UpdateLivesText(int _lives)
    {
        m_livesText.text = "Lives: " + _lives;
    }

    public void UpdateLevelText(int _level)
    {
        m_levelText.text = "Level: " + _level;
    }
}