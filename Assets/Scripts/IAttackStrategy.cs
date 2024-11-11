using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStrategy
{
    //public void Attack(EnemyController enemy);
    public void Attack(CharacterBase target);
}