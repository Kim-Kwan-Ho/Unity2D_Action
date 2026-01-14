using UnityEngine;


[CreateAssetMenu(fileName = "EnemyStat_", menuName = "ScriptableObjects/Stats/Enemy")]
public class EnemyStatSo : ScriptableObject
{
    [Header("Health")]
    [SerializeField] private float _maxHealth;
    public float MaxHealth { get { return _maxHealth; } }
    
    [Header("Movement Stat")]
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }

    [Header("AI Stat")]
    [SerializeField] private float _minIdleTime;
    public float MinIdleTime { get { return _minIdleTime; } }
    [SerializeField] private float _maxIdleTime;
    public float MaxIdleTime { get { return _maxIdleTime; } }
    [SerializeField] private float _minMoveTime;
    public float MinMoveTime { get { return _minMoveTime; } }
    [SerializeField] private float _maxMoveTime;
    public float MaxMoveTime { get { return _maxMoveTime; } }
    [SerializeField] private float _playerCheckDistance;
    public float PlayerCheckDistance { get { return _playerCheckDistance; } }

    [Header("Attack Stat")]
    [SerializeField] private float _attackDistance;
    public float AttackDistance { get { return _attackDistance; } }
    [SerializeField] private float _attackCoolTime;
    public float AttackCoolTime { get { return _attackCoolTime; } }

    [Header("Attack Info")]
    [SerializeField] private AttackInfoSo _attackInfo;
    public AttackInfoSo AttackInfo { get { return _attackInfo; } }
}
