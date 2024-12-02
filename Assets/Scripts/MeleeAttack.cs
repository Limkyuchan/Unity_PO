using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackStrategy
{
    [SerializeField]
    GameObject m_attackAreaObj;

    AttackAreaUnitFind m_attackArea;

    public void AnimEvent_Attack(CharacterBase target)
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
                    enemy.GetPlayer.SetDamage(enemy.GetStatus.attack, enemy);
                }
            }
        }
    }

    public void BasicAttack(CharacterBase target) { }

    public void SkillAttack_1(CharacterBase target) { }

    public void SkillAttack_2(CharacterBase target) { }

    void Start()
    {
        m_attackArea = m_attackAreaObj.GetComponentInChildren<AttackAreaUnitFind>();
    }
}