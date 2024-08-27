using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkMovement : IMovementStrategy
{
    public void Move(EnemyController enemyController)
    {
        if (enemyController.GetMotion == EnemyController.AiState.Chase)
        {
            enemyController.GetNavMeshAgent().speed = 3.5f;
            enemyController.GetAnimator().Play(EnemyAnimController.Motion.Run);
        }
        else if (enemyController.GetMotion == EnemyController.AiState.Patrol)
        {
            enemyController.GetNavMeshAgent().speed = 1f;
            enemyController.GetAnimator().Play(EnemyAnimController.Motion.Walk);
        }
    }
}
