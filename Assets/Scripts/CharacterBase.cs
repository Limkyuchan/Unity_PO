using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public abstract void SetDamage(float damage, EnemyController enemy);
    public abstract void SetDamage(SkillData skill, DamageType type, float damage);
    public abstract Transform GetTransform();
}