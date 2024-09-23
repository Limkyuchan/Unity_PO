using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Normal,
    Critical,
    Miss
}

[Serializable]
public struct Status
{
    public int hp;
    public int hpMax;
    public float attack;
    public float defense;
    public float hitRate;
    public float dodgeRate;
    public float criRate;
    public float criAttack;

    public Status(int hp, float attack, float defense, float hitRate, float dodgeRate, float criRate, float criAttack)
    {
        this.hp = hp;
        this.hpMax = hp;
        this.attack = attack;
        this.defense = defense;
        this.hitRate = hitRate;
        this.dodgeRate = dodgeRate;
        this.criRate = criRate;
        this.criAttack = criAttack;
    }
}