using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerStat : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    TextMeshProUGUI m_textBlood;
    [SerializeField]
    TextMeshProUGUI m_textAttack;
    [SerializeField]
    TextMeshProUGUI m_textKillScore;

    void Update()
    {
        m_textBlood.text = m_player.PlayerCurHp + " / " + m_player.PlayerMaxHp;
        m_textAttack.text = m_player.PlayerAttack.ToString();
        m_textKillScore.text = m_player.DeathEnemyCnt + " / " + m_player.TotalEnemyCnt;
    }
}