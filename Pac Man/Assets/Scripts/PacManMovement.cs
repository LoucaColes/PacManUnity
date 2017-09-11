using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMovement : MonoBehaviour
{
    private Vector3 m_direction;
    public float m_moveSpeed;

    private Direction.Directions m_currentDirection;

    private Animator m_animator;

    [SerializeField]
    private Node m_node;

    private Node m_startNode;

    private Node m_pinkyNode;

    private Score m_scoreScript;

    private Lives m_lives;

    public static PacManMovement m_pacman;

    // Use this for initialization
    protected void Start()
    {
        m_direction = Vector3.zero;
        m_currentDirection = Direction.Directions.STOP;
        m_animator = GetComponent<Animator>();
        m_scoreScript = GetComponent<Score>();

        int t_x = Mathf.RoundToInt(transform.position.x);
        int t_y = Mathf.RoundToInt(transform.position.y);
        m_startNode = Grid.m_grid.GetNodeAt(t_x, t_y);

        if (m_pacman == null)
        {
            m_pacman = this;
        }
        else if (m_pacman != this)
        {
            Destroy(gameObject);
        }

        m_lives = GetComponent<Lives>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_direction != Vector3.zero)
        {
            Node t_nextNode = GetNextNode();
            m_node = t_nextNode;
            if ((t_nextNode && t_nextNode.IsWalkable()))
            {
                transform.position = Vector3.MoveTowards(transform.position, t_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 t_endPos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);
                transform.position = Vector3.MoveTowards(transform.position, t_endPos, m_moveSpeed * Time.deltaTime);
            }
        }
        MovementAnimationCheck();
    }

    private Node GetNextNode()
    {
        Vector2 t_positionToCheck;
        Node t_nextNode;
        if (m_currentDirection == Direction.Directions.UP)
        {
            t_positionToCheck = new Vector2(transform.position.x, transform.position.y + 1);
            t_nextNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(t_positionToCheck.x), Mathf.RoundToInt(t_positionToCheck.y));
            return t_nextNode;
        }
        else if (m_currentDirection == Direction.Directions.DOWN)
        {
            t_positionToCheck = new Vector2(transform.position.x, transform.position.y - 1);
            t_nextNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(t_positionToCheck.x), Mathf.RoundToInt(t_positionToCheck.y));
            return t_nextNode;
        }
        else if (m_currentDirection == Direction.Directions.LEFT)
        {
            t_positionToCheck = new Vector2(transform.position.x - 1, transform.position.y);
            t_nextNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(t_positionToCheck.x), Mathf.RoundToInt(t_positionToCheck.y));
            return t_nextNode;
        }
        else if (m_currentDirection == Direction.Directions.RIGHT)
        {
            t_positionToCheck = new Vector2(transform.position.x + 1, transform.position.y);
            t_nextNode = Grid.m_grid.GetNodeAt(Mathf.RoundToInt(t_positionToCheck.x), Mathf.RoundToInt(t_positionToCheck.y));
            return t_nextNode;
        }
        else
        {
            return null;
        }
    }

    private void MovementAnimationCheck()
    {
        switch (m_currentDirection)
        {
            case Direction.Directions.RIGHT:
                m_animator.SetBool("Right", true);
                m_animator.SetBool("Up", false);
                m_animator.SetBool("Left", false);
                m_animator.SetBool("Down", false);
                break;

            case Direction.Directions.LEFT:
                m_animator.SetBool("Right", false);
                m_animator.SetBool("Up", false);
                m_animator.SetBool("Left", true);
                m_animator.SetBool("Down", false);
                break;

            case Direction.Directions.UP:
                m_animator.SetBool("Right", false);
                m_animator.SetBool("Up", true);
                m_animator.SetBool("Left", false);
                m_animator.SetBool("Down", false);
                break;

            case Direction.Directions.DOWN:
                m_animator.SetBool("Right", false);
                m_animator.SetBool("Up", false);
                m_animator.SetBool("Left", false);
                m_animator.SetBool("Down", true);
                break;
        }
    }

    public Vector3 GetDirection()
    {
        return m_direction;
    }

    public void SetDirection(Vector3 _direction, Direction.Directions _newDirectionState)
    {
        m_direction = _direction;
        m_currentDirection = _newDirectionState;
    }

    public Node GetCurrentNode()
    {
        return m_node;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Pellet"))
        {
            Node t_node = collision.GetComponent<Node>();
            if (t_node.GetName().Contains("Pellet"))
            {
                GameManager.m_gameManager.UpdatePelletCount(-1);
            }
            if (t_node.GetName().Contains("Power"))
            {
                GameObject[] t_ghosts = GameObject.FindGameObjectsWithTag("Ghost");
                for (int i = 0; i < t_ghosts.Length; i++)
                {
                    t_ghosts[i].GetComponent<Ghosts>().ResetTimer();
                    t_ghosts[i].GetComponent<Ghosts>().ChangeMode(Ghosts.GhostMode.FRIGHTENED);
                }
            }
            m_scoreScript.UpdateScore(t_node.GetObject().GetComponent<Scoreable>().GetScoreValue());
            t_node.RemoveObject();
            t_node.SetObject(null);
        }
        if (collision.tag == "Fruit")
        {
            m_scoreScript.UpdateScore(collision.GetComponent<Scoreable>().GetScoreValue());
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Ghost" && collision.GetComponent<Ghosts>().GetCurrentMode() != Ghosts.GhostMode.FRIGHTENED)
        {
            ResetPostion();
            GameObject[] t_ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            for (int i = 0; i < t_ghosts.Length; i++)
            {
                t_ghosts[i].GetComponent<Ghosts>().ResetPosition();
                t_ghosts[i].GetComponent<Ghosts>().ChangeMode(Ghosts.GhostMode.EXIT);
            }
            m_lives.UpdateLives(-1);
        }
        else if (collision.tag == "Ghost" && collision.GetComponent<Ghosts>().GetCurrentMode() == Ghosts.GhostMode.FRIGHTENED)
        {
            collision.GetComponent<Ghosts>().ResetPosition();
            collision.GetComponent<Ghosts>().ChangeMode(Ghosts.GhostMode.EXIT);
            m_scoreScript.UpdateScore(collision.GetComponent<Scoreable>().GetScoreValue());
        }
    }

    private void ResetPostion()
    {
        transform.position = m_startNode.GetPosition();
    }

    public Node GetPinkyNode()
    {
        if (m_currentDirection == Direction.Directions.UP)
        {
            int t_x = (int)m_node.GetPosition().x;
            int t_y = (int)m_node.GetPosition().y + 2;
            return Grid.m_grid.GetNodeAt(t_x, t_y);
        }
        else if (m_currentDirection == Direction.Directions.DOWN)
        {
            int t_x = (int)m_node.GetPosition().x;
            int t_y = (int)m_node.GetPosition().y - 2;
            return Grid.m_grid.GetNodeAt(t_x, t_y);
        }
        else if (m_currentDirection == Direction.Directions.LEFT)
        {
            int t_x = (int)m_node.GetPosition().x - 2;
            int t_y = (int)m_node.GetPosition().y;
            return Grid.m_grid.GetNodeAt(t_x, t_y);
        }
        else
        {
            int t_x = (int)m_node.GetPosition().x + 2;
            int t_y = (int)m_node.GetPosition().y;
            return Grid.m_grid.GetNodeAt(t_x, t_y);
        }
    }
}