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

    public static GameObject FindChildObject(GameObject obj, string childName)
    {
        var childs = obj.GetComponentsInChildren<Transform>();
        foreach (var child in childs)
        {
            if (child.name.Equals(childName))
            {
                return child.gameObject;
            }
        }
        return null;
    }
}