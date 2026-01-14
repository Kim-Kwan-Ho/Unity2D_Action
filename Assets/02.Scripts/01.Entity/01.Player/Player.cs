using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Data")]
    [SerializeField] private PlayerStatSo _stat;
    public PlayerStatSo Stat { get { return _stat; } }

    [Header("Player Components")]
    [SerializeField] protected Player_Skills _skills;
    public Player_Skills Skills { get { return _skills; } }
    [SerializeField] protected Entity_Health _health;
    public Entity_Health Health { get { return _health; } }
    [SerializeField] protected Player_Combat _combat;
    public Player_Combat Combat { get { return _combat; } }
    [SerializeField] protected Player_SFX _sfx;
    public Player_SFX SFX { get { return _sfx; } }
    [SerializeField] protected Player_ComboSystem _combo;
    public Player_ComboSystem Combo { get { return _combo; } }
    private Player_Input _input;
    public Player_Input Input { get { return _input; } }
    [SerializeField] private UI_Player _ui;
    public UI_Player UI {get { return _ui; }}

    [Header("States")]
    private Player_IdleState _idleState;
    public Player_IdleState IdleState { get { return _idleState; } }
    private Player_MoveState _moveState;
    public Player_MoveState MoveState { get { return _moveState; } }
    private Player_JumpState _jumpState;
    public Player_JumpState JumpState { get { return _jumpState; } }
    private Player_FallState _fallState;
    public Player_FallState FallState { get { return _fallState; } }
    private Player_AiredAttackState _airedAttackState;
    public Player_AiredAttackState AiredAttackState { get { return _airedAttackState; } }
    private Player_DashState _dashState;
    public Player_DashState DashState { get { return _dashState; } }
    private Player_BasicAttackState _basicAttackState;
    public Player_BasicAttackState BasicAttackState { get { return _basicAttackState; } }
    private Player_DeadState _deadState;
    public Player_DeadState DeadState { get { return _deadState; } }
    private Player_PlungeState _plungeState;
    public Player_PlungeState PlungeState { get { return _plungeState; } }
    private Player_FlashStrikeState _flashStrikeState;
    public Player_FlashStrikeState FlashStrikeState { get { return _flashStrikeState; } }



    [Header("State Environmnets")]
    private float _moveInput;
    public float MoveInput { get { return _moveInput; } }
    private int _currentJumpCount;
    public int CurrentJumpCount { get { return _currentJumpCount; } }
    private Coroutine _attackRoutine;
    private Coroutine _invincibleRoutine;


    #region Initialize
    protected override void Initialize()
    {
        base.Initialize();
        InitializeInput();
        InitializeStates();
        InitializeHealth();
        InitializeCombat();
        InitializeUI();
    }
    private void InitializeInput()
    {
        _input = new Player_Input();
        _input.Enable();
        _input.Character.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>().x;
        _input.Character.Move.canceled += ctx => _moveInput = 0;
    }
    protected override void InitializeStates()
    {
        _idleState = new Player_IdleState(this, _stateMachine, AnimationToHash.Idle);
        _moveState = new Player_MoveState(this, _stateMachine, AnimationToHash.Move);
        _jumpState = new Player_JumpState(this, _stateMachine, AnimationToHash.Jump);
        _fallState = new Player_FallState(this, _stateMachine, AnimationToHash.Fall);
        _airedAttackState = new Player_AiredAttackState(this, _stateMachine, AnimationToHash.AiredAttack);
        _dashState = new Player_DashState(this, _stateMachine, AnimationToHash.Dash);
        _basicAttackState = new Player_BasicAttackState(this, _stateMachine, AnimationToHash.BasicAttack);
        _deadState = new Player_DeadState(this, _stateMachine, AnimationToHash.Dead);
        _plungeState = new Player_PlungeState(this, _stateMachine, AnimationToHash.Plunge);
        _flashStrikeState = new Player_FlashStrikeState(this, _stateMachine, AnimationToHash.FlashStrike);
    }
    protected override void InitializeHealth()
    {
        _health.SetMaxHealth(_stat.MaxHealth);
    }
    protected override void InitializeCombat()
    {
        _combat.SetAttackInfos(_stat.BasicAttacks);
    }
    private void InitializeUI()
    {
        _ui.Initialize(this);
    }
    private void Start()
    {
        _stateMachine.Initialize(_idleState);
    }
    #endregion
    #region States
    #region Attack
    public void EnterAttackStateWithDelay()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(CoEnterAttackStateWithDelay());
    }

    private IEnumerator CoEnterAttackStateWithDelay()
    {
        yield return new WaitForEndOfFrame();
        _stateMachine.ChangeState(_basicAttackState);
    }
    #endregion
    #region Death
    public override void EntityDeath()
    {
        base.EntityDeath();
        _input.Disable();
        _stateMachine.ChangeState(_deadState);
        GameManager.Instance.OnPlayerDeath();
    }
    #endregion
    #region Jump
    public void ResetJumpCount()
    {
        _currentJumpCount = 0;
    }
    public void IncreaseJumpCount()
    {
        _currentJumpCount++;
    }
    #endregion
    #region Invincible
    public void ChangeInvincible(bool isInvincible, float delayTime)
    {
        // 기존 무적 코루틴 중지
        if (_invincibleRoutine != null)
        {
            StopCoroutine(_invincibleRoutine);
            _invincibleRoutine = null;
        }

        if (delayTime > 0f)
        {
            _invincibleRoutine = StartCoroutine(CoChangeInvincibleWithDelay(isInvincible, delayTime));
        }
        else
        {
            ApplyInvincible(isInvincible);
        }
    }
    private IEnumerator CoChangeInvincibleWithDelay(bool isInvincible, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ApplyInvincible(isInvincible);
        _invincibleRoutine = null;
    }
    private void ApplyInvincible(bool isInvincible)
    {
        if (isInvincible)
        {
            _health.SetCanTakeDamage(false);
            SetLayerToUntargetable();
        }
        else
        {
            _health.SetCanTakeDamage(true);
            SetLayerToDefault();
        }
    }
    #endregion
    #endregion
    public void ResetPlayer(Vector3 initialPosition, Quaternion initialRotation)
    {
        _input.Disable();
        StopAllCoroutines();
        if (_stateMachine.CurrentState != null)
        {
            _stateMachine.CurrentState.Exit();
        }
        ChangeInvincible(false, 0);
        if (_rigid != null)
        {
            _rigid.velocity = Vector2.zero;
            _rigid.angularVelocity = 0f;
            _rigid.simulated = true;
        }
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        ResetEntity();
        _health.ResetHealth();
        _skills.ResetAllSkillCooldowns();
        _combo.ResetCombo();
        _sfx.ResetEffect();
        ResetJumpCount();
        
        if (_animator != null)
        {
            _animator.SetBool(AnimationToHash.Dead, false);
            _animator.Rebind();
            _animator.Update(0f);
        }
        _stateMachine.Initialize(_idleState);
        _input.Enable();
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _skills = GetComponent<Player_Skills>();
        _health = GetComponent<Entity_Health>();
        _combat = GetComponent<Player_Combat>();
        _sfx = GetComponent<Player_SFX>();
        _combo = GetComponent<Player_ComboSystem>();
        _ui = FindObjectOfType<UI_Player>();
    }
#endif
}
