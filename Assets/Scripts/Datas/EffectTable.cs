using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectDataClassDictionary : SerializableDictionary<int, EffectData> { }

public class EffectTable : SingletonMonoBehaviour<EffectTable>
{
    [SerializeField]
    public EffectDataClassDictionary m_table = new EffectDataClassDictionary();

    public EffectData GetData(int id)
    {
        if (m_table.ContainsKey(id))
        {
            return m_table[id];
        }
        else
        {
            var defaultEffectData = new EffectData
            {
                Id = 0,
                Dummy = "",
                Prefabs = new string[4] {"", "", "", ""}
            };
            return defaultEffectData;
        }
    }

    public void LoadData()
    {
        m_table.Clear();
        ExcelDataLoader.Instance.LoadTable("Effect");

        for (int i = 0; i < ExcelDataLoader.Instance.Count; i++)
        {
            EffectData data = new EffectData();
            data.Id = ExcelDataLoader.Instance.GetInteger("Id", i);
            data.Dummy = ExcelDataLoader.Instance.GetString("Dummy", i);
            for (int j = 0; j < data.Prefabs.Length; j++)
            {
                data.Prefabs[j] = ExcelDataLoader.Instance.GetString("Prefab_" + (j + 1), i);
            }
            m_table.Add(data.Id, data);
        }
        ExcelDataLoader.Instance.Clear();
    }
}