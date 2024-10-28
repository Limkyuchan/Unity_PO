using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    TextMeshProUGUI m_textAttack;
    [SerializeField]
    TextMeshProUGUI m_textKillScore;

    void Update()
    {
        m_textAttack.text = m_player.GetPlayerAttack.ToString();
        m_textKillScore.text = m_player.DeathEnemyCnt.ToString();
    }
}
