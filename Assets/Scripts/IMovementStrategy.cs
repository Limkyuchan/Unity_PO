using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IMovementStrategy 
{
    public void Move(EnemyController enemyController);
}
