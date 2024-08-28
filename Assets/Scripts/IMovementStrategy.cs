using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementStrategy 
{
    public void ChaseMove(EnemyController enemyController);

    public void PatrolMove(EnemyController enemyController);
}
