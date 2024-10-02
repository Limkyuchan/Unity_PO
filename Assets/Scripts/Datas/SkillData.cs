using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Debuff
{
    None = -1,
    Stun,
    Knockdown
}

[Serializable]
public struct SkillData
{
    public PlayerAnimController.Motion skillMotion;
    public int effectId;
    public int attackArea;
    public float attack;
    public float hitRate;
    public float knockback;
    public float knockbackDuration;
    public Debuff debuff;
    public float debuffDuration;
}