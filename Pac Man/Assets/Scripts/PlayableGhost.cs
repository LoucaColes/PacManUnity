using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayableGhost : Ghosts
{
    private Vector3 m_direction;

    public int m_playerId;
    private Player m_player;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        m_player = ReInput.players.GetPlayer(m_playerId);
    }

    // Update is called once per frame
    protected override void Update()
    {
        ModeUpdate();
        base.MovementAnimationCheck();
        Input();
        Movement();
    }

    public void SetDirection(Vector3 _direction, Direction.Directions _newDirectionState)
    {
        m_direction = _direction;
        m_currentDirection = _newDirectionState;
    }

    private void Input()
    {
        if (m_player.GetButtonDown("MoveUp"))
        {
            SetDirection(Vector3.up, Direction.Directions.UP);
        }
        else if (m_player.GetButtonDown("MoveDown"))
        {
            SetDirection(Vector3.down, Direction.Directions.DOWN);
        }
        else if (m_player.GetButtonDown("MoveRight"))
        {
            SetDirection(Vector3.right, Direction.Directions.RIGHT);
        }
        else if (m_player.GetButtonDown("MoveLeft"))
        {
            SetDirection(Vector3.left, Direction.Directions.LEFT);
        }
    }

    private void Movement()
    {
        if (m_direction != Vector3.zero)
        {
            Node t_nextNode = GetNextNode();
            if ((t_nextNode && t_nextNode.IsWalkableForGhosts()))
            {
                transform.position = Vector3.MoveTowards(transform.position, t_nextNode.GetPosition(), m_moveSpeed * Time.deltaTime);
                if (t_nextNode.GetComponent<TeleportPoint>())
                {
                    t_nextNode.GetComponent<TeleportPoint>().Teleport(gameObject, m_direction);
                }
            }
            else
            {
                Vector3 t_endPos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);
                transform.position = Vector3.MoveTowards(transform.position, t_endPos, m_moveSpeed * Time.deltaTime);
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
}