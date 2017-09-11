using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghosts
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
                if (PacManMovement.m_pacman.GetPinkyNode())
                {
                    m_targetNode = PacManMovement.m_pacman.GetPinkyNode();
                }
                else
                {
                    m_targetNode = PacManMovement.m_pacman.GetCurrentNode();
                }
                if (m_timer >= m_chaseTime)
                {
                    ChangeMode(GhostMode.SCATTER);
                    m_timer = 0;
                }
                break;
        }
    }
}