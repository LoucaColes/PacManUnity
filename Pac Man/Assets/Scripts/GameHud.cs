using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    public Text m_scoreText;
    public Text m_livesText;
    public Text m_levelText;
    public Text m_powerText;

    // Use this for initialization
    private void Start()
    {
        m_scoreText.text = "";
        m_livesText.text = "";
        m_levelText.text = "";
        m_powerText.text = "";
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

    public void UpdatePowerText(string _power)
    {
        m_powerText.text = _power;
        StartCoroutine(SplashText());
    }

    private IEnumerator SplashText()
    {
        yield return new WaitForSeconds(5);
        m_powerText.text = "";
    }
}