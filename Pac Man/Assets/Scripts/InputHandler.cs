using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PacManMovement m_movementScript;

    // Use this for initialization
    private void Start()
    {
        m_movementScript = GetComponent<PacManMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_movementScript.SetDirection(Vector3.up, Direction.Directions.UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_movementScript.SetDirection(Vector3.down, Direction.Directions.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_movementScript.SetDirection(Vector3.right, Direction.Directions.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_movementScript.SetDirection(Vector3.left, Direction.Directions.LEFT);
        }
    }
}