using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementStrategy 
{
    public void Move(EnemyController enemyController);
}
