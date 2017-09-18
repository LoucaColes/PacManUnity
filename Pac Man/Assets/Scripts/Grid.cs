using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid m_grid;

    [SerializeField]
    private int m_width;

    [SerializeField]
    private int m_height;

    private Node[,] m_nodes;

    [SerializeField]
    private Node m_ghostExit;

    public void CreateGrid(int _width, int _height)
    {
        m_width = _width;
        m_height = _height;
        m_nodes = new Node[_width, _height];

        if (m_grid == null)
        {
            m_grid = this;
        }
        else if (m_grid != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddNodeToGrid(int _x, int _y, Node _newNode)
    {
        m_nodes[_x, _y] = _newNode;
    }

    public Node GetNodeAt(int _x, int _y)
    {
        if (_x < 0 || _x > m_width - 1 || _y < 0 || _y > m_height - 1)
        {
            return null;
        }
        return m_nodes[_x, _y];
    }

    public void SetExitPoint(Node _point)
    {
        m_ghostExit = _point;
    }

    public Node GetExitPoint()
    {
        return m_ghostExit;
    }
}