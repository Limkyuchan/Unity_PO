using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack : MonoBehaviour, IAttackStrategy
{
    public void Attack(EnemyController enemy)
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
            if (enemy.GetUnitFind.PlayerUnitList != null)
            {
                enemy.GetPlayer.SetDamage(enemy.GetStatus.attack);
            }
        } 
    }
}
