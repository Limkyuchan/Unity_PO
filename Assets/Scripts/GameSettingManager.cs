using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class GameSettingManager : MonoBehaviour
{
    [Header("초기 캐릭터 설정")]
    [SerializeField]
    GameObject m_choicePlayer;
    [SerializeField]
    Button m_buttonWarrior;
    [SerializeField]
    Button m_buttonRange;

    [Header("공통적으로 사용되는 UI")]
    [SerializeField]
    GameObject m_playerNameParent;
    [SerializeField]
    TMP_InputField m_playerNameInput;
    [SerializeField]
    TextMeshProUGUI m_placeholderText;
    [SerializeField]
    GameObject m_warningParent;
    [SerializeField]
    TextMeshProUGUI m_warningText;

    [Header("근거리 캐릭터 UI")]
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

    [Header("원거리 캐릭터 UI")]
    [SerializeField]
    GameObject m_playerRange;
    [SerializeField]
    GameObject m_rangeCharacterUI;
    [SerializeField]
    Button m_buttonRangeWeapon1;
    [SerializeField]
    Button m_buttonRangeWeapon2;
    [SerializeField]
    GameObject m_weaponRange1;
    [SerializeField]
    GameObject m_weaponRange2;

    string m_selectCharacterType;
    string m_selectedWeapon;
    int m_maxNameLength;

    public void SelectCharacterType(string characterType)
    { 
        m_choicePlayer.SetActive(false);

        if (characterType == "Warrior")
        {
            m_warriorCharacterUI.SetActive(true);
            m_rangeCharacterUI.SetActive(false);
            m_playerWarrior.SetActive(true);
            m_playerNameParent.SetActive(true);
        }
        else if (characterType == "Range")
        {
            m_warriorCharacterUI.SetActive(false);
            m_rangeCharacterUI.SetActive(true);
            m_playerRange.SetActive(true);
            m_playerNameParent.SetActive(true);
        }

        m_selectCharacterType = characterType;
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
            if (weapon == "Weapon1")
            {
                m_weaponRange1.SetActive(true);
                m_weaponRange2.SetActive(false);
            }
            else if (weapon == "Weapon2")
            {
                m_weaponRange1.SetActive(false);
                m_weaponRange2.SetActive(true);
            }
        }

        m_selectedWeapon = weapon;
    }

    public void GoGameScene()
    {
        if (string.IsNullOrEmpty(m_playerNameInput.text))
        {
            m_warningText.text = " 이름을 입력해주세요!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }
        
        if (m_playerNameInput.text.Length > m_maxNameLength)
        {
            m_warningText.text = "영문,공백 포함 8글자 이내로 입력해주세요!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        if (string.IsNullOrEmpty(m_selectedWeapon))
        {
            m_warningText.text = "무기를 선택해주세요!";
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
            m_warningText.text = "이름은 8글자 이내로 입력해주세요!";
            StartCoroutine(CoOnOffWarningMessage());
        }
    }

    void Start()
    {
        m_maxNameLength = 8;
        m_placeholderText.text = "  이름을 입력해주세요";

        m_warriorCharacterUI.SetActive(false);
        m_rangeCharacterUI.SetActive(false);
        m_playerWarrior.SetActive(false);
        m_playerRange.SetActive(false);
        m_weaponAxe.SetActive(false);
        m_weaponSword.SetActive(false);
        m_weaponRange1.SetActive(false);
        m_weaponRange2.SetActive(false);
        m_playerNameParent.gameObject.SetActive(false);
        m_warningParent.gameObject.SetActive(false);

        m_buttonWarrior.onClick.AddListener(() => SelectCharacterType("Warrior"));
        m_buttonRange.onClick.AddListener(() => SelectCharacterType("Range"));

        m_buttonAxe.onClick.AddListener(() => SelectWeapon("Axe"));
        m_buttonSword.onClick.AddListener(() => SelectWeapon("Sword"));
        m_buttonRangeWeapon1.onClick.AddListener(() => SelectWeapon("Weapon1"));
        m_buttonRangeWeapon2.onClick.AddListener(() => SelectWeapon("Weapon2"));

        m_playerNameInput.onValueChanged.AddListener(ValidateNameLength);
    }

    void Update()
    {
        RotateCharacter();
    }
}