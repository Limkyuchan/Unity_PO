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

            if (enemy.Type == EnemyManager.EnemyType.BossMonster)
            {
                Debug.Log("보스 몬스터 공격호출");
                if (!enemy.IsEnemyAttack)
                {
                    enemy.SetState(EnemyController.AiState.Attack);
                    enemy.transform.LookAt(enemy.GetPlayer.transform);
                    BossAttack(enemy);
                }
                else if (enemy.IsEnemyAttack)
                {
                    if (m_attackArea.PlayerUnitList != null)
                    {
                        enemy.GetPlayer.SetDamage(enemy.GetStatus.attack, enemy);
                    }
                }
            }
            else
            {
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
            //if (enemy.Type == EnemyManager.EnemyType.BossMonster)
            //{
            //    BossAttack(enemy);
            //    return;
            //}

            //if (enemy.GetPlayer.PlayerCurHp <= 0)
            //{
            //    enemy.IsEnemyAttack = false;
            //    enemy.SetState(EnemyController.AiState.Idle);
            //    return;
            //}

            //if (!enemy.IsEnemyAttack)
            //{
            //    enemy.SetState(EnemyController.AiState.Attack);
            //    enemy.transform.LookAt(enemy.GetPlayer.transform);
            //    enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack1);
            //}
            //else if (enemy.IsEnemyAttack)
            //{
            //    if (m_attackArea.PlayerUnitList != null)
            //    {
            //        enemy.GetPlayer.SetDamage(enemy.GetStatus.attack, enemy);
            //    }
            //}
        }
    }

    public void BasicAttack(CharacterBase target) { }

    public void SkillAttack_1(CharacterBase target) { }

    public void SkillAttack_2(CharacterBase target) { }

    void BossAttack(EnemyController enemy)
    {
        Debug.Log("보스 몬스터 랜덤공격 :" + enemy.Type);
        int randomAttack = Random.Range(1, 4);
        switch (randomAttack)
        {
            case 1:
                enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack1);
                break;
            case 2:
                enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack2);
                break;
            case 3:
                enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack3);
                break;
        }
    }

    void Start()
    {
        m_attackArea = m_attackAreaObj.GetComponentInChildren<AttackAreaUnitFind>();
    }
}