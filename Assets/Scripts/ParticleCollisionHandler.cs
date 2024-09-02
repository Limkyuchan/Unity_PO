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
        Debug.Log("진입!!");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player 맞았음!");
            m_enemy.GetPlayer.SetDamage(m_enemy);
        }
    }
}