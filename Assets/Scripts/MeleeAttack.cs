using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MeleeAttack : IAttackStrategy
{
    public void Attack(EnemyController enemy)
    {
        if (!enemy.IsEnemyAttack)
        {
            enemy.SetState(EnemyController.AiState.Attack);
            enemy.transform.LookAt(enemy.GetPlayer.transform);
            enemy.GetAnimator().Play(EnemyAnimController.Motion.Attack1);
        }
        else if (enemy.IsEnemyAttack)
        {
            if (enemy.GetUnitFind.PlayerUnitList != null)
            {
                enemy.GetPlayer.SetDamage(enemy);
            }
        }
    }
}