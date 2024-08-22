using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator m_animator;

    public void SetFloat(int animHash, float value)
    {
        m_animator.SetFloat(animHash, value);
    }

    public void Play(int animHash, bool isBlend = true)
    {
        if (isBlend)
        {
            m_animator.SetTrigger(animHash);
        }
        else
        {
            m_animator.Play(animHash, 0, 0f);
        }
    }

    protected virtual void Start()
    {
        m_animator = GetComponent<Animator>();
    }
}
