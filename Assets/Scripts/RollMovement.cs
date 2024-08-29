using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMovement : IMovementStrategy
{
    public void Move(EnemyController enemy)
    {
        if (enemy.IsChase)
        {
            enemy.SetState(EnemyController.AiState.Chase);
            enemy.GetNavMeshAgent().speed = 3f;
            enemy.GetNavMeshAgent().stoppingDistance = enemy.GetAttackDist;
            enemy.GetAnimator().Play(EnemyAnimController.Motion.Run);
            enemy.IsChase = false;
        }

        if (enemy.IsPatrol)
        {
            enemy.SetState(EnemyController.AiState.Patrol);
            enemy.GetNavMeshAgent().speed = 0.01f;
            enemy.GetNavMeshAgent().stoppingDistance = enemy.GetNavMeshAgent().radius;
            enemy.GetAnimator().Play(EnemyAnimController.Motion.Roll);
            enemy.IsPatrol = false;
        }
    }
}