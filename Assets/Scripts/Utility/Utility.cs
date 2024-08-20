using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{ 
    static Dictionary<float, WaitForSeconds> m_waitForSecList = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitForSeconds(float sec)
    {
        if (m_waitForSecList.ContainsKey(sec))
        {
            return m_waitForSecList[sec];
        }
        m_waitForSecList.Add(sec, new WaitForSeconds(sec));
        return m_waitForSecList[sec];
    }
   
}