using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

// Excel 데이터 수정!!
// Attack1  1(FX_Hit_01)            0...
// Attack2  1(FX_Hit_01)            1...
// Attack3  3(FX_AttackCritical_01) 2...
// Attack4  2(FX_Attack01_01)       3...

// FX_Attack01_01
// FX_AttackCritical_01
// FX_Hit_01