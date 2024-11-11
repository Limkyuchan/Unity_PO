using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack : MonoBehaviour, IAttackStrategy
{
    //public void Attack(EnemyController enemy)
    //{
    //    if (enemy.GetPlayer.PlayerCurHp <= 0)
    //    {
    //        enemy.IsEnemyAttack = false;
    //        enemy.SetState(EnemyController.AiState.Idle);
    //        return;
    //    }

    //    if (!enemy.IsEnemyAttack)
    //    {
    //        enemy.SetState(EnemyController.AiState.Attack);
    //        enemy.transform.LookAt(enemy.GetPlayer.transform);
    //        enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack1);
    //    }
    //    else if (enemy.IsEnemyAttack)
    //    {
    //        if (enemy.GetUnitFind.PlayerUnitList != null)
    //        {
    //            enemy.GetPlayer.SetDamage(enemy.GetStatus.attack);
    //        }
    //    } 
    //}

    AttackAreaUnitFind m_attackArea;
    AttackAreaUnitFind[] m_attackAreas;

    [SerializeField]
    GameObject m_attackAreaObj;
    List<GameObject> m_enemyList = new List<GameObject>();

    public void Attack(CharacterBase target)
    {
        if (target is EnemyController enemy)
        {
            if (enemy.GetPlayer.PlayerCurHp <= 0)
            {
                enemy.IsEnemyAttack = false;
                enemy.SetState(EnemyController.AiState.Idle);
                return;
            }

            if (!enemy.IsEnemyAttack)
            {
                enemy.SetState(EnemyController.AiState.Attack);
                enemy.transform.LookAt(enemy.GetPlayer.transform);
                enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack1);
            }
            else if (enemy.IsEnemyAttack)
            {
                if (m_attackArea.PlayerUnitList != null)
                {
                    enemy.GetPlayer.SetDamage(enemy.GetStatus.attack);
                }
            }
        }
        else if (target is PlayerController player)
        {
            var skill = SkillTable.Instance.GetSkillData(player.GetMotion);
            var unitList = m_attackAreas[skill.attackArea].EnemyUnitList;
            var effectData = EffectTable.Instance.GetData(skill.effectId);

            if (skill.attack == 0) return;      // skill Data 못 가져오면 return
            m_enemyList.Clear();
            foreach (var unit in unitList)      // unitList에 적 추가
            {
                if (unit != null)
                {
                    m_enemyList.Add(unit);
                }
            }

            DamageType type = DamageType.Miss;
            float damage = 0f;

            for (int i = m_enemyList.Count - 1; i >= 0; i--)
            {
                var enemyObj = m_enemyList[i];
                if (enemyObj == null)
                {
                    m_enemyList.RemoveAt(i);
                    continue;
                }

                var checkEnemy = m_enemyList[i].GetComponent<EnemyController>();
                if (checkEnemy == null) continue;

                var status = StatusTable.Instance.GetStatusData(checkEnemy.Type);
                type = player.AttackDecision(checkEnemy, skill, status, out damage);
                checkEnemy.SetDamage(skill, type, damage);

                if (type != DamageType.Miss)
                {
                    // 공격 데미지에 따른 Z스킬 게이지 계산 및 활성화
                    if (checkEnemy.GetMotion != EnemyController.AiState.Death && !player.IsSkillActive)
                    {
                        player.PlayerCurSkillGauge = Mathf.Min(100, Mathf.Round(player.PlayerCurSkillGauge + damage / 2.5f));
                        player.GetPlayerSkillGauge.UpdateGauge(player.PlayerCurSkillGauge / player.PlayerMaxSkillGauge);
                    }
                    if (player.PlayerCurSkillGauge >= player.PlayerMaxSkillGauge)
                    {
                        player.EnableSkill();
                    }

                    // 공격 이팩트 효과 적용
                    var effect = EffectPool.Instance.Create(effectData.Prefabs[type == DamageType.Normal ? 0 : 1]);
                    effect.transform.position = checkEnemy.transform.position + Vector3.up * 0.6f;
                    var dir = transform.position - effect.transform.position;
                    dir.y = 0f;
                    effect.transform.rotation = Quaternion.FromToRotation(effect.transform.forward, dir.normalized);
                }
            }
        }
    }

    void Start()
    {
        m_attackArea = m_attackAreaObj.GetComponentInChildren<AttackAreaUnitFind>();
        m_attackAreas = m_attackAreaObj.GetComponentsInChildren<AttackAreaUnitFind>();
    }
}