using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonDontDestroy<PlayerStatus>
{
    public enum PlayerType
    {
        Warrior,
        Range
    }

    public PlayerType playerType;
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
    public string playerName;
    public string playerWeapon;

    public void InitializeStatus(string name, string weapon, PlayerType type)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = name;
            playerWeapon = weapon;
            playerType = type;

            SetPlayerStatus();
        }
    }

    public void UpdateStatus(int currentHp, float currentAttack, float currentSkillGauge)
    {
        hp = currentHp;
        attack = currentAttack;
        skillGauge = currentSkillGauge;
        deathEnemyCnt = 0;
        totalEnemyCnt = 0;
    }

    void SetPlayerStatus()
    {
        switch (playerType)
        {
            case PlayerType.Warrior:
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
                break;

            case PlayerType.Range:
                hp = 180;              
                hpMax = 180;
                attack = 25;           
                defense = 8; 
                hitRate = 95;
                criRate = 12;
                criAttack = 150;
                skillGauge = 0;
                deathEnemyCnt = 0;
                totalEnemyCnt = 0;
                break;
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
    }
}