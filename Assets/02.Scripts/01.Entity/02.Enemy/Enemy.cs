using UnityEngine;

public class Enemy : Entity, IPoolable
{
    [Header("Data")]
    [SerializeField] private EnemyStatSo _stat;
    public EnemyStatSo Stat { get { return _stat; } }

    [Header("Enemy Components")]
    [SerializeField] protected Entity_Health _health;
    public Entity_Health Health { get { return _health; } }
    [SerializeField] protected Entity_Combat _entityCombat;
    public Entity_Combat EntityCombat { get { return _entityCombat; } }
    [SerializeField] private Enemy_SFX _sfx;
    public Enemy_SFX SFX {get { return _sfx; }}

    [Header("States")]
    private Enemy_SpawnState _spawnState;
    public Enemy_SpawnState SpawnState { get { return _spawnState; } }
    private Enemy_IdleState _idleState;
    public Enemy_IdleState IdleState { get { return _idleState; } }
    private Enemy_MoveState _moveState;
    public Enemy_MoveState MoveState { get { return _moveState; } }
    private Enemy_BattleState _battleState;
    public Enemy_BattleState BattleState { get { return _battleState; } }
    private Enemy_AttackState _attackState;
    public Enemy_AttackState AttackState { get { return _attackState; } }
    private Enemy_DeadState _deadState;
    public Enemy_DeadState DeadState { get { return _deadState; } }
    private Enemy_StunnedState _stunnedState;
    public Enemy_StunnedState StunnedState { get { return _stunnedState; } }


    [Header("Player Check")]
    [SerializeField] private Transform _playerCheckTrs;
    private Transform _player;
    public Transform Player { get { return _player; } }


    protected override void Initialize()
    {
        base.Initialize();
        InitializeStates();
        InitializeHealth();
        InitializeCombat();
    }

    protected override void InitializeStates()
    {
        _spawnState = new Enemy_SpawnState(this, _stateMachine, AnimationToHash.Spawn);
        _idleState = new Enemy_IdleState(this, _stateMachine, AnimationToHash.Idle);
        _moveState = new Enemy_MoveState(this, _stateMachine, AnimationToHash.Move);
        _attackState = new Enemy_AttackState(this, _stateMachine, AnimationToHash.Attack);
        _deadState = new Enemy_DeadState(this, _stateMachine, AnimationToHash.Dead);
        _stunnedState = new Enemy_StunnedState(this, _stateMachine, AnimationToHash.Stunned);
        _battleState = new Enemy_BattleState(this, _stateMachine, AnimationToHash.Battle);
    }
    protected override void InitializeHealth()
    {
        _health.SetMaxHealth(_stat.MaxHealth);
    }

    protected override void InitializeCombat()
    {
        _entityCombat.SetAttackInfo(_stat.AttackInfo);
    }

    private void Start()
    {
        _stateMachine.Initialize(_spawnState);
    }
    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(_playerCheckTrs.position, Vector2.right, _stat.PlayerCheckDistance, PlayerLayer);

        if (hit.collider != null)
        {
            _player = hit.transform;
            return hit;
        }

        hit = Physics2D.Raycast(_playerCheckTrs.position, Vector2.left, _stat.PlayerCheckDistance, PlayerLayer);

        if (hit.collider != null)
        {
            _player = hit.transform;
            return hit;
        }

        return default;
    }

    public virtual void PerformSpecialAttack()
    {

    }

    private void ResetAttackCooldown()
    {
        _battleState.ResetAttackCooldown();
    }

    public override void ReceiveStunKnockback(Vector2 knockbackForce, float stunDuration)
    {
        ResetAttackCooldown();
        _stunnedDuration = stunDuration;
        _stateMachine.ChangeState(_stunnedState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        _stateMachine.ChangeState(_deadState);
    }

    public void SetStat(EnemyStatSo newStat)
    {
        _stat = newStat;
        InitializeHealth();
        InitializeCombat();
    }

    public void OnSpawnFromPool()
    {
        ResetEntity();

        _health.SetMaxHealth(_stat.MaxHealth);
        _health.ResetHealth();
        _sfx.ResetEffect();
        _stateMachine.Initialize(_spawnState);
    }

    public void OnReturnToPool()
    {
        StopAllCoroutines();

        if (_stateMachine != null)
        {
            _stateMachine.SwitchOffStateMachine();
        }

        _player = null;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _health = GetComponent<Entity_Health>();
        _entityCombat = GetComponent<Entity_Combat>();
        _sfx = GetComponent<Enemy_SFX>();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_playerCheckTrs.position + Vector3.down * 0.1f, _playerCheckTrs.position + Vector3.right * _stat.PlayerCheckDistance + Vector3.down * 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_playerCheckTrs.position + Vector3.up * 0.1f, _playerCheckTrs.position + Vector3.right * _stat.AttackDistance + Vector3.up * 0.1f);
    }
#endif
}
