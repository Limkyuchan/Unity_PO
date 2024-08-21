using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component
{
    Queue<T> m_pool;
    int m_count;
    Func<T> m_createFunc;      

    public int Count { get { return m_pool.Count; } }

    public GameObjectPool() { }

    public GameObjectPool(int count, Func<T> createFunc)    
    {
        m_count = count;
        m_createFunc = createFunc;
        m_pool = new Queue<T>(count);
        Allocate();     // 갯수 지정한 만큼 GameObject 생성하고 Queue에 저장
    }

    public void MakePool(int count, Func<T> createFunc)
    {
        m_count = count;
        m_createFunc = createFunc;
        m_pool = new Queue<T>(count);
        Allocate();
    }

    public void Set(T obj)
    {
        m_pool.Enqueue(obj);
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

    public T New()
    {
        return m_createFunc();
    }

    void Allocate()     // 메모리 할당 함수
    {
        for (int i = 0; i < m_count; i++)
        {
            m_pool.Enqueue(m_createFunc());
        }
    }
}
