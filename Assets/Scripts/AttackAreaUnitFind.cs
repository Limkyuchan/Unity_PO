using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaUnitFind : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_enemyUnitList = new List<GameObject>();

    public List<GameObject> EnemyUnitList { get { return m_enemyUnitList; } }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_enemyUnitList.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_enemyUnitList.Remove(other.gameObject);
        }
    }

    void Start()
    {
        m_enemyUnitList.Clear();
    }
}
