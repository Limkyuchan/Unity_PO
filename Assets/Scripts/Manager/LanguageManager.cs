using System;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager : SingletonDontDestroy<LanguageManager>
{
    public enum Language
    {
        None = -1,
        Korean,
        English
    }

    Dictionary<string, Dictionary<Language, string>> localizedTexts;
    
    public Language CurrentLanguage { get; private set; } = Language.Korean;

    public event Action OnLanguageChanged;

    public void SetLanguage(Language language)
    {
        CurrentLanguage = language;
        OnLanguageChanged?.Invoke();     // ��� ������� �˸�
    }

    public string GetLocalizedText(string key)
    {
        if (localizedTexts.TryGetValue(key, out var translations))
        {
            if (translations.TryGetValue(CurrentLanguage, out var text))
            {
                return text;
            }
        }
        return "Text not found!";
    }

    public string SetUITextLanguage(string key)
    {
        switch (key)
        {
            case "PlayerKeyText":
                return CurrentLanguage == Language.Korean ? "<b>< ���� ���� ></b>\r\n\n��   ĳ���� �̵� ��� : [ W, A, S, D ]\r\n\t<b>W</b> : ������ �̵�\r\n\t<b>A</b> : �·� �̵�\t<b>D</b> : ��� �̵�\r\n\t<b>S</b> : ������ ����\r\n\r��  ĳ���� ���� ��� : [ Space ]\r\n��  ���� �� ���� Ȯ�� : [ H ]\r\n��  ���� ���� (�Ҹ�, ī�޶� ����, ���) : [ O ]" : "<b>< Game Tips ></b>\r\n\n��   How to move characters : [ W, A, S, D ]\r\n\t<b>W</b> : Move forward\r\n\t<b>A</b> : Move Left\t<b>D</b> : Move Right \r\n\t<b>S</b> : Stop Movement\r\n\r��  How to Attack Characters : [ Space ]\r\n��  Check in-game information : [ H ]\r\n��  Game settings (sound, camera vibrations, language) : [ O ]";
            case "GameSetting":
                return CurrentLanguage == Language.Korean ? "[O] ���� ����" : "[O] Game Settings";
            case "GameOption":
                return CurrentLanguage == Language.Korean ? "���� �ɼ�" : "Game Option";
            case "Sound":
                return CurrentLanguage == Language.Korean ? "�Ҹ�" : "Sound";
            case "CameraShake":
                return CurrentLanguage == Language.Korean ? "ī�޶� ����" : "Camera Shake";
            case "SetDamage":
                return CurrentLanguage == Language.Korean ? "�� �ǰ� �� ī�޶� ����" : "�� Camera vibrations at the time of shooting";
            case "Language":
                return CurrentLanguage == Language.Korean ? "���" : "Language";
            case "PlayerSettings":
                return CurrentLanguage == Language.Korean ? "�÷��̾� ����" : "Player Settings";
            case "PlayerType":
                return CurrentLanguage == Language.Korean ? "�� �÷����� ���ΰ� Ÿ���� ���ϼ���." : "�� Choose the type of protagonist \r\nyou want to play.";
            case "IntroduceWarrior":
                return CurrentLanguage == Language.Korean ? "< �ٰŸ� �÷��̾� >\r\n\r\n������ ���� �޺��� ��ų ���� ����\r\n\r\nü��: 200\r\n���ݷ�: 30\r\n����: 10\r\n\r\n�ش� �÷��̾�� \r\n�÷����Ͻðڽ��ϱ�?" : "< Close Range Player >\r\n\r\nPowerful combos and skill attacks\r\n\r\nPhysical strength: 200\r\nAttacks: 30\r\nDefensive: 10\r\n\r\nWith that player\r\nDo you want to play?";
            case "IntroduceRange":
                return CurrentLanguage == Language.Korean ? "< ���Ÿ� �÷��̾� >\r\n\r\n�� �Ÿ����� ���� �����ϸ� �������� ����\r\n\r\nü��: 180\r\n���ݷ�: 25\r\n����: 8\r\n\r\n�ش� �÷��̾�� \r\n�÷����Ͻðڽ��ϱ�?" : "< Long Distance Player >\r\n\r\nAttacking the enemy from a long distance and being able to survive\r\n\r\nPhysical strength: 180\r\nAttack Power: 25\r\nDefensive: 8\r\n\r\nWith that player\r\nDo you want to play?";
            case "PlayerNameTitle":
                return CurrentLanguage == Language.Korean ? "�÷��̾� �̸� ����" : "Set Player Name";
            case "PlayerNameRule":
                return CurrentLanguage == Language.Korean ? "�� �÷����� ���ΰ��� �̸��� �����ּ���.\r\n     ( ��, ���� �� ���� ���� 8���� �̳� )" : "�� Please choose the name of the\r\n     main character.\r\n ";
            case "PlaceHolder":
                return CurrentLanguage == Language.Korean ? "  �̸��� �Է����ּ���" : "  Please enter your name";
            case "MovePlayer":
                return CurrentLanguage == Language.Korean ? "���콺 �� Ŭ������ ĳ���͸� ȸ����ų �� �ֽ��ϴ�." : "You can rotate the character \r\nwith a left mouse click.";
            case "Previous":
                return CurrentLanguage == Language.Korean ? "����" : "Previous";
            case "GameOn":
                return CurrentLanguage == Language.Korean ? "���ӽ���" : "Game On";
            case "WeaponSetting":
                return CurrentLanguage == Language.Korean ? "�÷��̾� ���� ����" : "Player Weapon Settings";
            case "WeaponSelect":
                return CurrentLanguage == Language.Korean ? "�� �÷����� ���ΰ��� ���⸦ �����ּ���." : "�� Please choose the weapon";
            case "PleaseInputName":
                return CurrentLanguage == Language.Korean ? "�̸��� �Է����ּ���!" : "Please enter your name!";
            case "InputNameLength":
                return CurrentLanguage == Language.Korean ? "�̸��� 8���� �̳��� �Է����ּ���!" : "Please enter your name within 8 characters!";
            case "PleaseSelectWeapon":
                return CurrentLanguage == Language.Korean ? "���⸦ �������ּ���!" : "Please choose your weapon!";
            case "InputNameRule":
                return CurrentLanguage == Language.Korean ? "����,���� ���� 8���� �̳��� �Է����ּ���!" : "Please enter within 8 characters including English and spaces!";
            case "ShowInfo":
                return CurrentLanguage == Language.Korean ? "[H] ���� ����" : "[H] View Information";
            case "BossSpawn":
                return CurrentLanguage == Language.Korean ? "���� ���� ����!" : "Boss Monster is here!";
        }
        return key;
    }

    void InitializePopUpLanguageData()
    {
        localizedTexts = new Dictionary<string, Dictionary<Language, string>>
        {
            { "GameGuideTitle", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>���� �ȳ�</color>" },
                    { Language.English, "<color=#000000>Game Guide</color>" }
                }
            },
            { "PlayerInfoTitle", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>�÷��̾� ����</color>" },
                    { Language.English, "<color=#000000>Player Info</color>" }
                }
            },
            { "CommonGameInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "< ���� ���� >\r\n��� ���� ������ ��ġ��� �����Դϴ�.\r\n���� ����ǵ� ������ ����˴ϴ�.\r\n���� ��ġ��� �������� ȿ���� ����ϴ�.\r\n �� 25% Ȯ��: <color=#ff0000>ü��</color> ���� (+20)\r\n �� 25% Ȯ��: <color=#ff0000>��ų ������</color> ���� (+10)\r\n �� 25% Ȯ��: <color=#ff0000>���ݷ�</color> ���� (+2)\r\n\n" },
                    { Language.English, "< Game Info >\r\nThis is a game where you defeat all enemies and bosses.\r\nYour progress is saved even if the map changes.\r\nDefeating enemies grants random effects.\r\n �� 25% chance: <color=#ff0000>Health</color> (+20)\r\n �� 25% chance: <color=#ff0000>Skill gauge</color> (+10)\r\n �� 25% chance: <color=#ff0000>Attack power</color> (+2)\r\n\n" }
                }
            },
            { "BasicKeyInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "< �⺻ Ű >\r\n[O]\r\n ���� ���� �ɼ��� ������ �� �ֽ��ϴ�.\r\n[Tab]\r\n �÷��̾��� ������ Ȯ���� �� �ֽ��ϴ�.\r\n[Left Shift]\r\n �÷��̾��� �̵��ӵ��� �����մϴ�.\r\n[C]\r\n �÷��̾ ���⸦ ��� ����մϴ�.\r\n" },
                    { Language.English, "< Basic Keys >\r\n[O]\r\n Game-related options can be set.\r\n[Tab]\r\n Check player information.\r\n[Left Shift]\r\n Increases player movement speed.\r\n[C]\r\n Player raises a weapon to defend.\r\n" }
                }
            },
            { "WarriorSkillInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "[Space]\r\n �� <color=#ff0000>�޺� ����</color>(4ȸ ��ø)�� �����մϴ�.\r\n\n< ��ų ���� >\r\n[Z]\r\n �� �⺻ �������� �������� ���Դϴ�.\r\n �� ZŰ�� ���� ���� <color=#ff0000>����</color> ��ų �� �ֽ��ϴ�.\r\n[X]\r\n �� 20���� ��Ÿ���� ������ ��ų�Դϴ�.\r\n �� XŰ�� ���� ���� <color=#ff0000>�˹�</color> ��ų �� �ֽ��ϴ�.\r\n\n" },
                    { Language.English, "[Space]\r\n �� <color=#ff0000>Combo Attack</color>(4 stacks) is possible.\r\n\n< Skill Attacks >\r\n[Z]\r\n �� Gauge fills with basic attacks.\r\n �� Press Z to <color=#ff0000>stun</color> enemies.\r\n[X]\r\n �� Has a 20-second cooldown.\r\n �� Press X to <color=#ff0000>knockback</color> enemies.\r\n\n" }
                }
            },
            { "RangeSkillInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "[Space]\r\n �� <color=#ff0000>�⺻ ����</color> �Դϴ�.\r\n\n< ��ų ���� >\r\n[Z]\r\n �� �⺻ �������� �������� ���Դϴ�.\r\n �� �⺻ ������ ���ÿ� <color=#ff0000>3��</color> �߻��մϴ�.\r\n[X]\r\n �� 20���� ��Ÿ���� ������ ��ų�Դϴ�.\r\n �� ���� ��ų�� ������ <color=#ff0000>����</color> ��ŵ�ϴ�.\r\n\n" },
                    { Language.English, "[Space]\r\n �� <color=#ff0000>Basic Attack</color>.\r\n\n< Skill Attacks >\r\n[Z]\r\n �� Gauge fills with basic attacks.\r\n �� Fires <color=#ff0000>3 arrows</color> simultaneously.\r\n[X]\r\n �� Has a 20-second cooldown.\r\n �� Make enemies <color=#ff0000>stun</color> with the range skill\r\n\n" }
                }
            },
            { "CurrentScene", new Dictionary<Language, string>
                {
                    { Language.Korean, "���� ��" },
                    { Language.English, "CurrentScene" }
                }
            },
            { "PlayerType", new Dictionary<Language, string>
                {
                    { Language.Korean, "�÷��̾� Ÿ��" },
                    { Language.English, "PlayerType" }
                }
            },
            { "KillScore", new Dictionary<Language, string>
                {
                    { Language.Korean, "ų ���ھ�" },
                    { Language.English, "KillScore" }
                }
            },
            { "AttackPower", new Dictionary<Language, string>
                {
                    { Language.Korean, "���ݷ�" },
                    { Language.English, "AttackPower" }
                }
            },
            { "CurrentHealth", new Dictionary<Language, string>
                {
                    { Language.Korean, "���� ü��" },
                    { Language.English, "CurrentHealth" }
                }
            },
            { "SkillGauge", new Dictionary<Language, string>
                {
                    { Language.Korean, "��ų ������" },
                    { Language.English, "SkillGauge" }
                }
            },
            { "Notice", new Dictionary<Language, string>
                {
                    { Language.Korean, "�ȳ�" },
                    { Language.English, "Notice" }
                }
            },
            { "ExitMessage", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>������ �����ϰ� Ÿ��Ʋ�� ���ư��ðڽ��ϱ�?\r\n�������� ���� ������ ���� �����˴ϴ�.</color>" },
                    { Language.English, "<color=#000000>Do you want to close the game and return to the title?\r\nAny unsaved content will be deleted.</color>" }
                }
            },
            { "ExitGame", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>������ �����Ͻðڽ��ϱ�?</color>" },
                    { Language.English, "<color=#000000>Do you want to close the game?</color>" }
                }
            },
            { "OkButton", new Dictionary<Language, string>
                {
                    { Language.Korean, "Ȯ��" },
                    { Language.English, "Ok" }
                }
            },
            { "CancelButton", new Dictionary<Language, string>
                {
                    { Language.Korean, "���" },
                    { Language.English, "Cancel" }
                }
            },
            { "EndButton", new Dictionary<Language, string>
                {
                    { Language.Korean, "����" },
                    { Language.English, "End" }
                }
            },
            { "GameOver", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#ff0000>���� ����!</color>" },
                    { Language.English, "<color=#ff0000>GameOver!</color>" }
                }
            },
            { "GameOverText", new Dictionary<Language, string>
                {
                    { Language.Korean, "�÷��̾ ����Ͽ� ������ ����Ǿ����ϴ�. \r\n" + "\"Ȯ��\" Ŭ�� �� Ÿ��Ʋ ȭ������ �̵��մϴ�. \r\n" + "\"����\" Ŭ�� �� ������ �����մϴ�." },
                    { Language.English, "The game has been shut down because the player died. \r\n" + "Click \"OK\" to navigate to the title screen. \r\n" + "Click \"End\" to exit the game." }
                }
            }
        };
    }

    protected override void OnAwake()
    {
        InitializePopUpLanguageData();
    }
}