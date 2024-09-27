using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    None = -1,
    Normal,
    Critical,
    Miss
}

[Serializable]
public struct StatusData
{
    public EnemyManager.EnemyType type;
    public int hp;
    public int hpMax;
    public float attack;
    public float defense;
    public float hitRate;
    public float dodgeRate;
    public float criRate;
    public float criAttack;
    public float attackDist;
    public float detectDist;

    public StatusData(EnemyManager.EnemyType type, int hp, int hpMax, float attack, float defense, float hitRate, float dodgeRate, float criRate, float criAttack, float attackDist, float detectDist)
    {
        this.type = type;
        this.hp = hp;
        this.hpMax = hpMax;
        this.attack = attack;
        this.defense = defense;
        this.hitRate = hitRate;
        this.dodgeRate = dodgeRate;
        this.criRate = criRate;
        this.criAttack = criAttack;
        this.attackDist = attackDist;
        this.detectDist = detectDist;
    }
}