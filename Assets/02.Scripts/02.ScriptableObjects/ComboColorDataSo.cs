using UnityEngine;


[CreateAssetMenu(fileName = "ComboColor", menuName = "ScriptableObjects/UI/ComboColor")]
public class ComboColorDataSo : ScriptableObject
{
    [Header("Combo Color Settings")]
    [SerializeField]
    private ComboColorData[] _comboColor;
    public ComboColorData[] ComboColor { get { return _comboColor; } }

    [Header("Add Combo Color Settings")]
    [SerializeField]
    private ComboColorData[] _addComboColor;
    public ComboColorData[] AddComboColor { get { return _addComboColor; } }

    [Header("Animation Settings")]
    [SerializeField]
    private float _fadeSpeed = 5f;
    public float FadeSpeed { get { return _fadeSpeed; } }

    [SerializeField]
    private float _addTextDuration = 1f;
    public float AddTextDuration { get { return _addTextDuration; } }

    [SerializeField]
    private float _addTextMoveDistance = 50f;
    public float AddTextMoveDistance { get { return _addTextMoveDistance; } }
}
