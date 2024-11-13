using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerStat : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_textBlood;
    [SerializeField]
    TextMeshProUGUI m_textAttack;
    [SerializeField]
    TextMeshProUGUI m_textKillScore;
    PlayerController m_player;

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }

    void Update()
    {
        if (m_player != null)
        {
            m_textBlood.text = m_player.PlayerCurHp + " / " + m_player.PlayerMaxHp;
            m_textAttack.text = m_player.PlayerAttack.ToString();
            m_textKillScore.text = m_player.DeathEnemyCnt + " / " + m_player.TotalEnemyCnt;
        }
    }
}