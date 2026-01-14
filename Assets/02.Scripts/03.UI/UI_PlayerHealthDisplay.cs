using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_PlayerHealthDisplay : BaseBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _healthText;

    private Entity_Health _playerHealth;


    public void Initialize(Entity_Health playerHealth)
    {
        _playerHealth = playerHealth;
        _playerHealth.OnHealthUpdate += UpdateHealthBar;
        InitializeHealthBar();
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthUpdate -= UpdateHealthBar;
        }
    }

    private void InitializeHealthBar()
    {
        int currentHealth = Mathf.RoundToInt(_playerHealth.CurrentHealth);
        _healthText.text = currentHealth + "/" + currentHealth;
        _healthSlider.value = 1;
    }

    private void UpdateHealthBar()
    {
        int maxHealth = (int)_playerHealth.MaxHealth;
        float currentHealth = _playerHealth.CurrentHealth;
        int displayHealth;

        if (currentHealth < 1 && currentHealth > 0)
        {
            displayHealth = 1;
        }
        else if (currentHealth < 0)
        {
            displayHealth = 0;
        }
        else
        {
            displayHealth = Mathf.RoundToInt(currentHealth);
        }

        _healthText.text = displayHealth + "/" + maxHealth;
        _healthSlider.value = _playerHealth.GetHealthRatio();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _healthSlider = GetComponentInChildren<Slider>();
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
    }
#endif
}
