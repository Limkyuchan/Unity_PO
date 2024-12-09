using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStrategy
{
    public void AnimEvent_Attack(CharacterBase target);

    public void BasicAttack(CharacterBase target);

    public void SkillAttack_1(CharacterBase target);

    public void SkillAttack_2(CharacterBase target);
}