using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaUnitFind : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_unitList = new List<GameObject>();

    public List<GameObject> UnitList { get { return m_unitList; } }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_unitList.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_unitList.Remove(other.gameObject);
        }
    }

    void Start()
    {
        m_unitList.Clear();
    }
}
