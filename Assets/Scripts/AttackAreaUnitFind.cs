using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaUnitFind : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_enemyUnitList = new List<GameObject>();
    [SerializeField]
    GameObject m_playerUnitList;

    public List<GameObject> EnemyUnitList { get { return m_enemyUnitList; } }

    public GameObject PlayerUnitList { get { return m_playerUnitList; } }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_enemyUnitList.Add(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            m_playerUnitList = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_enemyUnitList.Remove(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            m_playerUnitList = null;
        }
    }

    void Start()
    {
        m_enemyUnitList.Clear();
        m_playerUnitList = null;
    }
}
