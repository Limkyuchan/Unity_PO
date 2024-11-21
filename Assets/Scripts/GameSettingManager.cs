using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEditor.Rendering;
using UnityEngine.SceneManagement;

public class GameSettingManager : MonoBehaviour
{
    [Header("초기 캐릭터 설정")]
    [SerializeField]
    GameObject m_choicePlayer;
    [SerializeField]
    GameObject m_introduceWarrior;
    [SerializeField]
    GameObject m_introduceRange;
    [SerializeField]
    GameObject m_pointLight;
    [SerializeField]
    GameObject m_lightWarrior;
    [SerializeField]
    GameObject m_lightRange;

    [Header("공통적으로 사용되는 UI")]
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

    [Header("근거리 캐릭터 UI")]
    [SerializeField]
    GameObject m_playerWarrior;
    [SerializeField]
    GameObject m_Axe;
    [SerializeField]
    GameObject m_Sword;
    [SerializeField]
    GameObject m_warriorCharacterUI;
    [SerializeField]
    GameObject m_wearAxe;
    [SerializeField]
    GameObject m_wearSword;

    [Header("원거리 캐릭터 UI")]
    [SerializeField]
    GameObject m_playerRange;
    [SerializeField]
    GameObject m_rangeCharacterUI;
    [SerializeField]
    Button m_buttonRedFlare;
    [SerializeField]
    Button m_buttonBlueBolt;
    [SerializeField]
    GameObject m_wearRedFlare;
    [SerializeField]
    GameObject m_wearBlueBolt;

    PlayerAnimController m_warriorAnimController;
    PlayerAnimController m_rangeAnimController;
    BoxCollider m_warriorCollider;
    BoxCollider m_rangeCollider;
    Vector3 m_warriorInitPosition = new Vector3(-0.600989461f, 0f, -7.62807608f);
    Quaternion m_warriorInitRoation = new Quaternion(0f, 0.985329509f, 0f, 0.17066291f);
    Vector3 m_rangeInitPosition = new Vector3(0.70246619f, 0f, -7.62788105f);
    Quaternion m_rangeInitRotation = new Quaternion(0f, 0.987895489f, 0f, -0.155121163f);
    Vector3 m_targetPosition = new Vector3(-1.27900004f, -0.182690054f, -7.6457777f);
    Quaternion m_targetRotation = new Quaternion(0.0424279869f, 0.949925661f, -0.0326390304f, 0.307856888f);

    string m_selectCharacterType;
    string m_selectWeapon;
    int m_maxNameLength;

    public void SelectCharacterType(string characterType)
    {
        m_selectCharacterType = characterType;

        if (m_selectCharacterType == "Warrior")
        {
            m_introduceWarrior.SetActive(true);
            m_playerRange.SetActive(false);

            m_warriorAnimController.Play(PlayerAnimController.Motion.Victory);
            StartCoroutine(CoResetToIdle(m_warriorAnimController, 2.5f));
        }
        else if (m_selectCharacterType == "Range")
        {
            m_introduceRange.SetActive(true);
            m_playerWarrior.SetActive(false);

            m_rangeAnimController.Play(PlayerAnimController.Motion.Victory);
            StartCoroutine(CoResetToIdle(m_rangeAnimController, 2.5f));
        }
    }

    public void ReturnSelectCharacterType()
    {
        m_playerWarrior.SetActive(true);
        m_playerRange.SetActive(true);
        m_warriorCollider.enabled = true;
        m_rangeCollider.enabled = true;

        m_introduceWarrior.SetActive(false);
        m_introduceRange.SetActive(false);

        m_choicePlayer.SetActive(true);
    }

    public void SettingPlayerCharacter()
    {
        m_choicePlayer.SetActive(false);
        m_commonParent.SetActive(true);

        m_lightRange.SetActive(false);
        m_lightWarrior.SetActive(false);
        m_pointLight.SetActive(true);

        if (m_selectCharacterType == "Warrior")
        {
            m_warriorAnimController.Play(PlayerAnimController.Motion.Idle);
            m_playerRange.SetActive(false);
            m_playerWarrior.SetActive(true);
            m_warriorCollider.enabled = false;
            m_playerWarrior.transform.position = m_targetPosition;
            m_playerWarrior.transform.rotation = m_targetRotation;

            m_warriorCharacterUI.SetActive(true);
            m_Axe.SetActive(true);
            m_Sword.SetActive(true);
        }
        else if (m_selectCharacterType == "Range")
        {
            m_rangeAnimController.Play(PlayerAnimController.Motion.Idle);
            m_playerWarrior.SetActive(false);
            m_playerRange.SetActive(true);
            m_rangeCollider.enabled = false;
            m_playerRange.transform.position = m_targetPosition;
            m_playerRange.transform.rotation = m_targetRotation;

            m_rangeCharacterUI.SetActive(true);
        }
    }

    public void ReturnSettingPlayerCharacter()
    {
        m_commonParent.SetActive(false);
        m_pointLight.SetActive(false);
        m_warriorCharacterUI.SetActive(false);
        m_rangeCharacterUI.SetActive(false);
        m_Axe.SetActive(false);
        m_wearAxe.SetActive(false);
        m_Sword.SetActive(false);
        m_wearSword.SetActive(false);

        m_choicePlayer.SetActive(true);
        m_lightRange.SetActive(true);
        m_lightWarrior.SetActive(true);

        m_playerWarrior.SetActive(true);
        m_playerWarrior.transform.position = m_warriorInitPosition;
        m_playerWarrior.transform.rotation = m_warriorInitRoation;

        m_playerRange.SetActive(true);
        m_playerRange.transform.position = m_rangeInitPosition;
        m_playerRange.transform.rotation = m_rangeInitRotation;

        m_introduceWarrior.SetActive(false);
        m_introduceRange.SetActive(false);
    }

    public void SelectWeapon(string weapon)
    {
        m_selectWeapon = weapon;

        if (m_selectCharacterType == "Warrior")
        {
            m_warriorAnimController.Play(PlayerAnimController.Motion.Idle, false);
            if (weapon == "Axe")
            {
                m_wearAxe.SetActive(true);
                m_wearSword.SetActive(false);
            }
            else if (weapon == "Sword")
            {
                m_wearSword.SetActive(true);
                m_wearAxe.SetActive(false);
            }
            m_warriorAnimController.Play(PlayerAnimController.Motion.ShowSkill);
            StartCoroutine(CoResetToIdle(m_warriorAnimController, 4f));
        }
        else if (m_selectCharacterType == "Range")
        {
            if (weapon == "RedFlare")
            {

            }
            else if (weapon == "BlueBolt")
            {

            }
        }
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

        if (string.IsNullOrEmpty(m_selectWeapon))
        {
            m_warningText.text = "무기를 선택해주세요!";
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        PlayerPrefs.SetString("PlayerName", m_playerNameInput.text);
        PlayerPrefs.SetString("PlayerWeapon", m_selectWeapon);
        PlayerPrefs.SetString("PlayerCharacterType", m_selectCharacterType);

        PlayerPrefs.Save();

        LoadSceneManager.Instance.LoadSceneAsync(SceneState.GameScene01);
    }

    public void GoTitleScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(SceneState.Title);
    }


    IEnumerator CoResetToIdle(PlayerAnimController animController, float time)
    {
        yield return Utility.GetWaitForSeconds(time);
        animController.Play(PlayerAnimController.Motion.Idle);
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
        // Warrior 캐릭터의 초기 설정
        m_playerWarrior.SetActive(true);
        m_playerWarrior.transform.position = m_warriorInitPosition;
        m_playerWarrior.transform.rotation = m_warriorInitRoation;
        m_warriorAnimController = m_playerWarrior.GetComponent<PlayerAnimController>();
        m_warriorCollider = m_playerWarrior.GetComponent<BoxCollider>();

        // Range 캐릭터의 초기 설정
        m_playerRange.SetActive(true);
        m_playerRange.transform.position = m_rangeInitPosition;
        m_playerRange.transform.rotation = m_rangeInitRotation;
        m_rangeAnimController = m_playerRange.GetComponent<PlayerAnimController>();
        m_rangeCollider = m_playerRange.GetComponent<BoxCollider>();

        m_maxNameLength = 8;
        m_placeholderText.text = "  이름을 입력해주세요";
        m_textMessage.text = "마우스 좌 클릭으로 캐릭터를 회전시킬 수 있습니다.";

        m_choicePlayer.SetActive(true);
        m_lightWarrior.SetActive(true);
        m_lightRange.SetActive(true);
        m_introduceWarrior.SetActive(false);
        m_introduceRange.SetActive(false);
        m_pointLight.SetActive(false);

        m_warriorCharacterUI.SetActive(false);
        m_rangeCharacterUI.SetActive(false);
        m_commonParent.gameObject.SetActive(false);
        m_warningParent.gameObject.SetActive(false);

        m_Axe.SetActive(false);
        m_wearAxe.SetActive(false);
        m_Sword.SetActive(false);
        m_wearSword.SetActive(false);

        m_playerNameInput.onValueChanged.AddListener(ValidateNameLength);
    }

    void Update()
    {
        RotateCharacter();
    }
}