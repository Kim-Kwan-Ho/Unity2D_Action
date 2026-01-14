using UnityEngine;


[CreateAssetMenu(fileName = "AttackInfo", menuName = "ScriptableObjects/Combat/AttackInfo")]
public class AttackInfoSo : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private EAttackType _attackType;
    public EAttackType AttackType { get { return _attackType; } }


    [Header("Damage Settings")]
    [SerializeField] private EDamageType _damageType;
    public EDamageType DamageType { get { return _damageType; } }

    [SerializeField] private float _minDamage;
    public float MinDamage { get { return _minDamage; } }
    [SerializeField] private float _maxDamage;
    public float MaxDamage { get { return _maxDamage; } }

    [Header("Critical Settings")]
    [SerializeField][Range(0, 100)] private float _criticalChance;
    public float CriticalChance { get { return _criticalChance; } }
    [SerializeField] private float _criticalRatio;
    public float CriticalRatio { get { return _criticalRatio; } }


    [Header("Knockback Settings")]
    [SerializeField] private Vector2 _knockbackForce;
    public Vector2 KnockbackForce { get { return _knockbackForce; } }
    [SerializeField] private float _knockbackDuration;
    public float KnockbackDuration { get { return _knockbackDuration; } }

    [Header("Stun Settings")]
    [SerializeField] private float _stunDuration;
    public float StunDuration { get { return _stunDuration; } }

}
