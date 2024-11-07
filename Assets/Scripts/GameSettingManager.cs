using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class GameSettingManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_player;
    [SerializeField]
    TMP_InputField m_playerNameInput;
    [SerializeField]
    TextMeshProUGUI m_placeholderText;
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
            m_player.transform.Rotate(Vector3.up, -rotation);
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