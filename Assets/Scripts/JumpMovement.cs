using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

public class JumpMovement : IMovementStrategy
{
    public void ChaseMove(EnemyController enemyController)
    {
        enemyController.SetState(AiState.Chase);
        enemyController.GetNavMeshAgent().speed = 3.5f;
        enemyController.GetNavMeshAgent().stoppingDistance = enemyController.GetAttackDist;
        enemyController.GetAnimator().Play(EnemyAnimController.Motion.Run);
    }

    public void PatrolMove(EnemyController enemyController)
    {
        enemyController.SetState(AiState.Jump);
        enemyController.GetNavMeshAgent().enabled = false;
        enemyController.GetAnimator().Play(EnemyAnimController.Motion.Jump);
    }
}
