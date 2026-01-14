using UnityEngine;

public abstract class EntityState
{
    [Header("Components")]    
    protected Animator _animator;
    protected Rigidbody2D _rigid;

    [Header("States")]
    protected StateMachine _stateMachine;
    protected int _animationHash;
    protected float _stateTimer;
    protected bool _isTriggerCalled;

    public EntityState(StateMachine stateMachine, int animationHash)
    {
        _stateMachine = stateMachine;
        _animationHash = animationHash;
    }

    public virtual void Enter()
    {
        _animator.SetBool(_animationHash, true);
        _isTriggerCalled = false;
    }

    public virtual void Update()
    {
        _stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        _animator.SetBool(_animationHash, false);
    }

    public void AnimationTrigger()
    {
        _isTriggerCalled = true;
    }
    
    public virtual void UpdateAnimationParameters()
    {

    }
}
