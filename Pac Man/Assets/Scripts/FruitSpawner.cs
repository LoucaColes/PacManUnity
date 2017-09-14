using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] m_fruits;
    private GameObject m_currentFruit;

    public float m_waitTime;
    private float m_waitTimer;

    public float m_lifeTime;
    private float m_lifeTimer;

    public float m_flashTime;
    private float m_flashTimer;

    private Node m_node;

    // Use this for initialization
    private void Start()
    {
        m_waitTimer = 0;
        m_lifeTimer = 0;
        m_flashTimer = 0;

        m_currentFruit = null;
        m_node = GetComponent<Node>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_currentFruit)
        {
            StartSpawnProcess();
        }
        else if (m_currentFruit)
        {
        }
    }

    private void StartSpawnProcess()
    {
        m_waitTimer += Time.deltaTime;
        if (m_waitTimer >= m_waitTime)
        {
            SpawnFruit();
            m_waitTimer = 0;
        }
    }

    private void SpawnFruit()
    {
        m_currentFruit = (GameObject)Instantiate(m_fruits[GameManager.m_gameManager.GetLevel()], transform.position, Quaternion.identity);
        m_node.SetObject(m_currentFruit);
    }

    private void StartDecayProcess()
    {
        m_lifeTimer += Time.deltaTime;
        if (m_lifeTimer >= m_lifeTime / 2)
        {
            HalfLifeTime();
        }
        if (m_lifeTimer >= m_lifeTime || GameManager.m_gameManager.GetState() == GameManager.GameState.MAIN)
        {
            EndOfLife();
        }
    }

    private void HalfLifeTime()
    {
        m_flashTimer += Time.deltaTime;
        if (m_flashTimer >= m_flashTime)
        {
            m_currentFruit.GetComponent<SpriteRenderer>().enabled = !m_currentFruit.GetComponent<SpriteRenderer>().enabled;
            m_flashTimer = 0;
        }
    }

    private void EndOfLife()
    {
        Destroy(m_currentFruit);
        m_flashTimer = 0;
        m_lifeTimer = 0;
        m_waitTimer = 0;
        m_node.SetObject(this.gameObject);
    }
}