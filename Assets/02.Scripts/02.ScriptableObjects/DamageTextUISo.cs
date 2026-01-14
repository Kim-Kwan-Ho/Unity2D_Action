using UnityEngine;


[CreateAssetMenu(fileName = "DamageTextUI", menuName = "ScriptableObjects/UI/DamageText")]
public class DamageTextUISo : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private float _lifeTime;
    public float LifeTime { get { return _lifeTime; } }
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField] private float _criticalSizeRatio;
    public float CriticalSizeRatio { get { return _criticalSizeRatio; } }

    [Header("Player Hit")]
    [SerializeField] private Color _pHitNormalColor;
    public Color PHitNormalColor { get { return _pHitNormalColor; } }
    [SerializeField] private Color _pHitCriticalNormalColor;
    public Color PHitCriticalNormalColor { get { return _pHitCriticalNormalColor; } }

    [Header("Enemy Hit")]
    [SerializeField] private Color _eHitNormalColor;
    public Color EHitNormalColor { get { return _eHitNormalColor; } }
    [SerializeField] private Color _eHitCriticalNormalColor;
    public Color EHitCriticalNormalColor { get { return _eHitCriticalNormalColor; } }
    [SerializeField] private Color _eHitSkillColor;
    public Color EHitSkillColor { get { return _eHitSkillColor; } }
    [SerializeField] private Color _eHitCriticalSkillColor;
    public Color EHitCriticalSkillColor { get { return _eHitCriticalSkillColor; } }


}
