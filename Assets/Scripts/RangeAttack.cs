using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : IAttackStrategy
{
    public void Attack(EnemyController enemy)
    {
        enemy.SetState(EnemyController.AiState.Attack);
        enemy.transform.LookAt(enemy.GetPlayer.transform);
        enemy.GetAnimator().Play(EnemyAnimController.Motion.Attack1);

        if (enemy.IsEnemyAttack)
        {

            enemy.IsEnemyAttack = false;
        }
    }
}

// ���Ÿ� ���� �ֵ�
// ����Ʈ ������ �ҷ�����
//if (CheckArea(m_player.transform, m_attackDist))
//{
//    var dir = m_player.transform.position - m_dummyFire.position;
//    dir.y = 0f;
//    var effect = 1;
//    effect.transform.position = m_dummyFire.position;
//    effect.transform.forward = dir.normalized;
//}
