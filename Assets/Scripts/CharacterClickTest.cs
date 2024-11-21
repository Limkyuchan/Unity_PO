using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterClickTest : MonoBehaviour
{
    string characterType;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {characterType}");
    }

    private void SelectCharacterType(string type)
    {
        Debug.Log($"Character Selected: {type}");
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 Ŭ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = LayerMask.GetMask("Default"); // Default ���̾ �ش��ϴ� �̸� �Է�
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}");
            }
            else
            {
                Debug.Log("No object detected on specified layer");
            }
        }
    }
}