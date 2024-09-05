using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField]
    List<PlayerAnimController.Motion> m_comboList;
    Queue<KeyCode> m_keyBuffer = new Queue<KeyCode>();
    int m_comboIndex;

    public int CommandCount { get { return m_keyBuffer.Count; } }
    public int ComboCount { get { return m_comboList.Count; } }

    public PlayerAnimController.Motion GetCombo()
    {
        m_comboIndex++;
        if (m_comboIndex >= m_comboList.Count)
        {
            m_comboIndex = 0;
        }
        return m_comboList[m_comboIndex];
    }

    public void ResetCombo()
    {
        m_comboIndex = 0;
    }

    public KeyCode GetCommand()
    {
        return m_keyBuffer.Dequeue();
    }

    public void AddCommand(KeyCode key)
    {
        m_keyBuffer.Enqueue(key);
        if (IsInvoking("ReleaseKeyBuffer"))
        { 
            CancelInvoke("ReleaseKeyBuffer");
        }
        Invoke("ReleaseKeyBuffer", 0.5f);
    }

    public void ReleaseKeyBuffer()
    {
        m_keyBuffer.Clear();
    }

    void Start()
    {
        
    }
}
