using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    PlayerController m_player;
    EnemyController m_enemy;
    SkillData m_skillData;

    public void InitializeEnemy(EnemyController enemy)
    {
        m_enemy = enemy;
    }

    public void InitializePlayer(PlayerController player, SkillData skill)
    {
        m_player = player;
        m_skillData = skill;
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            m_enemy.GetPlayer.SetDamage(m_enemy.GetStatus.attack);
        }

        if (other.CompareTag("Enemy"))
        {
            m_enemy = other.GetComponent<EnemyController>();
            if (m_enemy != null)
            {
                float damage = 0f;
                var status = StatusTable.Instance.GetStatusData(m_enemy.Type);
                DamageType type = m_player.AttackDecision(m_enemy, m_skillData, status, out damage);

                m_enemy.SetDamage(m_skillData, type, damage);

                // ���� �������� ���� Z��ų ������ ��� �� Ȱ��ȭ
                if (m_enemy.GetMotion != EnemyController.AiState.Death && !m_player.IsSkillActive)
                {
                    m_player.PlayerCurSkillGauge = Mathf.Min(100, Mathf.Round(m_player.PlayerCurSkillGauge + damage / 1.5f));
                    m_player.GetPlayerSkillGauge.UpdateGauge(m_player.PlayerCurSkillGauge / m_player.PlayerMaxSkillGauge);
                }
                if (m_player.PlayerCurSkillGauge >= m_player.PlayerMaxSkillGauge)
                {
                    m_player.EnableSkill();
                }
            }
        }
    }
}