using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCollisionHandler : MonoBehaviour
{
    PlayerController m_player;
    EnemyController m_enemy;
    SkillData m_skillData;

    public void InitializePlayer(PlayerController player, SkillData skill)
    {
        m_player = player;
        m_skillData = skill;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_enemy = other.GetComponent<EnemyController>();
            if (m_enemy != null)
            {
                float damage = 0f;
                var status = StatusTable.Instance.GetStatusData(m_enemy.Type);
                DamageType damageType = m_player.AttackDecision(m_enemy, m_skillData, status, out damage);

                m_enemy.SetDamage(m_skillData, damageType, damage);

                Debug.Log("HIHI");
            }
        }
    }
}