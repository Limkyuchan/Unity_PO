using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : IMovementStrategy
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
            enemy.GetNavMeshAgent().speed = 1f;
            enemy.GetNavMeshAgent().stoppingDistance = enemy.GetNavMeshAgent().radius;
            enemy.GetAnimator().Play(EnemyAnimController.Motion.Walk);
            enemy.IsPatrol = false;
        }
    }
}
