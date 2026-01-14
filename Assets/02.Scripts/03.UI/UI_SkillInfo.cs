using UnityEngine;
using UnityEngine.UI;

public class UI_SkillInfo : BaseBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _skillImage;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private Image _disabledOverlay;

    private SkillBase _skill;
    private bool _isOnCooldown;

    public void Initialize(SkillBase skill, Sprite icon)
    {
        _skill = skill;
        _skillImage.sprite = icon;

        _skill.OnCooldownStart += OnCooldownStarted;
        _skill.OnCooldownEnd += OnCooldownEnded;

        SetCooldownUI(false);
        UpdateAvailabilityUI();
    }

    private void OnDestroy()
    {
        if (_skill != null)
        {
            _skill.OnCooldownStart -= OnCooldownStarted;
            _skill.OnCooldownEnd -= OnCooldownEnded;
        }
    }

    private void OnCooldownStarted()
    {
        _isOnCooldown = true;
        SetCooldownUI(true);
    }

    private void OnCooldownEnded()
    {
        _isOnCooldown = false;
        SetCooldownUI(false);
    }

    private void Update()
    {
        if (_isOnCooldown)
            UpdateCooldownUI();

        UpdateAvailabilityUI();
    }

    private void UpdateCooldownUI()
    {
        _cooldownImage.fillAmount = _skill.GetCooldownRatio();
    }

    private void UpdateAvailabilityUI()
    {
        bool canUse = _skill.CanUseSkill();

        if (_disabledOverlay != null)
            _disabledOverlay.gameObject.SetActive(!canUse && !_isOnCooldown);
    }

    private void SetCooldownUI(bool isOnCooldown)
    {
        _cooldownImage.gameObject.SetActive(isOnCooldown);
        if (!isOnCooldown)
            _cooldownImage.fillAmount = 0;
    }
}
