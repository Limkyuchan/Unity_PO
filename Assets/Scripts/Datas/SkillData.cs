using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SkillData
{
    public PlayerAnimController.Motion skillMotion;
    public int attackArea;
    public float attack;
    public float hitRate;
    public float knockback;
    public float knockbackDuration;
}