using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeImage : MonoBehaviour
{
    [SerializeField]
    Image skill_Image;
    [SerializeField]
    Sprite img_Warrior;
    [SerializeField]
    Sprite img_Range;

    PlayerController m_player;

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
        SetImage(m_player);
    }

    void SetImage(PlayerController player)
    {
        if (m_player.GetPlayerType == PlayerController.Type.Warrior)
        {
            skill_Image.sprite = img_Warrior;
        }
        else if (m_player.GetPlayerType == PlayerController.Type.Range)
        {
            skill_Image.sprite = img_Range;
        }
    }
}