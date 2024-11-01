using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonDontDestroy<PlayerStatus>
{
    public int hp;
    public int hpMax;
    public int deathEnemyCnt;
    public int totalEnemyCnt;
    public float attack;
    public float defense;
    public float hitRate;
    public float criRate;
    public float criAttack;
    public float skillGauge;

    public void UpdateStatus(int currentHp, float currentAttack, float currentSkillGauge)
    {
        hp = currentHp;
        attack = currentAttack;
        skillGauge = currentSkillGauge;
        deathEnemyCnt = 0;
        totalEnemyCnt = 0;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        hp = 200;
        hpMax = 200;
        attack = 30;
        defense = 10;
        hitRate = 90;
        criRate = 10;
        criAttack = 150;
        skillGauge = 0;
        deathEnemyCnt = 0;
        totalEnemyCnt = 0;
    }
}