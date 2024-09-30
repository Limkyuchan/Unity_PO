using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillDataClassDictionary : SerializableDictionary<PlayerAnimController.Motion, SkillData> { }

public class SkillTable : SingletonMonoBehaviour<SkillTable>
{
    [SerializeField]
    SkillDataClassDictionary m_table = new SkillDataClassDictionary();

    public SkillData GetSkillData(PlayerAnimController.Motion motion)
    {
        if (m_table.ContainsKey(motion))
        {
            return m_table[motion];
        }
        else
        {
            var defaultSkillData = new SkillData
            {
                skillMotion = PlayerAnimController.Motion.None,
                attackArea = 0,
                attack = 0,
                hitRate = 0,
                knockback = 0,
                knockbackDuration = 0
            };
            return defaultSkillData;
        }
    }

    void LoadData()
    {
        m_table.Clear();
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
        ExcelDataLoader.Instance.Clear();
    }

    protected override void OnStart()
    {
        LoadData();
    }
}