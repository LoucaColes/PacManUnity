using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghosts
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
                int t_rand = Random.Range(0, 100);
                if (t_rand < 50)
                {
                    m_targetNode = PacManMovement.m_pacman.GetCurrentNode();
                }
                else
                {
                    if (PacManMovement.m_pacman.GetPinkyNode())
                    {
                        m_targetNode = PacManMovement.m_pacman.GetPinkyNode();
                    }
                    else
                    {
                        m_targetNode = PacManMovement.m_pacman.GetCurrentNode();
                    }
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