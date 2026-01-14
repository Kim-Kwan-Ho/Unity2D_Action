using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObjects/Stats/Player")]
public class PlayerStatSo : ScriptableObject
{
    [Header("Health")]
    [SerializeField] private float _maxHealth;
    public float MaxHealth { get { return _maxHealth; } }
    
    [Header("Movement Stat")]
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField] private float _jumpForce;
    public float JumpForce { get { return _jumpForce; } }
    [SerializeField] private int _maxJumpCount;
    public int MaxJumpCount { get { return _maxJumpCount; } }

    [Header("Combat Detail Stat")]
    [SerializeField] private int _comboLimit = 3;
    public int ComboLimit { get { return _comboLimit; } }
    [SerializeField] private Vector2[] _attackVelocities;
    public Vector2[] AttackVelocities { get { return _attackVelocities; } }
    [SerializeField] private float _attackVelocityDuration;
    public float AttackVelocityDuration { get { return _attackVelocityDuration; } }
    [SerializeField] private float _comboResetTime;
    public float ComboResetTime { get { return _comboResetTime; } }

    [Header("Attack Info")]
    [SerializeField] private AttackInfoSo[] _basicAttacks;
    public AttackInfoSo[] BasicAttacks { get { return _basicAttacks; } }
}
