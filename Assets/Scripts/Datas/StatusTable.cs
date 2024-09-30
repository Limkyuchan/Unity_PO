using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusDataClassDictionary : SerializableDictionary<EnemyManager.EnemyType, StatusData> { }

public class StatusTable : SingletonMonoBehaviour<StatusTable>
{
    [SerializeField]
    StatusDataClassDictionary m_table = new StatusDataClassDictionary();

    public StatusData GetStatusData(EnemyManager.EnemyType type)
    {
        if (m_table.ContainsKey(type))
        {
            return m_table[type];
        }
        else
        {
            var defaultStatusData = new StatusData
            {
                type = EnemyManager.EnemyType.None,
                hp = 0,
                hpMax = 0,
                attack = 0,
                defense = 0,
                hitRate = 0,
                dodgeRate = 0,
                criRate = 0,
                criAttack = 0,
                attackDist = 0,
                detectDist = 0
            };
            return defaultStatusData;
        }
    }

    void LoadData()
    {
        m_table.Clear();
        ExcelDataLoader.Instance.LoadTable("Status");
        for (int i = 0; i < ExcelDataLoader.Instance.Count; i++)
        {
            StatusData data = new StatusData();
            data.type = ExcelDataLoader.Instance.GetEnum<EnemyManager.EnemyType>("type", i);
            data.hp = ExcelDataLoader.Instance.GetInteger("hp", i);
            data.hpMax = ExcelDataLoader.Instance.GetInteger("hpMax", i);
            data.attack = ExcelDataLoader.Instance.GetFloat("attack", i);
            data.defense = ExcelDataLoader.Instance.GetFloat("defense", i);
            data.hitRate = ExcelDataLoader.Instance.GetFloat("hitRate", i);
            data.dodgeRate = ExcelDataLoader.Instance.GetFloat("dodgeRate", i);
            data.criRate = ExcelDataLoader.Instance.GetFloat("criRate", i);
            data.criAttack = ExcelDataLoader.Instance.GetFloat("criAttack", i);
            data.attackDist = ExcelDataLoader.Instance.GetFloat("attackDist", i);
            data.detectDist = ExcelDataLoader.Instance.GetFloat("detectDist", i);
            m_table.Add(data.type, data);
        }
        ExcelDataLoader.Instance.Clear();
    }

    protected override void OnStart()
    {
        LoadData();
    }
}