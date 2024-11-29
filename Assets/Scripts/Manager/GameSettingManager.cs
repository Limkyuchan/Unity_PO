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
    GameObject m_introduceWarrior;
    [SerializeField]
    GameObject m_introduceRange;
    [SerializeField]
    GameObject m_pointLight;
    [SerializeField]
    GameObject m_lightWarrior;
    [SerializeField]
    GameObject m_lightRange;

    [Header("Text Mesh Pro UGUI")]
    [SerializeField]
    TextMeshProUGUI m_playerSettings;
    [SerializeField]
    TextMeshProUGUI m_playerType;
    [SerializeField]
    TextMeshProUGUI m_introduceWarriorText;
    [SerializeField]
    TextMeshProUGUI m_introduceRangeText;
    [SerializeField]
    TextMeshProUGUI m_playerNameTitle;
    [SerializeField]
    TextMeshProUGUI m_playerNameRule;
    [SerializeField]
    TextMeshProUGUI m_placeholderText;
    [SerializeField]
    TextMeshProUGUI m_movePlayerText;
    [SerializeField]
    TextMeshProUGUI m_previousButton;
    [SerializeField]
    TextMeshProUGUI m_gameOnButton;
    [SerializeField]
    TextMeshProUGUI m_warriorWeaponSetting;
    [SerializeField]
    TextMeshProUGUI m_warriorWeaponSelect;
    [SerializeField]
    TextMeshProUGUI m_rangeWeaponSetting;
    [SerializeField]
    TextMeshProUGUI m_rangeWeaponSelect;

    [Header("캐릭터 선택 화살표")]
    [SerializeField]
    GameObject m_arrowIndicator;
    [SerializeField]
    Vector3 m_arrowOffset = new Vector3(0, 1.5f, 0);

    [Header("공통적으로 사용되는 UI")]
    [SerializeField]
    GameObject m_commonParent;
    [SerializeField]
    GameObject m_gameOptionSetting;
    [SerializeField]
    TMP_InputField m_playerNameInput;
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
    GameObject m_AxeEffect;
    [SerializeField]
    GameObject m_Sword;
    [SerializeField]
    GameObject m_SwordEffect;
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
    GameObject m_RedFlare;
    [SerializeField]
    GameObject m_BlueBolt;
    [SerializeField]
    GameObject m_rangeCharacterUI;
    [SerializeField]
    GameObject m_wearRedFlare;
    [SerializeField]
    GameObject m_wearBlueBolt;

    PlayerAnimController m_warriorAnimController;
    PlayerAnimController m_rangeAnimController;
    BoxCollider m_warriorCollider;
    BoxCollider m_rangeCollider;
    
    Vector3 m_warriorInitPosition = new Vector3(-0.660000026f, -0.129999995f, -7.4000001f);
    Quaternion m_warriorInitRoation = new Quaternion(0f, 0.985329509f, 0f, 0.17066291f);
    Vector3 m_rangeInitPosition = new Vector3(0.479999989f, -0.150000006f, -7.42000008f);
    Quaternion m_rangeInitRotation = new Quaternion(0f, 0.987895489f, 0f, -0.155121163f);
    Vector3 m_targetPosition = new Vector3(-1.29999995f, -0.182690054f, -7.42000008f);
    Quaternion m_targetRotation = new Quaternion(0.0421416238f, 0.947202921f, -0.0330081023f, 0.3161349f);

    string m_selectCharacterType;
    string m_selectWeapon;
    int m_maxNameLength;

    public void SelectCharacterType(string characterType)
    {
        m_selectCharacterType = characterType;

        if (m_selectCharacterType == "Warrior")
        {
            m_introduceWarrior.SetActive(true);
            m_introduceRange.SetActive(false);
            m_playerRange.SetActive(false);

            ShowArrowIndicator(m_playerWarrior);

            m_warriorAnimController.Play(PlayerAnimController.Motion.Victory);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_Victory);
            StartCoroutine(CoResetToIdle(m_warriorAnimController, 2.5f));
        }
        else if (m_selectCharacterType == "Range")
        {
            m_introduceRange.SetActive(true);
            m_introduceWarrior.SetActive(false);
            m_playerWarrior.SetActive(false);

            ShowArrowIndicator(m_playerRange);

            m_rangeAnimController.Play(PlayerAnimController.Motion.Victory);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_Victory);
            StartCoroutine(CoResetToIdle(m_rangeAnimController, 2.5f));
        }
    }

    public void ReturnSelectCharacterType()
    {
        m_playerWarrior.SetActive(true);
        m_playerRange.SetActive(true);
        m_warriorCollider.enabled = true;
        m_rangeCollider.enabled = true;

        m_arrowIndicator.SetActive(false);
        m_introduceWarrior.SetActive(false);
        m_introduceRange.SetActive(false);

        m_choicePlayer.SetActive(true);
    }

    public void SettingPlayerCharacter()
    {
        m_choicePlayer.SetActive(false);
        m_commonParent.SetActive(true);

        m_arrowIndicator.SetActive(false);
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
            m_RedFlare.SetActive(true);
            m_BlueBolt.SetActive(true);
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
        m_RedFlare.SetActive(false);
        m_wearRedFlare.SetActive(false);
        m_BlueBolt.SetActive(false);
        m_wearBlueBolt.SetActive(false);

        m_playerNameInput.text = null;
        m_selectWeapon = null;
        m_choicePlayer.SetActive(true);
        m_lightRange.SetActive(true);
        m_lightWarrior.SetActive(true);

        m_playerWarrior.SetActive(true);
        m_warriorCollider.enabled = true;
        m_playerWarrior.transform.position = m_warriorInitPosition;
        m_playerWarrior.transform.rotation = m_warriorInitRoation;

        m_playerRange.SetActive(true);
        m_rangeCollider.enabled = true;
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
                m_AxeEffect.SetActive(true);
                m_wearSword.SetActive(false);
            }
            else if (weapon == "Sword")
            {
                m_wearSword.SetActive(true);
                m_SwordEffect.SetActive(true);
                m_wearAxe.SetActive(false);
            }
            m_warriorAnimController.Play(PlayerAnimController.Motion.ShowSkill);          
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_warriorAttack, 1.166f);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_warriorAttack, 1.5f);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_warriorAttack, 3f);
            StartCoroutine(CoTurnOffEffect(4f));
            StartCoroutine(CoResetToIdle(m_warriorAnimController, 4f));
        }
        else if (m_selectCharacterType == "Range")
        {
            m_rangeAnimController.Play(PlayerAnimController.Motion.Idle, false);
            if (weapon == "RedFlare")
            {
                m_wearRedFlare.SetActive(true);
                m_wearBlueBolt.SetActive(false);
            }
            else if (weapon == "BlueBolt")
            {
                m_wearBlueBolt.SetActive(true);
                m_wearRedFlare.SetActive(false);
            }
            m_rangeAnimController.Play(PlayerAnimController.Motion.ShowSkill);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_rangeAttack, 0.46f);
            StartCoroutine(CoResetToIdle(m_rangeAnimController, 2.5f));
        }
    }

    public void GoGameScene()
    {
        if (string.IsNullOrEmpty(m_playerNameInput.text))
        {
            m_warningText.text = LanguageManager.Instance.SetUITextLanguage("PleaseInputName");
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        if (m_playerNameInput.text.Length > m_maxNameLength)
        {
            m_warningText.text = LanguageManager.Instance.SetUITextLanguage("InputNameRule");
            StartCoroutine(CoOnOffWarningMessage());
            return;
        }

        if (string.IsNullOrEmpty(m_selectWeapon))
        {
            m_warningText.text = LanguageManager.Instance.SetUITextLanguage("PleaseSelectWeapon");
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

    void UpdateTexts()
    {
        m_playerSettings.text = LanguageManager.Instance.SetUITextLanguage("PlayerSettings");
        m_playerType.text = LanguageManager.Instance.SetUITextLanguage("PlayerType");
        m_introduceWarriorText.text = LanguageManager.Instance.SetUITextLanguage("IntroduceWarrior");
        m_introduceRangeText.text = LanguageManager.Instance.SetUITextLanguage("IntroduceRange");
        m_playerNameTitle.text = LanguageManager.Instance.SetUITextLanguage("PlayerNameTitle");
        m_playerNameRule.text = LanguageManager.Instance.SetUITextLanguage("PlayerNameRule");
        m_placeholderText.text = LanguageManager.Instance.SetUITextLanguage("PlaceHolder");
        m_movePlayerText.text = LanguageManager.Instance.SetUITextLanguage("MovePlayer");
        m_previousButton.text = LanguageManager.Instance.SetUITextLanguage("Previous");
        m_gameOnButton.text = LanguageManager.Instance.SetUITextLanguage("GameOn");
        m_warriorWeaponSetting.text = LanguageManager.Instance.SetUITextLanguage("WeaponSetting");
        m_warriorWeaponSelect.text = LanguageManager.Instance.SetUITextLanguage("WeaponSelect");
        m_rangeWeaponSetting.text = LanguageManager.Instance.SetUITextLanguage("WeaponSetting");
        m_rangeWeaponSelect.text = LanguageManager.Instance.SetUITextLanguage("WeaponSelect");
    }

    IEnumerator CoResetToIdle(PlayerAnimController animController, float time)
    {
        yield return Utility.GetWaitForSeconds(time);
        animController.Play(PlayerAnimController.Motion.Idle);
    }

    IEnumerator CoMoveArrowIndicator(GameObject target)
    {
        while (m_arrowIndicator.activeSelf)
        {
            Vector3 targetPosition = target.transform.position + m_arrowOffset;
            m_arrowIndicator.transform.position = targetPosition;

            float bounce = Mathf.Sin(Time.time * 2) * 0.1f;
            m_arrowIndicator.transform.position += new Vector3(0, bounce, 0);

            yield return null;
        }
    }

    IEnumerator CoTurnOffEffect(float time)
    {
        yield return Utility.GetWaitForSeconds(time);
        m_AxeEffect.SetActive(false);
        m_SwordEffect.SetActive(false);
    }

    IEnumerator CoOnOffWarningMessage()
    {
        m_warningParent.SetActive(true);
        yield return Utility.GetWaitForSeconds(2f);
        m_warningParent.SetActive(false);
        yield return Utility.GetWaitForSeconds(0.5f);
    }


    void ShowArrowIndicator(GameObject target)
    {
        if (m_arrowIndicator != null)
        {
            m_arrowIndicator.SetActive(true);
            StartCoroutine(CoMoveArrowIndicator(target));
        }
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
            m_warningText.text = LanguageManager.Instance.SetUITextLanguage("InputNameLength");
            StartCoroutine(CoOnOffWarningMessage());
        }
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

        m_choicePlayer.SetActive(true);
        m_gameOptionSetting.SetActive(true);
        m_lightWarrior.SetActive(true);
        m_lightRange.SetActive(true);
        m_arrowIndicator.SetActive(false);
        m_introduceWarrior.SetActive(false);
        m_introduceRange.SetActive(false);
        m_pointLight.SetActive(false);

        m_warriorCharacterUI.SetActive(false);
        m_rangeCharacterUI.SetActive(false);
        m_commonParent.gameObject.SetActive(false);
        m_warningParent.gameObject.SetActive(false);

        m_Axe.SetActive(false);
        m_AxeEffect.SetActive(false);
        m_wearAxe.SetActive(false);
        m_Sword.SetActive(false);
        m_SwordEffect.SetActive(false);
        m_wearSword.SetActive(false);
        m_RedFlare.SetActive(false);
        m_wearRedFlare.SetActive(false);
        m_BlueBolt.SetActive(false);
        m_wearBlueBolt.SetActive(false);

        m_playerNameInput.onValueChanged.AddListener(ValidateNameLength);
    }

    void Update()
    {
        RotateCharacter();
    }
}