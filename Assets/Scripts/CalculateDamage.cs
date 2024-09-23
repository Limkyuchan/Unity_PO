using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateDamage
{
    public static bool AttackDecision(float attackerHitRate, float defenserDodgeRate)
    {
        if (attackerHitRate >= 100.0f) return true;

        float hit = Random.Range(0.0f, 100.0f);
        if (hit <= attackerHitRate - defenserDodgeRate)
        {
            return true;
        }
        return false;
    }

    public static float NormalDamage(float attackerAttack, float skillAttack, float defenserDefense)
    {
        float attack = attackerAttack + (attackerAttack * skillAttack) / 100.0f;
        float damage = attack - defenserDefense;
        float value = (damage * 10f) / 100.0f;

        return damage + Random.Range(-value, value);
    }

    public static bool CriticalDecision(float criRate)
    {
        float rate = Random.Range(0.0f, 100.0f);
        if (rate <= criRate)
        {
            return true;
        }
        return false;
    }

    public static float CriticalDamage(float damage, float criAttack)
    {
        return damage + (damage * criAttack) / 100.0f;
    }
}