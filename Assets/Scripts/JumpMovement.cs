using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField]
    EnemyManager.EnemyType m_enemyType;

    public void Move(EnemyController enemy)
    {
        m_enemyType = enemy.Type;
        if (enemy.IsChase)
        {
            enemy.IsPatrol = false;
            enemy.SetState(EnemyController.AiState.Chase);
            enemy.GetNavMeshAgent.speed = 3.5f;
            enemy.GetNavMeshAgent.stoppingDistance = enemy.GetAttackDist;
            enemy.GetAnimator.Play(EnemyAnimController.Motion.Run);
            enemy.IsChase = false;
        }

        if (enemy.IsPatrol)
        {
            enemy.SetState(EnemyController.AiState.Jump);
            enemy.GetNavMeshAgent.enabled = false;
            enemy.GetAnimator.Play(EnemyAnimController.Motion.Jump);
        }
    }
}
