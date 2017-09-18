using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputHandler : MonoBehaviour
{
    private PacManMovement m_movementScript;

    public int m_playerId;
    private Player m_player;

    // Use this for initialization
    private void Start()
    {
        m_movementScript = GetComponent<PacManMovement>();

        m_player = ReInput.players.GetPlayer(m_playerId);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_movementScript.IsDead())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (m_movementScript.GetPower() == PowerUp.Powers.INVERT)
        {
            InvertedInput();
        }
        else
        {
            NormalInput();
        }
    }

    private void NormalInput()
    {
        if (m_player.GetButtonDown("MoveUp"))
        {
            m_movementScript.SetDirection(Vector3.up, Direction.Directions.UP);
        }
        else if (m_player.GetButtonDown("MoveDown"))
        {
            m_movementScript.SetDirection(Vector3.down, Direction.Directions.DOWN);
        }
        else if (m_player.GetButtonDown("MoveRight"))
        {
            m_movementScript.SetDirection(Vector3.right, Direction.Directions.RIGHT);
        }
        else if (m_player.GetButtonDown("MoveLeft"))
        {
            m_movementScript.SetDirection(Vector3.left, Direction.Directions.LEFT);
        }
    }

    private void InvertedInput()
    {
        if (m_player.GetButtonDown("MoveUp"))
        {
            m_movementScript.SetDirection(Vector3.down, Direction.Directions.DOWN);
        }
        else if (m_player.GetButtonDown("MoveDown"))
        {
            m_movementScript.SetDirection(Vector3.up, Direction.Directions.UP);
        }
        else if (m_player.GetButtonDown("MoveRight"))
        {
            m_movementScript.SetDirection(Vector3.left, Direction.Directions.LEFT);
        }
        else if (m_player.GetButtonDown("MoveLeft"))
        {
            m_movementScript.SetDirection(Vector3.right, Direction.Directions.RIGHT);
        }
    }
}