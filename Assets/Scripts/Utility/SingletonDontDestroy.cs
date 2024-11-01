using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : SingletonDontDestroy<T>
{
    static T m_instance;

    public static T Instance { get { return m_instance; } private set { m_instance = value; } }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = (T)this;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (m_instance == this)
        {
            OnStart();
        }
    }
}