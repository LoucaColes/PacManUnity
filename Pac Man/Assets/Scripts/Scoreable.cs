using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreable : MonoBehaviour
{
    public int m_scoreValue;

    public int GetScoreValue()
    {
        return m_scoreValue;
    }

    public void SetScoreValue(int _newValue)
    {
        m_scoreValue = _newValue;
    }
}