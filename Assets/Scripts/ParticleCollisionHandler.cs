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
        if (other.CompareTag("Player"))
        {
            m_enemy.GetPlayer.SetDamage();
        }
    }
}