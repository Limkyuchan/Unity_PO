using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class GameSettingManager : MonoBehaviour
{
    [Header("�ʱ� ĳ���� ����")]
    [SerializeField]
    GameObject m_choicePlayer;
    [SerializeField]
    Button m_buttonWarrior;
    [SerializeField]
    Button m_buttonRange;
    [SerializeField]
    GameObject m_settingUI;
    [SerializeField]
    TextMeshProUGUI m_settingText;

    [Header("���������� ���Ǵ� UI")]
    [SerializeField]
    GameObject m_commonParent;
    [SerializeField]
    TMP_InputField m_playerNameInput;
    [SerializeField]
    TextMeshProUGUI m_placeholderText;
    [SerializeField]
    TextMeshProUGUI m_textMessage;
    [SerializeField]
    GameObject m_warningParent;
    [SerializeField]
    TextMeshProUGUI m_warningText;

    [Header("�ٰŸ� ĳ���� UI")]
    [SerializeField]
    GameObject m_playerWarrior;
    [SerializeField]
    GameObject m_warriorCharacterUI;
    [SerializeField]
    Button m_buttonAxe;
    [SerializeField]
    Button m_buttonSword;
    [SerializeField]
    GameObject m_weaponAxe;
    [SerializeField]
    GameObject m_weaponSword;

    [Header("���Ÿ� ĳ���� UI")]
    [SerializeField]
    GameObject m_playerRange;
    [SerializeField]
    GameObject m_rangeCharacterUI;
    [SerializeField]
    Button m_buttonRedFlare;
    [SerializeField]
    Button m_buttonBlueBolt;
    [SerializeField]
    GameObject m_weaponRedFlare;
    [SerializeField]
    GameObject m_weaponBlueBolt;

    string m_selectCharacterType;
    string m_chectText;
    string m_selectedWeapon;
    int m_maxNameLength;

    public void SelectCharacterType(string characterType)
    {
        m_selectCharacterType = characterType;
        m_settingUI.SetActive(true);
        
        if (m_selectCharacterType == "Warrior")
        {
            m_chectText = "�ٰŸ� ĳ���ͷ�\r\n �÷��� �Ͻðڽ��ϱ�?";
        }
        else if (m_selectCharacterType == "Range")
        {
            m_chectText = "���Ÿ� ĳ���ͷ�\r\n �÷��� �Ͻðڽ��ϱ�?";
        }

        m_settingText.text = m_chectText;
    }

    public void ReturnSelectCharacterType()
    {
        m_choicePlayer.SetActive(true);
        m_settingUI.SetActive(false);

        m_playerNameInput.text = null;
        m_selectedWeapon = null;
        m_warriorCharacterUI.SetActive(false);
        m_rangeCharacterUI.SetActive(false);
        m_playerWarrior.SetActive(false);
        m_playerRange.SetActive(false);
        m_weaponAxe.SetActive(false);
        m_weaponSword.SetActive(false);
        m_weaponRedFlare.SetActive(false);
        m_weaponBlueBolt.SetActive(false);
        m_commonParent.gameObject.SetActive(false);
    }

    public void SettingPlayerCharacter()
    {
        m_choicePlayer.SetActive(false);
        m_commonParent.SetActive(true);

        if (m_selectCharacterType == "Warrior")
        {
            m_warriorCharacterUI.SetActive(true);
            m_playerWarrior.SetActive(true);
        }
        else if (m_selectCharacterType == "Range")
        {
            m_rangeCharacterUI.SetActive(true);
            m_playerRange.SetActive(true);
        }
    }

    public void SelectWeapon(string weapon)
    {
        if (m_selectCharacterType == "Warrior")
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
        }
        else if (m_selectCharacterType == "Range")
        {
            if (weapon == "RedFlare")
            {
                m_weaponRedFlare.SetActive(true);
                m_weaponBlueBolt.SetActive(false);
            }
            else if (weapon == "BlueBolt")
            {
                m_weaponRedFlare.SetActive(false);
                m_weaponBlueBolt.SetActive(true);
            }
        }

        m_selectedWeapon = weapon;
    }

    public void GoGameScene()
    {
        if (string.IsNullOrEmpty(m_playerNameInput.text))
        {
            m_warningText.text = " �̸��� �Է����ּ���!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }
        
        if (m_playerNameInput.text.Length > m_maxNameLength)
        {
            m_warningText.text = "����,���� ���� 8���� �̳��� �Է����ּ���!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        if (string.IsNullOrEmpty(m_selectedWeapon))
        {
            m_warningText.text = "���⸦ �������ּ���!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        PlayerPrefs.SetString("PlayerName", m_playerNameInput.text);
        PlayerPrefs.SetString("PlayerWeapon", m_selectedWeapon);
        PlayerPrefs.SetString("PlayerCharacterType", m_selectCharacterType);

        PlayerPrefs.Save();

        LoadSceneManager.Instance.LoadSceneAsync(SceneState.GameScene01);
    }

    public void GoTitleScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(SceneState.Title);
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            float rotationSpeed = 10f;
            float rotation = Input.GetAxis("Mouse X") * rotationSpeed;

            if (m_selectCharacterType == "Warrior")
            {
                m_playerWarrior.transform.Rotate(Vector3.up, -rotation);
            }
            else if (m_selectCharacterType == "Range")
            {
                m_playerRange.transform.Rotate(Vector3.up, -rotation);
            }
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
        m_placeholderText.text = "  �̸��� �Է����ּ���";
        m_textMessage.text = "���콺 �� Ŭ������ ĳ���͸� ȸ����ų �� �ֽ��ϴ�.";

        m_choicePlayer.SetActive(true);
        m_settingUI.SetActive(false);
        m_warriorCharacterUI.SetActive(false);
        m_rangeCharacterUI.SetActive(false);
        m_playerWarrior.SetActive(false);
        m_playerRange.SetActive(false);
        m_weaponAxe.SetActive(false);
        m_weaponSword.SetActive(false);
        m_weaponRedFlare.SetActive(false);
        m_weaponBlueBolt.SetActive(false);
        m_commonParent.gameObject.SetActive(false);
        m_warningParent.gameObject.SetActive(false);

        m_buttonWarrior.onClick.AddListener(() => SelectCharacterType("Warrior"));
        m_buttonRange.onClick.AddListener(() => SelectCharacterType("Range"));

        m_buttonAxe.onClick.AddListener(() => SelectWeapon("Axe"));
        m_buttonSword.onClick.AddListener(() => SelectWeapon("Sword"));
        m_buttonRedFlare.onClick.AddListener(() => SelectWeapon("RedFlare"));
        m_buttonBlueBolt.onClick.AddListener(() => SelectWeapon("BlueBolt"));

        m_playerNameInput.onValueChanged.AddListener(ValidateNameLength);
    }

    void Update()
    {
        RotateCharacter();
    }
}