using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private GameObject m_object;

    private Vector3 m_position;

    private bool m_walkable;

    private bool m_walkableForGhosts;

    private bool m_partOfGhostHouse;

    private string m_name;

    private bool m_intersection;

    public void SetUpNode(GameObject _object, Vector3 _position, bool _walkable, bool _walkableGhosts, string _name, bool _intersection, bool _partOfGhostHouse)
    {
        m_object = _object;
        m_position = _position;
        m_walkable = _walkable;
        m_walkableForGhosts = _walkableGhosts;
        m_name = _name;
        m_intersection = _intersection;
        m_partOfGhostHouse = _partOfGhostHouse;
    }

    public GameObject GetObject()
    {
        return m_object;
    }

    public void SetObject(GameObject _object)
    {
        m_object = _object;
    }

    public void RemoveObject()
    {
        m_object.GetComponent<Collider2D>().enabled = false;
        m_object.GetComponent<SpriteRenderer>().enabled = false;
    }

    public Vector3 GetPosition()
    {
        return m_position;
    }

    public bool IsWalkable()
    {
        return m_walkable;
    }

    public bool IsWalkableForGhosts()
    {
        return m_walkableForGhosts;
    }

    public string GetName()
    {
        return m_name;
    }

    public bool IsIntersection()
    {
        return m_intersection;
    }

    public bool IsPartOfGhostHouse()
    {
        return m_partOfGhostHouse;
    }
}