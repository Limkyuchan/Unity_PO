using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTable : SingletonMonoBehaviour<SkillTable>
{
    [SerializeField]
    List<SkillData> m_skillList;
    Dictionary<PlayerAnimController.Motion, SkillData> m_table = new Dictionary<PlayerAnimController.Motion, SkillData>();

    public SkillData GetSkillData(PlayerAnimController.Motion motion)
    {
        return m_table[motion];
    }


    protected override void OnStart()
    {
        for (int i = 0; i < m_skillList.Count; i++)
        {
            m_table.Add(m_skillList[i].skillMotion, m_skillList[i]);
        }
    }
}
