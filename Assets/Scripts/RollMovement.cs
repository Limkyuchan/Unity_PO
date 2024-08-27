using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMovement : IMovementStrategy
{
    public void Move(EnemyController enemyController)
    {
        if (enemyController.GetMotion == EnemyController.AiState.Chase || enemyController.GetMotion == EnemyController.AiState.Patrol)
        {
            enemyController.GetAnimator().Play(EnemyAnimController.Motion.Roll);
        }
    }
}
