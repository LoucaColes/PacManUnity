using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghosts
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void ModeUpdate()
    {
        base.ModeUpdate();
        switch (m_currentMode)
        {
            case GhostMode.CHASE:
                m_targetNode = m_cornerNode;
                if (m_timer >= m_chaseTime)
                {
                    ChangeMode(GhostMode.SCATTER);
                    m_timer = 0;
                }
                break;
        }
    }

    protected override void MoveToNextNode()
    {
        if (m_targetNode)
        {
            switch (m_currentMode)
            {
                case GhostMode.SCATTER:
                    if (Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)).IsIntersection())
                    {
                        if (!m_nextNode)
                        {
                            CompareNodes();
                        }
                        if (m_nextNode)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, m_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
                            if (GetDistance(transform.position, m_nextNode.GetPosition()) < 0.1f)
                            {
                                m_nextNode = null;
                            }
                        }
                    }
                    else if (!Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)).IsIntersection())
                    {
                        if (m_nextNode)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, m_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
                        }
                        if (m_currentDirection == Direction.Directions.UP)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.RIGHT;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.LEFT;
                                    break;
                                }
                            }
                        }
                        if (m_currentDirection == Direction.Directions.DOWN)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.RIGHT;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.LEFT;
                                    break;
                                }
                            }
                        }
                        if (m_currentDirection == Direction.Directions.RIGHT)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.UP;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.DOWN;
                                    break;
                                }
                            }
                        }
                        if (m_currentDirection == Direction.Directions.LEFT)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.UP;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.DOWN;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case GhostMode.CHASE:
                    if (Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)).IsIntersection())
                    {
                        if (!m_nextNode)
                        {
                            CompareNodesChase();
                        }
                        if (m_nextNode)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, m_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
                            if (GetDistance(transform.position, m_nextNode.GetPosition()) < 0.1f)
                            {
                                m_nextNode = null;
                            }
                        }
                    }
                    else if (!Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)).IsIntersection())
                    {
                        if (m_nextNode)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, m_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
                        }
                        if (m_currentDirection == Direction.Directions.UP)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.RIGHT;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.LEFT;
                                    break;
                                }
                            }
                        }
                        if (m_currentDirection == Direction.Directions.DOWN)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.RIGHT;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.LEFT;
                                    break;
                                }
                            }
                        }
                        if (m_currentDirection == Direction.Directions.RIGHT)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.UP;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.DOWN;
                                    break;
                                }
                            }
                        }
                        if (m_currentDirection == Direction.Directions.LEFT)
                        {
                            m_nextNode = null;
                            Node t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y));
                            if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                            {
                                m_nextNode = t_possibleNode;
                                break;
                            }
                            else if (!t_possibleNode.IsWalkable())
                            {
                                t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1));
                                if (t_possibleNode.IsWalkable() && !t_possibleNode.IsPartOfGhostHouse())
                                {
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.UP;
                                    break;
                                }
                                else if (!t_possibleNode.IsWalkable())
                                {
                                    t_possibleNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1));
                                    m_nextNode = t_possibleNode;
                                    m_currentDirection = Direction.Directions.DOWN;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case GhostMode.EXIT:
                    CompareNodesForExit();
                    if (m_nextNode)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, m_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
                    }
                    break;
            }
        }
    }

    private void CompareNodesChase()
    {
        List<Node> t_possibleNodes = new List<Node>();
        Node t_upNode = Grid.m_grid.GetNodeAt((int)transform.position.x, (int)transform.position.y + 1);
        Node t_downNode = Grid.m_grid.GetNodeAt((int)transform.position.x, (int)transform.position.y - 1);
        Node t_rightNode = Grid.m_grid.GetNodeAt((int)transform.position.x + 1, (int)transform.position.y);
        Node t_leftNode = Grid.m_grid.GetNodeAt((int)transform.position.x - 1, (int)transform.position.y);
        t_possibleNodes.Add(t_upNode);
        t_possibleNodes.Add(t_downNode);
        t_possibleNodes.Add(t_rightNode);
        t_possibleNodes.Add(t_leftNode);
        int t_rand = (int)m_oppositeDirection;
        while (t_rand == (int)m_oppositeDirection)
        {
            t_rand = Random.Range(0, 3);
        }
        if (t_rand != (int)m_oppositeDirection)
        {
            m_nextNode = t_possibleNodes[t_rand];
            switch (t_rand)
            {
                case 0:
                    m_currentDirection = Direction.Directions.UP;
                    SetOppositeDirection();
                    break;

                case 1:
                    m_currentDirection = Direction.Directions.DOWN;
                    SetOppositeDirection();
                    break;

                case 2:
                    m_currentDirection = Direction.Directions.RIGHT;
                    SetOppositeDirection();
                    break;

                case 3:
                    m_currentDirection = Direction.Directions.LEFT;
                    SetOppositeDirection();
                    break;
            }
        }
    }
}