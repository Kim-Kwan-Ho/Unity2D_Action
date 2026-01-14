using UnityEngine;
using TMPro;
using System.Collections;


public class UI_ComboDisplay : BaseBehaviour
{
    [Header("Data")]
    [SerializeField] private ComboColorDataSo _colorData;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _comboAddText;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Player_ComboSystem _comboSystem;
    private int _currentCombo;
    private float _targetAlpha;
    private Vector3 _addTextInitialPosition;


    public void Initialize(Player_ComboSystem comboSystem)
    {
        _comboSystem = comboSystem;
        _comboSystem.OnComboChanged += OnComboChanged;
        _currentCombo = 0;
        _targetAlpha = 0f;
        _canvasGroup.alpha = 0f;
        _addTextInitialPosition = _comboAddText.transform.localPosition;
        _comboAddText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_comboSystem != null)
        {
            _comboSystem.OnComboChanged -= OnComboChanged;
        }
    }

    private void Update()
    {
        _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _targetAlpha, Time.deltaTime * _colorData.FadeSpeed);
    }

    private void OnComboChanged(int newCombo, int addedAmount)
    {
        _currentCombo = newCombo;

        if (_currentCombo <= 0)
        {
            _targetAlpha = 0f;
            _comboText.text = "";
        }
        else
        {
            _targetAlpha = 1f;
            _comboText.text = $"{_currentCombo} COMBO";
            _comboText.color = GetColorByValue(_currentCombo, _colorData.ComboColor);
            if (addedAmount > 0)
            {
                ShowComboAddText(addedAmount);
            }
        }
    }

    private void ShowComboAddText(int amount)
    {
        StopAllCoroutines();
        StartCoroutine(CoShowComboAddText(amount));
    }

    private IEnumerator CoShowComboAddText(int amount)
    {
        _comboAddText.transform.localPosition = _addTextInitialPosition;
        _comboAddText.text = $"+{amount} COMBO";
        _comboAddText.gameObject.SetActive(true);

        Color color = GetColorByValue(amount, _colorData.AddComboColor);
        color.a = 1f;
        _comboAddText.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < _colorData.AddTextDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _colorData.AddTextDuration;

            Vector3 newPos = _addTextInitialPosition + Vector3.up * (_colorData.AddTextMoveDistance * progress);
            _comboAddText.transform.localPosition = newPos;

            color.a = 1f - progress;
            _comboAddText.color = color;

            yield return null;
        }

        _comboAddText.gameObject.SetActive(false);
    }

    private Color GetColorByValue(int value, ComboColorData[] comboColors)
    {
        if (comboColors == null || comboColors.Length == 0)
            return Color.white;

        Color resultColor = comboColors[0].Color;

        for (int i = 0; i < comboColors.Length; i++)
        {
            if (value >= comboColors[i].ComboCount)
            {
                resultColor = comboColors[i].Color;
            }
            else
            {
                break;
            }
        }

        return resultColor;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _comboText = FindGameObjectInChildren<TextMeshProUGUI>("ComboText");
        _comboAddText = FindGameObjectInChildren<TextMeshProUGUI>("ComboAddText");
        _canvasGroup = GetComponent<CanvasGroup>();
    }
#endif
}
