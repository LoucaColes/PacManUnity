﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ColourToPrefab : System.Object
{
    public string m_name;
    public GameObject m_prefab;
    public Color m_colour;
}

public class LevelLoader : MonoBehaviour
{
    public Texture2D m_levelMap;

    public ColourToPrefab[] m_colourToPrefabs;

    private Transform m_levelParent;

    private Grid m_grid;

    public int m_scale;

    [SerializeField]
    private Vector2 m_exitPoint;

    public Canvas m_mainHud;
    public Canvas m_gameHud;

    [SerializeField]
    private List<GameObject> m_ghosts;

    [SerializeField]
    private Node m_blinkyCorner;

    [SerializeField]
    private Node m_clydeCorner;

    [SerializeField]
    private Node m_inkyCorner;

    [SerializeField]
    private Node m_pinkyCorner;

    public int m_levelCap;

    [SerializeField]
    private List<GameObject> m_teleportPoints;

    // Use this for initialization
    private void Start()
    {
        SetUp();
    }

    public Transform GetLevelParent()
    {
        return m_levelParent;
    }

    public void SetUp()
    {
        GameObject t_newLevel = new GameObject("Level");
        m_levelParent = t_newLevel.transform;
        t_newLevel.AddComponent<Grid>();
        GameManager t_gameManager = t_newLevel.AddComponent<GameManager>();
        t_gameManager.SetCanvases(m_mainHud, m_gameHud);
        t_gameManager.SetLevelLoader(this);
        t_gameManager.SetPelletCount(0);
        t_gameManager.SetLevelCap(m_levelCap);
        m_grid = t_newLevel.GetComponent<Grid>();
        m_ghosts = new List<GameObject>();
        m_teleportPoints = new List<GameObject>();
    }

    public void CreateLevel()
    {
        m_grid.CreateGrid(m_levelMap.width, m_levelMap.height);

        for (int x = 0; x < m_levelMap.width; x++)
        {
            for (int y = 0; y < m_levelMap.height; y++)
            {
                GenerateLevelPiece(x, y);
            }
        }

        m_grid.SetExitPoint(m_grid.GetNodeAt((int)m_exitPoint.x, (int)m_exitPoint.y));

        for (int i = 0; i < m_ghosts.Count; i++)
        {
            GameManager.m_gameManager.AddGhost(m_ghosts[i]);
            if (m_ghosts[i].name.Contains("Blinky"))
            {
                m_ghosts[i].gameObject.GetComponent<Ghosts>().SetCornerNode(m_blinkyCorner);
            }
            if (m_ghosts[i].name.Contains("Clyde"))
            {
                m_ghosts[i].gameObject.GetComponent<Ghosts>().SetCornerNode(m_clydeCorner);
            }
            if (m_ghosts[i].name.Contains("Inky"))
            {
                m_ghosts[i].gameObject.GetComponent<Ghosts>().SetCornerNode(m_inkyCorner);
            }
            if (m_ghosts[i].name.Contains("Pinky"))
            {
                m_ghosts[i].gameObject.GetComponent<Ghosts>().SetCornerNode(m_pinkyCorner);
            }
            if (GameManager.m_gameManager.GetGameType() == GameManager.GameType.COMP)
            {
                m_ghosts[i].GetComponent<PlayableGhost>().m_playerId = i + 1;
                if (m_ghosts[i].GetComponent<Blinky>())
                {
                    Destroy(m_ghosts[i].GetComponent<Blinky>());
                }
                else if (m_ghosts[i].GetComponent<Clyde>())
                {
                    Destroy(m_ghosts[i].GetComponent<Clyde>());
                }
                else if (m_ghosts[i].GetComponent<Inky>())
                {
                    Destroy(m_ghosts[i].GetComponent<Inky>());
                }
                else if (m_ghosts[i].GetComponent<Pinky>())
                {
                    Destroy(m_ghosts[i].GetComponent<Pinky>());
                }
            }
            else
            {
                Destroy(m_ghosts[i].GetComponent<PlayableGhost>());
            }
        }

        for (int i = 0; i < m_teleportPoints.Count; i++)
        {
            int t_index = i + 1;
            if (t_index >= m_teleportPoints.Count)
            {
                t_index = 0;
            }
            m_teleportPoints[i].GetComponent<TeleportPoint>().SetNextNode(m_teleportPoints[t_index].GetComponent<Node>());
        }

        GameManager.m_gameManager.ChangeState(GameManager.GameState.GAME);
    }

    public void ClearLists()
    {
        m_ghosts.Clear();
        m_teleportPoints.Clear();
    }

    private bool CheckPixelAlpha(int _xPos, int _yPos)
    {
        Color t_pixelColor = m_levelMap.GetPixel(_xPos, _yPos);

        if (t_pixelColor.a == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void GenerateLevelPiece(int _xPos, int _yPos)
    {
        if (CheckPixelAlpha(_xPos, _yPos))
        {
            Color t_pixelColor = m_levelMap.GetPixel(_xPos, _yPos);
            for (int i = 0; i < m_colourToPrefabs.Length; i++)
            {
                if (m_colourToPrefabs[i].m_colour.Equals(t_pixelColor))
                {
                    CreateNode(_xPos, _yPos, i);
                }
            }
        }
        else
        {
            CreateBlankNode(_xPos, _yPos);
        }
    }

    private void CreateBlankNode(int _x, int _y)
    {
        Vector3 t_position = new Vector3(_x, _y, 0f) * m_scale;
        GameObject t_newPiece = new GameObject("BlankNode");
        t_newPiece.transform.position = t_position;
        t_newPiece.AddComponent<Node>();
        Node t_node = t_newPiece.GetComponent<Node>();
        t_node.SetUpNode(null, t_position, true, true, t_newPiece.name, false, false);
        m_grid.AddNodeToGrid(_x, _y, t_node);
        t_newPiece.transform.parent = m_levelParent;
    }

    private void CreateNode(int _x, int _y, int _i)
    {
        Vector3 t_position = new Vector3(_x, _y, 0f) * m_scale;
        GameObject t_newPiece = (GameObject)Instantiate(m_colourToPrefabs[_i].m_prefab, t_position, Quaternion.identity);
        t_newPiece.AddComponent<Node>();
        Node t_node = t_newPiece.GetComponent<Node>();
        switch (_i)
        {
            case 0:
                t_node.SetUpNode(t_newPiece, t_position, false, false, m_colourToPrefabs[_i].m_name, false, false);
                break;

            case 1:
            case 2:
                t_node.SetUpNode(t_newPiece, t_position, true, true, m_colourToPrefabs[_i].m_name, false, false);
                break;

            case 3:
                t_node.SetUpNode(t_newPiece, t_position, true, true, m_colourToPrefabs[_i].m_name, false, false);
                GameManager.m_gameManager.UpdatePelletCount(1);
                break;

            case 4:
                t_node.SetUpNode(t_newPiece, t_position, true, true, m_colourToPrefabs[_i].m_name, false, false);
                if (GameManager.m_gameManager.GetGameType() == GameManager.GameType.RAND)
                {
                    t_newPiece.GetComponent<PowerUp>().SetPower();
                }
                else
                {
                    Destroy(t_newPiece.GetComponent<PowerUp>());
                }
                GameManager.m_gameManager.UpdatePelletCount(1);
                break;

            case 5:
                t_node.SetUpNode(t_newPiece, t_position, true, true, m_colourToPrefabs[_i].m_name, false, false);
                m_teleportPoints.Add(t_newPiece);
                break;

            case 6:
                t_node.SetUpNode(t_newPiece, t_position, false, true, m_colourToPrefabs[_i].m_name, false, true);
                m_exitPoint = new Vector2(t_position.x, t_position.y + 1);
                break;

            case 7:
            case 8:
            case 9:
            case 10:
                t_node.SetUpNode(t_newPiece, t_position, false, true, m_colourToPrefabs[_i].m_name, false, true);
                m_ghosts.Add(t_newPiece);
                break;

            case 15:
                t_node.SetUpNode(t_newPiece, t_position, false, true, m_colourToPrefabs[_i].m_name, false, true);
                break;

            case 11:
                m_blinkyCorner = t_node;
                t_node.SetUpNode(t_newPiece, t_position, false, false, m_colourToPrefabs[_i].m_name, false, false);
                break;

            case 12:
                m_clydeCorner = t_node;
                t_node.SetUpNode(t_newPiece, t_position, false, false, m_colourToPrefabs[_i].m_name, false, false);
                break;

            case 13:
                m_inkyCorner = t_node;
                t_node.SetUpNode(t_newPiece, t_position, false, false, m_colourToPrefabs[_i].m_name, false, false);
                break;

            case 14:
                m_pinkyCorner = t_node;
                t_node.SetUpNode(t_newPiece, t_position, false, false, m_colourToPrefabs[_i].m_name, false, false);
                break;

            case 16:
                t_node.SetUpNode(t_newPiece, t_position, true, true, m_colourToPrefabs[_i].m_name, true, false);
                break;
        }
        m_grid.AddNodeToGrid(_x, _y, t_node);
        t_newPiece.transform.parent = m_levelParent;
    }
}