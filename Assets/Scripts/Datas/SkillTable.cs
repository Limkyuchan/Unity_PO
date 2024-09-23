using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class SkillDataClassDictionary : SerializableDictionary<PlayerAnimController.Motion, SkillData> { }

public class SkillTable : SingletonMonoBehaviour<SkillTable>
{
    [SerializeField]
    SkillDataClassDictionary m_table = new SkillDataClassDictionary();

    public SkillData GetSkillData(PlayerAnimController.Motion motion)
    {
        return m_table[motion];
    }

    void LoadData()
    {
        ExcelDataLoader.Instance.LoadTable("Skill");
        for (int i = 0; i < ExcelDataLoader.Instance.Count; i++)
        {
            SkillData data = new SkillData();
            data.skillMotion = ExcelDataLoader.Instance.GetEnum<PlayerAnimController.Motion>("skillMotion", i);
            data.attackArea = ExcelDataLoader.Instance.GetInteger("attackArea", i);
            data.attack = ExcelDataLoader.Instance.GetFloat("attack", i);
            data.hitRate = ExcelDataLoader.Instance.GetFloat("hitRate", i);
            data.knockback = ExcelDataLoader.Instance.GetFloat("knockback", i);
            data.knockbackDuration = ExcelDataLoader.Instance.GetFloat("knockbackDuration", i);
            m_table.Add(data.skillMotion, data);
        }
    }

    protected override void OnStart()
    {
        LoadData();
    }
}
