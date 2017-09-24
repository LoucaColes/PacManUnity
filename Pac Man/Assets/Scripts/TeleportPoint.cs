using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField]
    private Node m_nextNode;

    public void SetNextNode(Node _nextNode)
    {
        m_nextNode = _nextNode;
    }

    public Node GetNextNode()
    {
        return m_nextNode;
    }

    public void Teleport(GameObject _object)
    {
        _object.transform.position = m_nextNode.GetPosition();
    }

    public void Teleport(GameObject _object, Vector3 _direction)
    {
        _object.transform.position = m_nextNode.GetPosition() + _direction;
    }
}