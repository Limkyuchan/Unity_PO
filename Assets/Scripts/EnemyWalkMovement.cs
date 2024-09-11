using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField]
    EnemyManager.EnemyType m_enemyType;

    public void Move(EnemyController enemy)
    {
        m_enemyType = enemy.Type;
        if (enemy.IsChase)
        {
            enemy.SetState(EnemyController.AiState.Chase);
            enemy.GetNavMeshAgent.speed = 1.7f;
            enemy.GetNavMeshAgent.stoppingDistance = enemy.GetAttackDist;
            enemy.GetAnimator.Play(EnemyAnimController.Motion.Run);
            enemy.IsChase = false;
        }

        if (enemy.IsPatrol)
        {
            enemy.SetState(EnemyController.AiState.Patrol);
            enemy.GetNavMeshAgent.speed = 1f;
            enemy.GetNavMeshAgent.stoppingDistance = enemy.GetNavMeshAgent.radius;
            enemy.GetAnimator.Play(EnemyAnimController.Motion.Walk);
            enemy.IsPatrol = false;
        }
    }
}
