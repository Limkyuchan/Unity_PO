using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : AnimationController
{
    public enum Motion
    {
        None = -1,
        Victory,
        ShowSkill,
        Idle,
        Locomotion,
        Hit,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        RangeAttack,
        Skill1,
        Skill2,
        Skill3,
        Shield,
        Death,
        Max
    }

    [SerializeField]
    Motion m_curMotion;

    public Motion GetMotion { get { return m_curMotion; } }
    Dictionary<Motion, int> m_motionHashTable = new Dictionary<Motion, int>();

    public void Play(Motion motion, bool isBlend = true)
    {
        m_curMotion = motion;
        Play(m_motionHashTable[motion], isBlend);
    }

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < (int)Motion.Max; i++)
        {
            var motion = (Motion)i;
            m_motionHashTable.Add(motion, Animator.StringToHash(motion.ToString()));
        }
    }
}