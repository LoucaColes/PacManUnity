using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMovement : MonoBehaviour
{
    private Vector3 m_direction;
    public float m_moveSpeed;
    private float m_origSpeed;

    private Direction.Directions m_currentDirection;

    private Animator m_animator;

    [SerializeField]
    private Node m_node;

    private Node m_startNode;

    private Node m_pinkyNode;

    private Score m_scoreScript;

    private Lives m_lives;

    public static PacManMovement m_pacman;

    private float m_poweredTime;
    private float m_poweredTimer;

    private bool m_powered;

    private PowerUp.Powers m_currentPower;

    private SpriteRenderer m_renderer;

    public float m_resetTime;

    private bool m_dead;
    private bool m_enabled;

    // Use this for initialization
    protected void Start()
    {
        m_direction = Vector3.zero;
        m_currentDirection = Direction.Directions.STOP;
        m_animator = GetComponent<Animator>();
        m_scoreScript = GetComponent<Score>();
        m_renderer = GetComponent<SpriteRenderer>();

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
        m_origSpeed = m_moveSpeed;
        m_poweredTimer = 0;
        m_powered = false;
        m_currentPower = PowerUp.Powers.COUNT;
        m_dead = false;
        m_enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        MovementAnimationCheck();
        Powered();
    }

    private void Movement()
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
    }

    private void Powered()
    {
        if (m_powered)
        {
            m_poweredTimer += Time.deltaTime;
            if (m_poweredTimer >= m_poweredTime)
            {
                switch (m_currentPower)
                {
                    case PowerUp.Powers.FAST:
                    case PowerUp.Powers.SLOW:
                        m_moveSpeed = m_origSpeed;
                        break;

                    case PowerUp.Powers.INVISIBLE:
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
        if (!m_dead)
        {
            switch (m_currentDirection)
            {
                case Direction.Directions.RIGHT:
                    m_animator.SetBool("Right", true);
                    m_animator.SetBool("Up", false);
                    m_animator.SetBool("Left", false);
                    m_animator.SetBool("Down", false);
                    m_animator.SetBool("Dead", false);
                    break;

                case Direction.Directions.LEFT:
                    m_animator.SetBool("Right", false);
                    m_animator.SetBool("Up", false);
                    m_animator.SetBool("Left", true);
                    m_animator.SetBool("Down", false);
                    m_animator.SetBool("Dead", false);
                    break;

                case Direction.Directions.UP:
                    m_animator.SetBool("Right", false);
                    m_animator.SetBool("Up", true);
                    m_animator.SetBool("Left", false);
                    m_animator.SetBool("Down", false);
                    m_animator.SetBool("Dead", false);
                    break;

                case Direction.Directions.DOWN:
                case Direction.Directions.STOP:
                    m_animator.SetBool("Right", false);
                    m_animator.SetBool("Up", false);
                    m_animator.SetBool("Left", false);
                    m_animator.SetBool("Down", true);
                    m_animator.SetBool("Dead", false);
                    break;
            }
        }
        else
        {
            m_animator.SetBool("Right", false);
            m_animator.SetBool("Up", false);
            m_animator.SetBool("Left", false);
            m_animator.SetBool("Down", false);
            m_animator.SetBool("Dead", true);
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
            GameManager.m_gameManager.UpdatePelletCount(-1);
            if (t_node.GetName().Contains("Power"))
            {
                PowerPill(collision);
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
        if (collision.tag == "Ghost" && collision.GetComponent<Ghosts>().GetCurrentMode() != Ghosts.GhostMode.FRIGHTENED && m_currentPower != PowerUp.Powers.INVINCIBLE)
        {
            StartCoroutine("PacManReset");
        }
        else if (collision.tag == "Ghost" && collision.GetComponent<Ghosts>().GetCurrentMode() == Ghosts.GhostMode.FRIGHTENED)
        {
            collision.GetComponent<Ghosts>().ResetPosition();
            collision.GetComponent<Ghosts>().ChangeMode(Ghosts.GhostMode.EXIT);
            m_scoreScript.UpdateScore(collision.GetComponent<Scoreable>().GetScoreValue());
        }
    }

    public void ResetPostion()
    {
        transform.position = m_startNode.GetPosition();
        m_currentDirection = Direction.Directions.STOP;
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

    public bool IsPowered()
    {
        return m_powered;
    }

    public PowerUp.Powers GetPower()
    {
        return m_currentPower;
    }

    private void PowerPill(Collider2D _collision)
    {
        GameObject[] t_ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        for (int i = 0; i < t_ghosts.Length; i++)
        {
            t_ghosts[i].GetComponent<Ghosts>().ResetTimer();
            t_ghosts[i].GetComponent<Ghosts>().ChangeMode(Ghosts.GhostMode.FRIGHTENED);
        }

        if (_collision.gameObject.GetComponent<PowerUp>())
        {
            PowerUp t_powerUpScript = _collision.gameObject.GetComponent<PowerUp>();
            m_currentPower = t_powerUpScript.GetPower();
            m_poweredTime = t_powerUpScript.GetTime();
            GameManager.m_gameManager.GetGameHud().UpdatePowerText(m_currentPower.ToString());
            PowerSetUp(t_ghosts);
            m_poweredTimer = 0;
            m_powered = true;
        }
    }

    private void PowerSetUp(GameObject[] _ghosts)
    {
        switch (m_currentPower)
        {
            case PowerUp.Powers.FAST:
                m_moveSpeed = m_moveSpeed * 2;
                m_poweredTime *= 2;
                break;

            case PowerUp.Powers.SLOW:
                m_moveSpeed = m_moveSpeed / 2;
                m_poweredTime *= 2;
                break;

            case PowerUp.Powers.INVISIBLE:
                m_renderer.enabled = false;
                m_animator.enabled = false;
                m_poweredTime *= 3;
                break;

            case PowerUp.Powers.INVINCIBLE:
                m_poweredTime *= 3;
                break;

            case PowerUp.Powers.FASTGHOST:
            case PowerUp.Powers.INVISGHOST:
            case PowerUp.Powers.SLOWGHOST:
                for (int i = 0; i < _ghosts.Length; i++)
                {
                    _ghosts[i].GetComponent<Ghosts>().SetPowered(m_currentPower, m_poweredTime);
                }
                m_currentPower = PowerUp.Powers.COUNT;
                break;
        }
    }

    public void KillPacMan()
    {
        StartCoroutine("PacManReset");
    }

    private IEnumerator PacManReset()
    {
        m_dead = true;
        m_lives.UpdateLives(-1);
        m_currentDirection = Direction.Directions.STOP;
        m_direction = Vector3.zero;
        if (GameManager.m_gameManager.GetState() != GameManager.GameState.MAIN)
        {
            GameObject[] t_ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            for (int i = 0; i < t_ghosts.Length; i++)
            {
                t_ghosts[i].GetComponent<Ghosts>().ResetPosition();
                t_ghosts[i].GetComponent<Ghosts>().ChangeMode(Ghosts.GhostMode.WAIT);
                t_ghosts[i].GetComponent<Ghosts>().ResetTimer();
                t_ghosts[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        yield return new WaitForSeconds(m_resetTime);
        ResetPostion();
        m_dead = false;
        yield return new WaitForSeconds(m_resetTime);
        if (GameManager.m_gameManager.GetState() != GameManager.GameState.MAIN)
        {
            GameObject[] t_ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            for (int i = 0; i < t_ghosts.Length; i++)
            {
                t_ghosts[i].GetComponent<Ghosts>().ChangeMode(Ghosts.GhostMode.WAIT);
                t_ghosts[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    public bool IsDead()
    {
        return m_dead;
    }

    public void DisablePacMan()
    {
        m_renderer.enabled = false;
        m_dead = true;
        m_enabled = false;
    }

    public void EnablePacMan()
    {
        m_renderer.enabled = true;
        m_dead = false;
        m_enabled = true;
    }

    public bool IsEnabled()
    {
        return m_enabled;
    }
}