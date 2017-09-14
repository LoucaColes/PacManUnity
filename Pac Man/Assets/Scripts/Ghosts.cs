using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosts : MonoBehaviour
{
    public float m_moveSpeed;
    private float m_originalSpeed;

    [SerializeField]
    protected Node m_startNode;

    public float m_scatterTime;

    public float m_chaseTime;

    public float m_frightenedTime;

    public float m_waitTime;

    protected float m_timer;

    private Animator m_animator;

    public enum GhostMode
    {
        SCATTER,
        CHASE,
        FRIGHTENED,
        WAIT,
        EXIT
    }

    [SerializeField]
    protected GhostMode m_currentMode;

    private GhostMode m_previousMode;

    [SerializeField]
    protected Direction.Directions m_currentDirection;

    protected Direction.Directions m_oppositeDirection;

    [SerializeField]
    protected Node m_targetNode;

    [SerializeField]
    protected Node m_nextNode;

    [SerializeField]
    protected Node m_cornerNode;

    private float m_poweredTime;
    private float m_poweredTimer;

    [SerializeField]
    private bool m_powered;

    [SerializeField]
    private PowerUp.Powers m_currentPower;

    private SpriteRenderer m_renderer;

    private float m_origSpeed;

    // Use this for initialization
    protected virtual void Start()
    {
        m_currentMode = GhostMode.WAIT;
        m_currentDirection = Direction.Directions.STOP;
        m_oppositeDirection = Direction.Directions.STOP;
        m_targetNode = Grid.m_grid.GetExitPoint();
        m_nextNode = null;
        m_animator = GetComponent<Animator>();
        m_timer = 0;
        SetStartNode(Grid.m_grid.GetNodeAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
        m_origSpeed = m_moveSpeed;
        m_poweredTimer = 0;
        m_powered = false;
        m_currentPower = PowerUp.Powers.COUNT;
        m_renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ModeUpdate();
        MoveToNextNode();
        MovementAnimationCheck();
        SetOppositeDirection();
        if (m_powered)
        {
            m_poweredTimer += Time.deltaTime;
            if (m_poweredTimer >= m_poweredTime)
            {
                switch (m_currentPower)
                {
                    case PowerUp.Powers.FASTGHOST:
                    case PowerUp.Powers.SLOWGHOST:
                        m_moveSpeed = m_origSpeed;
                        break;

                    case PowerUp.Powers.INVISGHOST:
                        m_renderer.enabled = true;
                        m_animator.enabled = true;
                        break;
                }
                m_poweredTimer = 0;
                m_currentPower = PowerUp.Powers.COUNT;
                m_powered = false;
            }
        }
    }

    protected void MovementAnimationCheck()
    {
        if (m_currentMode != GhostMode.FRIGHTENED)
        {
            switch (m_currentDirection)
            {
                case Direction.Directions.RIGHT:
                    m_animator.SetBool("Right", true);
                    m_animator.SetBool("Up", false);
                    m_animator.SetBool("Left", false);
                    m_animator.SetBool("Down", false);
                    m_animator.SetBool("Frightened", false);
                    m_animator.SetBool("Returning", false);
                    break;

                case Direction.Directions.LEFT:
                    m_animator.SetBool("Right", false);
                    m_animator.SetBool("Up", false);
                    m_animator.SetBool("Left", true);
                    m_animator.SetBool("Down", false);
                    m_animator.SetBool("Frightened", false);
                    m_animator.SetBool("Returning", false);
                    break;

                case Direction.Directions.UP:
                    m_animator.SetBool("Right", false);
                    m_animator.SetBool("Up", true);
                    m_animator.SetBool("Left", false);
                    m_animator.SetBool("Down", false);
                    m_animator.SetBool("Frightened", false);
                    m_animator.SetBool("Returning", false);
                    break;

                case Direction.Directions.DOWN:
                    m_animator.SetBool("Right", false);
                    m_animator.SetBool("Up", false);
                    m_animator.SetBool("Left", false);
                    m_animator.SetBool("Down", true);
                    m_animator.SetBool("Frightened", false);
                    m_animator.SetBool("Returning", false);
                    break;
            }
        }
        else if (m_currentMode == GhostMode.FRIGHTENED)
        {
            m_animator.SetBool("Right", false);
            m_animator.SetBool("Up", false);
            m_animator.SetBool("Left", false);
            m_animator.SetBool("Down", false);
            if (m_timer < m_frightenedTime / 2)
            {
                m_animator.SetBool("Frightened", true);
                m_animator.SetBool("Returning", false);
            }
            else if (m_timer >= m_frightenedTime / 2 && m_timer < m_frightenedTime)
            {
                m_animator.SetBool("Frightened", false);
                m_animator.SetBool("Returning", true);
            }
        }
    }

    public void ChangeMode(GhostMode _newMode)
    {
        m_previousMode = m_currentMode;
        m_currentMode = _newMode;
    }

    public void SetStartNode(Node _startNode)
    {
        m_startNode = _startNode;
    }

    protected virtual void ModeUpdate()
    {
        m_timer += Time.deltaTime;
        switch (m_currentMode)
        {
            case GhostMode.WAIT:
                m_targetNode = m_startNode;
                if (m_timer >= m_waitTime)
                {
                    ChangeMode(GhostMode.EXIT);
                    m_timer = 0;
                }
                break;

            case GhostMode.EXIT:
                m_targetNode = Grid.m_grid.GetExitPoint();
                if (GetDistance(transform.position, m_targetNode.GetPosition()) == 0f)
                {
                    ChangeMode(GhostMode.SCATTER);
                }
                break;

            case GhostMode.SCATTER:
                m_targetNode = m_cornerNode;
                if (m_timer >= m_scatterTime)
                {
                    ChangeMode(GhostMode.CHASE);
                    m_timer = 0;
                }
                break;

            case GhostMode.CHASE:
                if (m_timer >= m_chaseTime)
                {
                    ChangeMode(GhostMode.SCATTER);
                    m_timer = 0;
                }
                break;

            case GhostMode.FRIGHTENED:
                if (m_timer >= m_frightenedTime)
                {
                    ChangeMode(GhostMode.SCATTER);
                    m_timer = 0;
                }
                break;
        }
    }

    protected float GetDistance(Vector3 _currentPosition, Vector3 _nextPosition)
    {
        return Vector3.Distance(_currentPosition, _nextPosition);
    }

    protected virtual void CompareNodes()
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

        float t_minDistance = 10000f;

        for (int i = 0; i < t_possibleNodes.Count; i++)
        {
            if (t_possibleNodes[i].IsWalkableForGhosts() && !t_possibleNodes[i].IsPartOfGhostHouse())
            {
                float t_distance = GetDistance(t_possibleNodes[i].GetPosition(), m_targetNode.GetPosition());
                if (t_distance < t_minDistance)
                {
                    t_minDistance = t_distance;
                    m_nextNode = t_possibleNodes[i];
                    switch (i)
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
    }

    protected virtual void CompareNodesFrightened()
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

        float t_maxDistance = 0;

        for (int i = 0; i < t_possibleNodes.Count; i++)
        {
            if (t_possibleNodes[i].IsWalkableForGhosts() && !t_possibleNodes[i].IsPartOfGhostHouse())
            {
                float t_distance = GetDistance(t_possibleNodes[i].GetPosition(), m_targetNode.GetPosition());
                if (t_distance > t_maxDistance)
                {
                    t_maxDistance = t_distance;
                    m_nextNode = t_possibleNodes[i];
                    switch (i)
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
    }

    protected void CompareNodesForExit()
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

        float t_minDistance = 10000f;

        for (int i = 0; i < t_possibleNodes.Count; i++)
        {
            if (t_possibleNodes[i].IsWalkableForGhosts())
            {
                float t_distance = GetDistance(t_possibleNodes[i].GetPosition(), m_targetNode.GetPosition());
                if (t_distance < t_minDistance)
                {
                    t_minDistance = t_distance;
                    m_nextNode = t_possibleNodes[i];
                    switch (i)
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
    }

    private void FindNextNode()
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

        for (int i = 0; i < t_possibleNodes.Count; i++)
        {
            if (t_possibleNodes[i].IsWalkableForGhosts())
            {
                if (i == 0 && m_oppositeDirection != Direction.Directions.DOWN)
                {
                    m_nextNode = t_possibleNodes[i];
                }
                else if (i == 1 && m_oppositeDirection != Direction.Directions.UP)
                {
                    m_nextNode = t_possibleNodes[i];
                }
                else if (i == 2 && m_oppositeDirection != Direction.Directions.LEFT)
                {
                    m_nextNode = t_possibleNodes[i];
                }
                else if (i == 3 && m_oppositeDirection != Direction.Directions.RIGHT)
                {
                    m_nextNode = t_possibleNodes[i];
                }
            }
        }
    }

    protected virtual void MoveToNextNode()
    {
        if (m_targetNode)
        {
            switch (m_currentMode)
            {
                case GhostMode.SCATTER:
                case GhostMode.CHASE:
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

                case GhostMode.EXIT:
                    CompareNodesForExit();
                    if (m_nextNode)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, m_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
                    }
                    break;

                case GhostMode.FRIGHTENED:
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
            }
        }
    }

    public GhostMode GetCurrentMode()
    {
        return m_currentMode;
    }

    public void ResetPosition()
    {
        transform.position = m_startNode.GetPosition();
    }

    public void ResetTimer()
    {
        m_timer = 0;
    }

    public void SetCornerNode(Node _node)
    {
        m_cornerNode = _node;
    }

    protected void SetOppositeDirection()
    {
        switch (m_currentDirection)
        {
            case Direction.Directions.STOP:
                m_oppositeDirection = Direction.Directions.STOP;
                break;

            case Direction.Directions.UP:
                m_oppositeDirection = Direction.Directions.DOWN;
                break;

            case Direction.Directions.DOWN:
                m_oppositeDirection = Direction.Directions.UP;
                break;

            case Direction.Directions.RIGHT:
                m_oppositeDirection = Direction.Directions.LEFT;
                break;

            case Direction.Directions.LEFT:
                m_oppositeDirection = Direction.Directions.RIGHT;
                break;
        }
    }

    public void SetPowered(PowerUp.Powers _newPower, float _powerTime)
    {
        m_powered = true;
        m_currentPower = _newPower;
        m_poweredTime = _powerTime;
        m_poweredTimer = 0;
        switch (m_currentPower)
        {
            case PowerUp.Powers.FASTGHOST:
                m_moveSpeed = m_moveSpeed * 2;
                m_poweredTime *= 2;
                break;

            case PowerUp.Powers.SLOWGHOST:
                m_moveSpeed = m_moveSpeed / 2;
                m_poweredTime *= 2;
                break;

            case PowerUp.Powers.INVISGHOST:
                m_renderer.enabled = false;
                m_animator.enabled = false;
                break;
        }
    }
}