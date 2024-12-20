using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HittedFeedback : MonoBehaviour
{
    [SerializeField]
    AnimationCurve m_curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    Vector3 m_from;
    [SerializeField]
    Vector3 m_to;
    [SerializeField]
    float m_duration = 1f;

    NavMeshAgent m_navAgent;
    CharacterController m_characterController;

    IEnumerator CoTweenProcess()
    {
        float time = 0f;
        float value = 0f;
        Vector3 pos = Vector3.zero;

        while (true)
        {
            if (time > 1.0f)
            {
                transform.position = m_to;
                yield break;
            }

            value = m_curve.Evaluate(time);
            pos = m_from * (1f - value) + m_to * value;

            if (m_navAgent != null)
            {
                m_navAgent.Move(pos - transform.position);
            }
            else if (m_characterController != null)
            {
                m_characterController.Move(pos - transform.position);
            }
            else
            {
                transform.position = pos;
            }

            time += Time.deltaTime / m_duration;
            yield return null;
        }
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(CoTweenProcess());
    }

    public void Play(Vector3 from, Vector3 to, float duration)
    {
        m_from = from;
        m_to = to;
        m_duration = duration;
        Play();
    }

    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_characterController = GetComponent<CharacterController>();
    }
}