using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LanguageManager : SingletonDontDestroy<LanguageManager>
{
    public enum Language
    {
        Korean,
        English
    }

    Dictionary<string, Dictionary<Language, string>> localizedTexts;
    
    public Language CurrentLanguage { get; private set; } = Language.Korean;

    public event Action OnLanguageChanged;

    public void SetLanguage(Language language)
    {
        CurrentLanguage = language;
        OnLanguageChanged?.Invoke();     // 언어 변경됨을 알림
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
            case "GameOption":
                return CurrentLanguage == Language.Korean ? "게임 옵션" : "GameOption";
            case "Sound":
                return CurrentLanguage == Language.Korean ? "소리" : "Sound";
            case "CameraShake":
                return CurrentLanguage == Language.Korean ? "카메라 진동" : "CameraShake";
            case "Language":
                return CurrentLanguage == Language.Korean ? "언어" : "Language";
            case "PlayerSettings":
                return CurrentLanguage == Language.Korean ? "플레이어 설정" : "Player Settings";
            case "PlayerType":
                return CurrentLanguage == Language.Korean ? "▶ 플레이할 주인공 타입을 정하세요." : "▶ Choose the type of protagonist \r\nyou want to play.";
            case "IntroduceWarrior":
                return CurrentLanguage == Language.Korean ? "< 근거리 플레이어 >\r\n\r\n강력한 근접 콤보와 스킬 공격 가능\r\n\r\n체력: 200\r\n공격력: 30\r\n방어력: 10\r\n\r\n해당 플레이어로 \r\n플레이하시겠습니까?" : "< Close Range Player >\r\n\r\nPowerful combos and skill attacks\r\n\r\nPhysical strength: 200\r\nAttacks: 30\r\nDefensive: 10\r\n\r\nWith that player\r\nDo you want to play?";
            case "IntroduceRange":
                return CurrentLanguage == Language.Korean ? "< 원거리 플레이어 >\r\n\r\n먼 거리에서 적을 공격하며 생존력을 보유\r\n\r\n체력: 180\r\n공격력: 25\r\n방어력: 8\r\n\r\n해당 플레이어로 \r\n플레이하시겠습니까?" : "< Long Distance Player >\r\n\r\nAttacking the enemy from a long distance and being able to survive\r\n\r\nPhysical strength: 180\r\nAttack Power: 25\r\nDefensive: 8\r\n\r\nWith that player\r\nDo you want to play?";
            case "PlayerNameTitle":
                return CurrentLanguage == Language.Korean ? "플레이어 이름 설정" : "Set Player Name";
            case "PlayerNameRule":
                return CurrentLanguage == Language.Korean ? "▶ 플레이할 주인공의 이름을 정해주세요.\r\n     ( 단, 영문 및 공백 포함 8글자 이내 )" : "▶ Please choose the name of the\r\n     main character.\r\n ";
            case "PlaceHolder":
                return CurrentLanguage == Language.Korean ? "  이름을 입력해주세요" : "  Please enter your name";
            case "MovePlayer":
                return CurrentLanguage == Language.Korean ? "마우스 좌 클릭으로 캐릭터를 회전시킬 수 있습니다." : "You can rotate the character \r\nwith a left mouse click.";
            case "Previous":
                return CurrentLanguage == Language.Korean ? "이전" : "Previous";
            case "GameOn":
                return CurrentLanguage == Language.Korean ? "게임시작" : "Game On";
            case "WeaponSetting":
                return CurrentLanguage == Language.Korean ? "플레이어 무기 설정" : "Player Weapon Settings";
            case "WeaponSelect":
                return CurrentLanguage == Language.Korean ? "▶ 플레이할 주인공의 무기를 정해주세요." : "▶ Please choose the weapon";
            case "PleaseInputName":
                return CurrentLanguage == Language.Korean ? "이름을 입력해주세요!" : "Please enter your name!";
            case "InputNameLength":
                return CurrentLanguage == Language.Korean ? "이름은 8글자 이내로 입력해주세요!" : "Please enter your name within 8 characters!";
            case "PleaseSelectWeapon":
                return CurrentLanguage == Language.Korean ? "무기를 선택해주세요!" : "Please choose your weapon!";
            case "InputNameRule":
                return CurrentLanguage == Language.Korean ? "영문,공백 포함 8글자 이내로 입력해주세요!" : "Please enter within 8 characters including English and spaces!";
        }
        return key;
    }

    void InitializeLanguageData()
    {
        localizedTexts = new Dictionary<string, Dictionary<Language, string>>
        {
            { "GameGuideTitle", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>게임 안내</color>" },
                    { Language.English, "<color=#000000>Game Guide</color>" }
                }
            },
            { "PlayerInfoTitle", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>플레이어 정보</color>" },
                    { Language.English, "<color=#000000>Player Info</color>" }
                }
            },
            { "CommonGameInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "< 게임 정보 >\r\n모든 적과 보스를 해치우는 게임입니다.\r\n맵이 변경되도 정보는 저장됩니다.\r\n적을 해치우면 랜덤으로 효과를 얻습니다.\r\n ▶ 25% 확률: <color=#ff0000>체력</color> 증가 (+20)\r\n ▶ 25% 확률: <color=#ff0000>스킬 게이지</color> 증가 (+10)\r\n ▶ 25% 확률: <color=#ff0000>공격력</color> 증가 (+2)\r\n\n" },
                    { Language.English, "< Game Info >\r\nThis is a game where you defeat all enemies and bosses.\r\nYour progress is saved even if the map changes.\r\nDefeating enemies grants random effects.\r\n ▶ 25% chance: <color=#ff0000>Health</color> increase (+20)\r\n ▶ 25% chance: <color=#ff0000>Skill gauge</color> increase (+10)\r\n ▶ 25% chance: <color=#ff0000>Attack power</color> increase (+2)\r\n\n" }
                }
            },
            { "BasicKeyInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "< 기본 키 >\r\n[V] 볼륨 조절창을 On/Off 할 수 있습니다.\r\n[Tab] 플레이어의 정보를 확인할 수 있습니다.\r\n[Left Shift] 플레이어의 이동속도가 증가합니다.\r\n[C] 플레이어가 무기를 들어 방어합니다.\r\n" },
                    { Language.English, "< Basic Keys >\r\n[V] Toggle volume control window On/Off.\r\n[Tab] Check player information.\r\n[Left Shift] Increases player movement speed.\r\n[C] Player raises a weapon to defend.\r\n" }
                }
            },
            { "WarriorSkillInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "[Space]\r\n ▶ <color=#ff0000>콤보 공격</color>(4회 중첩)이 가능합니다.\r\n\n< 스킬 공격 >\r\n[Z] ▶ 기본 공격으로 게이지가 쌓입니다.\r\n ▶ Z키를 통해 적을 <color=#ff0000>스턴</color> 시킬 수 있습니다.\r\n[X] ▶ 20초의 쿨타임을 가지는 스킬입니다.\r\n ▶ X키를 통해 적을 <color=#ff0000>넉백</color> 시킬 수 있습니다.\r\n\n" },
                    { Language.English, "[Space]\r\n ▶ <color=#ff0000>Combo Attack</color>(4 stacks) is possible.\r\n\n< Skill Attacks >\r\n[Z] ▶ Gauge fills with basic attacks.\r\n ▶ Press Z to <color=#ff0000>stun</color> enemies.\r\n[X] ▶ Has a 20-second cooldown.\r\n ▶ Press X to <color=#ff0000>knockback</color> enemies.\r\n\n" }
                }
            },
            { "RangeSkillInfo", new Dictionary<Language, string>
                {
                    { Language.Korean, "[Space]\r\n ▶ <color=#ff0000>기본 공격</color> 입니다.\r\n\n< 스킬 공격 >\r\n[Z] ▶ 기본 공격으로 게이지가 쌓입니다.\r\n ▶ 기본 공격을 동시에 <color=#ff0000>3발</color> 발사합니다.\r\n[X] ▶ 20초의 쿨타임을 가지는 스킬입니다.\r\n ▶ 범위 스킬로 적들을 <color=#ff0000>스턴</color> 시킵니다.\r\n\n" },
                    { Language.English, "[Space]\r\n ▶ <color=#ff0000>Basic Attack</color>.\r\n\n< Skill Attacks >\r\n[Z] ▶ Gauge fills with basic attacks.\r\n ▶ Fires <color=#ff0000>3 arrows</color> simultaneously.\r\n[X] ▶ Has a 20-second cooldown.\r\n ▶ AoE skill <color=#ff0000>stuns</color> enemies.\r\n\n" }
                }
            },
            { "CurrentScene", new Dictionary<Language, string>
                {
                    { Language.Korean, "현재 씬" },
                    { Language.English, "CurrentScene" }
                }
            },
            { "PlayerType", new Dictionary<Language, string>
                {
                    { Language.Korean, "플레이어 타입" },
                    { Language.English, "PlayerType" }
                }
            },
            { "KillScore", new Dictionary<Language, string>
                {
                    { Language.Korean, "킬 스코어" },
                    { Language.English, "KillScore" }
                }
            },
            { "AttackPower", new Dictionary<Language, string>
                {
                    { Language.Korean, "공격력" },
                    { Language.English, "AttackPower" }
                }
            },
            { "CurrentHealth", new Dictionary<Language, string>
                {
                    { Language.Korean, "현재 체력" },
                    { Language.English, "CurrentHealth" }
                }
            },
            { "SkillGauge", new Dictionary<Language, string>
                {
                    { Language.Korean, "스킬 게이지" },
                    { Language.English, "SkillGauge" }
                }
            },
            { "Notice", new Dictionary<Language, string>
                {
                    { Language.Korean, "안내" },
                    { Language.English, "Notice" }
                }
            },
            { "ExitMessage", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>게임을 종료하고 타이틀로 돌아가시겠습니까?\r\n저장하지 않은 내용은 전부 삭제됩니다.</color>" },
                    { Language.English, "<color=#000000>Do you want to close the game and return to the title?\r\nAny unsaved content will be deleted.</color>" }
                }
            },
            { "ExitGame", new Dictionary<Language, string>
                {
                    { Language.Korean, "<color=#000000>게임을 종료하시겠습니까?</color>" },
                    { Language.English, "<color=#000000>Do you want to close the game?</color>" }
                }
            },
            { "OkButton", new Dictionary<Language, string>
                {
                    { Language.Korean, "확인" },
                    { Language.English, "Ok" }
                }
            },
            { "CancelButton", new Dictionary<Language, string>
                {
                    { Language.Korean, "취소" },
                    { Language.English, "Cancel" }
                }
            },
        };
    }

    protected override void OnAwake()
    {
        InitializeLanguageData();
    }
}