using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackStrategy
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

    [SerializeField]
    GameObject m_attackAreaObj;

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
                //if (enemy.GetUnitFind.PlayerUnitList != null)
                //{
                //    enemy.GetPlayer.SetDamage(enemy.GetStatus.attack);
                //}
                if (m_attackArea.PlayerUnitList != null)
                {
                    enemy.GetPlayer.SetDamage(enemy.GetStatus.attack);
                }
            }
        }
    }

    void Start()
    {
        m_attackArea = m_attackAreaObj.GetComponentInChildren<AttackAreaUnitFind>();
    }
}