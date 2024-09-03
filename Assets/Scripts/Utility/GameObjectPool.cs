using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component
{
    int m_count;
    Queue<T> m_pool;
    Func<T> m_createFunc;

    public int Count { get { return m_pool.Count; } }

    public GameObjectPool() { }

    public GameObjectPool(int count, Func<T> createFunc)
    {
        m_count = count;
        m_createFunc = createFunc;
        m_pool = new Queue<T>(m_count);
        Allocate();
    }

    void Allocate()
    {
        for (int i = 0; i < m_count; i++)
        {
            m_pool.Enqueue(m_createFunc());
        }
    }

    public void CreatePool(int count, Func<T> createFunc)
    {
        m_count = count;
        m_createFunc = createFunc;
        m_pool = new Queue<T>(m_count);
        Allocate();
    }

    public T Get()
    {
        if (m_pool.Count > 0)
        {
            return m_pool.Dequeue();
        }
        else
        {
            return m_createFunc();
        }
    }

    public void Set(T obj)
    {
        m_pool.Enqueue(obj);
    }

    public T New()
    {
        return m_createFunc();
    }
}
