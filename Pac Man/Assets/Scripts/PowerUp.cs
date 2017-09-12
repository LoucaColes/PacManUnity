using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Powers
    {
        INVISIBLE,
        INVINCIBLE,
        FAST,
        SLOW,
        INVERT,
        INVISGHOST,
        FASTGHOST,
        SLOWGHOST,
        COUNT
    }

    public float m_powerUpTime;

    public Powers m_power;

    public Powers GetPower()
    {
        return m_power;
    }

    public void SetPower(Powers _newPower)
    {
        m_power = _newPower;
    }

    public void SetPower()
    {
        m_power = (Powers)Random.Range(0, (int)Powers.COUNT);
    }

    public float GetTime()
    {
        return m_powerUpTime;
    }

    public void SetPowerTime(float _newTime)
    {
        m_powerUpTime = _newTime;
    }
}