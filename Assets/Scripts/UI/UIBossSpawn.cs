using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBossSpawn : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_bossSpawnText;
    [SerializeField]
    EnemyManager m_enemyManager;
    [SerializeField]
    PathController m_bossPath;
    [SerializeField]
    DirectionArrow m_directionArrow;

    public void ShowBossSpawnMessage()
    {
        StartCoroutine(CoBossSpawnRoutine());
    }

    IEnumerator CoBossSpawnRoutine()
    {
        // 보스 몬스터 생성 텍스트 출력
        for (int i = 0; i < 3; i++)
        {
            yield return Utility.GetWaitForSeconds(1f);

            m_bossSpawnText.gameObject.SetActive(true);
            yield return Utility.GetWaitForSeconds(0.5f);

            m_bossSpawnText.gameObject.SetActive(false);
            yield return Utility.GetWaitForSeconds(0.5f);
        }

        // 보스 몬스터 생성 위치 표시
        if (m_bossPath.Points.Length > 0)
        {
            m_directionArrow.ShowArrowToTarget(m_bossPath.Points[0]);
        }

        // 보스 몬스터 생성
        m_enemyManager.CreateEnemy(EnemyManager.EnemyType.BossMonster, m_bossPath, 1);
    }

    void UpdateTexts()
    {
        m_bossSpawnText.text = LanguageManager.Instance.SetUITextLanguage("BossSpawn");
    }

    void OnEnable()
    {
        if (LanguageManager.Instance == null) return;

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
            UpdateTexts();
        }
    }

    void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged -= UpdateTexts;
        }
    }

    void Start()
    {
        m_bossSpawnText.gameObject.SetActive(false);
    }
}