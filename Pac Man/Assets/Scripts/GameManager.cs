using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager m_gameManager;

    private int m_level;
    private int m_levelCap;

    public enum GameState
    {
        MAIN,
        SETUP,
        GAME
    }

    [SerializeField]
    private GameState m_gameState;

    [SerializeField]
    private Canvas m_mainHud;

    [SerializeField]
    private Canvas m_gameHud;

    private GameHud m_gameHudScript;

    private LevelLoader m_levelLoader;

    private int m_pelletCount;

    [SerializeField]
    private List<GameObject> m_ghosts;

    [SerializeField]
    private List<Ghosts> m_ghostScripts;

    public enum GameType
    {
        NORM,
        RAND,
        COMP,
        TWITCH,
        COUNT,
    }

    private GameType m_gameType;

    // Use this for initialization
    private void Start()
    {
        if (m_gameManager == null)
        {
            m_gameManager = this;
        }
        else if (m_gameManager != this)
        {
            Destroy(gameObject);
        }

        ChangeState(GameState.MAIN);
        SetGameType(GameType.NORM);
        m_level = 0;

        m_ghosts = new List<GameObject>();
        m_ghostScripts = new List<Ghosts>();
    }

    public void NextLevel()
    {
        if (m_level < m_levelCap)
        {
            m_level++;
        }
        else
        {
            GameOver();
        }
        m_gameHudScript.UpdateLevelText(m_level);
    }

    public int GetLevel()
    {
        return m_level;
    }

    public void SetCanvases(Canvas _mainHud, Canvas _gameHud)
    {
        m_mainHud = _mainHud;
        m_gameHud = _gameHud;
        m_gameHudScript = m_gameHud.GetComponent<GameHud>();
    }

    private void CanvasUpdate()
    {
        switch (m_gameState)
        {
            case GameState.MAIN:
                m_mainHud.enabled = true;
                m_gameHud.enabled = false;
                break;

            case GameState.GAME:
                m_mainHud.enabled = false;
                m_gameHud.enabled = true;
                break;

            case GameState.SETUP:
                m_mainHud.enabled = false;
                m_gameHud.enabled = false;
                break;
        }
    }

    public void SetLevelLoader(LevelLoader _levelLoader)
    {
        m_levelLoader = _levelLoader;
    }

    private void StateUpdate()
    {
        switch (m_gameState)
        {
            case GameState.MAIN:
                if (PacManMovement.m_pacman && PacManMovement.m_pacman.IsEnabled())
                {
                    PacManMovement.m_pacman.DisablePacMan();
                }
                break;

            case GameState.SETUP:
                m_levelLoader.CreateLevel();
                NextLevel();
                break;

            case GameState.GAME:
                if (PacManMovement.m_pacman && !PacManMovement.m_pacman.IsEnabled())
                {
                    PacManMovement.m_pacman.EnablePacMan();
                }
                break;
        }
    }

    public void ChangeState(GameState _state)
    {
        m_gameState = _state;
        StateUpdate();
        CanvasUpdate();
    }

    public GameState GetState()
    {
        return m_gameState;
    }

    public GameHud GetGameHud()
    {
        return m_gameHudScript;
    }

    public void GameOver()
    {
        ClearGhosts();
        ClearLevel();

        ChangeState(GameState.MAIN);
        m_level = 0;
    }

    public void ClearLevel()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.GetComponent<PacManMovement>())
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                else
                {
                    transform.GetChild(i).gameObject.GetComponent<PacManMovement>().ResetPostion();
                }
            }
        }
    }

    public void MoveToNextLevel()
    {
        if (m_level == m_levelCap)
        {
            GameOver();
            Destroy(PacManMovement.m_pacman.gameObject);
        }
        else
        {
            ClearLevel();
            ClearGhosts();
            ChangeState(GameState.SETUP);
        }
    }

    public void SetPelletCount(int _amount)
    {
        m_pelletCount = _amount;
    }

    public void UpdatePelletCount(int _amount)
    {
        m_pelletCount += _amount;
        if (m_pelletCount <= 0)
        {
            MoveToNextLevel();
        }
    }

    public int GetPelletCount()
    {
        return m_pelletCount;
    }

    public void AddGhost(GameObject _newGhost)
    {
        m_ghosts.Add(_newGhost);
        m_ghostScripts.Add(_newGhost.GetComponent<Ghosts>());
    }

    public void ClearGhosts()
    {
        m_ghosts.Clear();
        m_ghostScripts.Clear();
        m_levelLoader.ClearGhosts();
    }

    public void SetGameType(GameType _newType)
    {
        m_gameType = _newType;
        if (m_gameType == GameType.TWITCH)
        {
            TwitchCanvas.m_instance.gameObject.GetComponent<Canvas>().enabled = true;
            TwitchCanvas.m_instance.enabled = true;
        }
        else
        {
            TwitchCanvas.m_instance.gameObject.GetComponent<Canvas>().enabled = false;
            TwitchCanvas.m_instance.enabled = false;
        }
    }

    public GameType GetGameType()
    {
        return m_gameType;
    }

    public void SetGameType(int _newType)
    {
        if (_newType < (int)GameType.COUNT)
        {
            m_gameType = (GameType)_newType;
            if (m_gameType == GameType.TWITCH)
            {
                TwitchCanvas.m_instance.gameObject.GetComponent<Canvas>().enabled = true;
                TwitchCanvas.m_instance.enabled = true;
            }
            else
            {
                TwitchCanvas.m_instance.gameObject.GetComponent<Canvas>().enabled = false;
                TwitchCanvas.m_instance.enabled = false;
            }
        }
    }

    public void SetLevelCap(int _cap)
    {
        m_levelCap = _cap;
    }
}