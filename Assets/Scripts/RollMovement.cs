using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

public class RollMovement : IMovementStrategy
{
    public void ChaseMove(EnemyController enemyController)
    {
        enemyController.SetState(AiState.Chase);
        enemyController.GetNavMeshAgent().speed = 0.01f;
        enemyController.GetNavMeshAgent().stoppingDistance = enemyController.GetAttackDist;
        enemyController.GetAnimator().Play(EnemyAnimController.Motion.Roll);
    }

    public void PatrolMove(EnemyController enemyController)
    {
        enemyController.SetState(AiState.Patrol);
        enemyController.GetNavMeshAgent().speed = 0.01f;
        enemyController.GetNavMeshAgent().stoppingDistance = enemyController.GetNavMeshAgent().radius;
        enemyController.GetAnimator().Play(EnemyAnimController.Motion.Roll);
    }
}