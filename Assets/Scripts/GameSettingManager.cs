using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSettingManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_player;
    [SerializeField]
    TMP_InputField m_playerNameInput;
    [SerializeField]
    GameObject m_warningParent;
    [SerializeField]
    TextMeshProUGUI m_warningText;
    [SerializeField]
    Button m_buttonAxe;
    [SerializeField]
    Button m_buttonSword;
    [SerializeField]
    GameObject m_weaponAxe;
    [SerializeField]
    GameObject m_weaponSword;

    string m_selectedWeapon;
    int m_maxNameLength;

    public void SelectWeapon(string weapon)
    {
        if (weapon == "Axe")
        {
            m_weaponAxe.SetActive(true);
            m_weaponSword.SetActive(false);
        }
        else if (weapon == "Sword")
        {
            m_weaponAxe.SetActive(false);
            m_weaponSword.SetActive(true);
        }

        m_selectedWeapon = weapon;
    }

    public void GoGameScene()
    {
        // �̸��� ���� ���� Ȯ��
        if (string.IsNullOrEmpty(m_playerNameInput.text) || m_playerNameInput.text.Length > m_maxNameLength)
        {
            m_warningText.text = "����,���� ���� 8���� �̳��� �̸��� �Է����ּ���!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        if (string.IsNullOrEmpty(m_selectedWeapon))
        {
            m_warningText.text = "���⸦ �������ּ���!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        // �̸��� ���⸦ PlayerPrefs�� ����
        PlayerPrefs.SetString("PlayerName", m_playerNameInput.text);
        PlayerPrefs.SetString("PlayerWeapon", m_selectedWeapon);
        PlayerPrefs.Save();

        //SceneManager.LoadScene("GameScene01");
    }

    public void GoTitleScene()
    {
        Debug.Log("Ÿ��Ʋ ��!");
    }

    IEnumerator CoOnOffWarningMessage()
    {
        m_warningParent.SetActive(true);
        yield return Utility.GetWaitForSeconds(2f);
        m_warningParent.SetActive(false);
        yield return Utility.GetWaitForSeconds(0.5f);
    }

    void RotateCharacter()
    {
        if (Input.GetMouseButton(0))
        {
            float rotationSpeed = 10f;
            float rotation = Input.GetAxis("Mouse X") * rotationSpeed;
            m_player.transform.Rotate(Vector3.up, -rotation);
        }
    }

    void ValidateNameLength(string name)
    {
        if (name.Length > m_maxNameLength)
        {
            m_warningText.text = "�̸��� 8���� �̳��� �Է����ּ���!";
            StartCoroutine(CoOnOffWarningMessage());
        }
    }

    void Start()
    {
        m_maxNameLength = 8;

        m_weaponAxe.SetActive(false);
        m_weaponSword.SetActive(false);
        m_warningParent.gameObject.SetActive(false);

        m_buttonAxe.onClick.AddListener(() => SelectWeapon("Axe"));
        m_buttonSword.onClick.AddListener(() => SelectWeapon("Sword"));

        m_playerNameInput.onValueChanged.AddListener(ValidateNameLength);
    }

    void Update()
    {
        RotateCharacter();
    }
}