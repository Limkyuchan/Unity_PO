using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    EnemyController m_enemy;

    public void Initialize(EnemyController enemy)
    {
        m_enemy = enemy;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("����!!");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player �¾���!");
            m_enemy.GetPlayer.SetDamage(m_enemy);
        }
    }
}