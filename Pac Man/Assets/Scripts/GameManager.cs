using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager m_gameManager;

    private int m_level;

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

        m_gameState = GameState.MAIN;

        m_level = 0;

        m_ghosts = new List<GameObject>();
        m_ghostScripts = new List<Ghosts>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_mainHud && m_gameHud)
        {
            CanvasUpdate();
            StateUpdate();
        }
    }

    public void NextLevel()
    {
        m_level++;
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
                break;

            case GameState.SETUP:
                m_levelLoader.CreateLevel();
                NextLevel();
                break;

            case GameState.GAME:
                break;
        }
    }

    public void ChangeState(GameState _state)
    {
        m_gameState = _state;
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

        m_gameState = GameState.MAIN;
        m_level = 0;
    }

    public void ClearLevel()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public void MoveToNextLevel()
    {
        ClearLevel();
        m_gameState = GameState.SETUP;
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

    public void SetGhostsFrightened()
    {
        for (int i = 0; i < m_ghostScripts.Count; i++)
        {
            m_ghostScripts[i].ChangeMode(Ghosts.GhostMode.FRIGHTENED);
        }
    }
}