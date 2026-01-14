using System;
using System.Collections;
using UnityEngine;

public abstract class Entity : BaseBehaviour
{
    public event Action OnFlipped;

    [Header("Components")]
    [SerializeField] protected Animator _animator;
    public Animator Animator { get { return _animator; } }
    [SerializeField] protected Rigidbody2D _rigid;
    public Rigidbody2D Rigid { get { return _rigid; } }
    protected StateMachine _stateMachine;



    [Header("Flip")]
    private bool _isFacingRight;
    private int _facingDirection;
    public int FacingDirection { get { return _facingDirection; } }

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDist;
    protected bool _isGrounded;
    public bool IsGrounded { get { return _isGrounded; } }


    [Header("WallCheck")]
    [SerializeField] private float _wallCheckDist;
    private bool _isWallDetected;
    public bool IsWallDetected { get { return _isWallDetected; } }


    [Header("Knockback")]
    [SerializeField] private EStanceType _stanceType;
    public EStanceType StanceType { get { return _stanceType; } }
    [SerializeField] private Coroutine _knockbackRoutine;

    [Header("Stun")]
    protected float _stunnedDuration;
    public float StunnedDuration { get { return _stunnedDuration; } }
    private bool _isKnockbacked;

    [Header("Layer Setting")]
    [SerializeField] private LayerMask _groundWallLayer;
    [SerializeField] private LayerMask _playerLayer;
    public LayerMask PlayerLayer { get { return _playerLayer; } }
    private int _defaultLayerValue;
    protected override void Initialize()
    {
        base.Initialize();
        _stateMachine = new StateMachine();
        _isFacingRight = true;
        _facingDirection = 1;
        _isKnockbacked = false;
        _defaultLayerValue = gameObject.layer;
    }
    protected abstract void InitializeStates();
    protected abstract void InitializeHealth();
    protected abstract void InitializeCombat();
    public void CurrentStateAnimationTrigger()
    {
        _stateMachine.CurrentState.AnimationTrigger();
    }


    public void SetVelocity(float velX, float velY)
    {
        if (_isKnockbacked)
            return;
        _rigid.velocity = new Vector2(velX, velY);
        HandleFlip(velX);
    }

    public void HandleFlip(float velX)
    {
        if (velX > 0 && !_isFacingRight)
            Flip();
        else if (velX < 0 && _isFacingRight)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        _isFacingRight = !_isFacingRight;
        _facingDirection *= -1;

        OnFlipped?.Invoke();
    }



    protected virtual void Update()
    {
        HandleCollisionDetection();
        _stateMachine.UpdateActiveState();
    }
    private void HandleCollisionDetection()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDist, _groundWallLayer);
        _isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * _facingDirection, _wallCheckDist, _groundWallLayer);
    }

    public void ReceiveKnockback(Vector2 knockback, float duration)
    {
        if (_knockbackRoutine != null)
        {
            StopCoroutine(_knockbackRoutine);
        }
        _knockbackRoutine = StartCoroutine(CoKnockback(knockback, duration));
    }
    private IEnumerator CoKnockback(Vector2 knockback, float duration)
    {
        _isKnockbacked = true;
        _rigid.velocity = new Vector2(knockback.x, _rigid.velocity.y + knockback.y);
        yield return new WaitForSeconds(duration);
        _isKnockbacked = false;
    }
    public virtual void ReceiveStunKnockback(Vector2 stunKnockback, float stunDuration)
    {

    }

    public virtual void EntityDeath()
    {
        gameObject.layer = LayerMask.NameToLayer("Untargetable");
    }

    public void ResetEntity()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.y = 0f;
        transform.eulerAngles = rotation;

        _isFacingRight = true;
        _facingDirection = 1;
        _isKnockbacked = false;

        if (_knockbackRoutine != null)
        {
            StopCoroutine(_knockbackRoutine);
            _knockbackRoutine = null;
        }

        gameObject.layer = _defaultLayerValue;
        _rigid.velocity = Vector2.zero;
    }

    public void SetLayerToUntargetable()
    {
        gameObject.layer = LayerMask.NameToLayer("Untargetable");
    }

    public void SetLayerToDefault()
    {
        gameObject.layer = _defaultLayerValue;
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponentInChildren<Rigidbody2D>();
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDist);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallCheckDist);

    }
#endif
}
