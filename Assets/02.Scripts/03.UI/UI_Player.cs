using UnityEngine;

public class UI_Player : BaseBehaviour
{
    [Header("Health")]
    [SerializeField] private UI_PlayerHealthDisplay _healthDisplay;

    // Todo: 스킬 교체를 고려하여 개선
    [Header("Skills")]
    [SerializeField] private UI_SkillInfo _dashSkillInfo;
    [SerializeField] private UI_SkillInfo _aSkillInfo;
    [SerializeField] private UI_SkillInfo _bSkillInfo;

    [Header("Skill Icons")]
    [SerializeField] private Sprite _dashIcon;
    [SerializeField] private Sprite _plungeIcon;
    [SerializeField] private Sprite _flashStrikeIcon;

    [Header("Combo")]
    [SerializeField] private UI_ComboDisplay _comboDisplay;


    public void Initialize(Player player)
    {
        InitializeHealthDisplay(player);
        InitializeSkillUI(player);
        InitializeComboDisplay(player);
    }

    private void InitializeHealthDisplay(Player player)
    {
        _healthDisplay.Initialize(player.Health);
    }

    // Todo: 스킬 교체를 고려하여 개선
    private void InitializeSkillUI(Player player)
    {
        _dashSkillInfo.Initialize(player.Skills.Dash, _dashIcon);
        _aSkillInfo.Initialize(player.Skills.Plunge, _plungeIcon);
        _bSkillInfo.Initialize(player.Skills.FlashStrike, _flashStrikeIcon);
    }

    private void InitializeComboDisplay(Player player)
    {
        _comboDisplay.Initialize(player.Combo);
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _healthDisplay = GetComponentInChildren<UI_PlayerHealthDisplay>();
        _comboDisplay = GetComponentInChildren<UI_ComboDisplay>();
    }
    #endif
}
