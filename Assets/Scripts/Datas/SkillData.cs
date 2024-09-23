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

// 0     1      2    3
// 5     10     15   20
// 0     0      5    10
// 0.2   0.3    0.4  0.5   
// 0.3   0.3    0.5  0.5