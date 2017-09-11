using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public enum Directions
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
        STOP
    }

    public bool CompareDirections(Directions _a, Directions _b)
    {
        if (_a == _b)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}