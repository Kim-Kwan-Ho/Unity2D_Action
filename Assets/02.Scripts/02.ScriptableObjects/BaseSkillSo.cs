using UnityEngine;

public class BaseSkillSo : ScriptableObject
{
    [SerializeField] private float _coolTime;
    public float CoolTime { get { return _coolTime; } }
    [SerializeField] private float _afterInvincibleTime;
    public float AfterInvincibleTime { get { return _afterInvincibleTime; } }
}
