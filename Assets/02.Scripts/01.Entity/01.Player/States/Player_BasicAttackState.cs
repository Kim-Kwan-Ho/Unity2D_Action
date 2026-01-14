using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float _attackVelocityTimer;
    private float _lastTimeAttacked;
    private bool _comboAttackQueued;
    private int _attackDirection;

    private int _comboIndex = 1;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _comboAttackQueued = false;
        ResetComboIndex();

        
        _attackDirection = _player.MoveInput != 0 ? ((int) _player.MoveInput) : _player.FacingDirection;
        _animator.SetInteger(AnimationToHash.ComboIndex, _comboIndex);
        if ( _player.MoveInput != 0)
        {
            ApplyAttackVelocity();
        }
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (_input.Character.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        if (_isTriggerCalled)
        {
            HandleStateExit();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _comboIndex++;
        _lastTimeAttacked = Time.time;
    }

    private void HandleStateExit()
    {
        if (_comboAttackQueued)
        {
            _animator.SetBool(_animationHash, false);
            _player.EnterAttackStateWithDelay();
        }
        else
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }

    private void QueueNextAttack()
    {
        if (_comboIndex < _playerStat.ComboLimit)
        {
            _comboAttackQueued = true;
        }
    }

    private void HandleAttackVelocity()
    {
        _attackVelocityTimer -= Time.deltaTime;

        if (_attackVelocityTimer < 0)
        {
            _player.SetVelocity(0, _rigid.velocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = _playerStat.AttackVelocities[_comboIndex - 1];
        _attackVelocityTimer = _playerStat.AttackVelocityDuration;
        _player.SetVelocity(attackVelocity.x * _attackDirection, attackVelocity.y);
    }

    private void ResetComboIndex()
    {
        if (Time.time > _lastTimeAttacked + _playerStat.ComboResetTime)
        {
            _comboIndex = Constants.FIRST_COMBO_INDEX;
        }

        if (_comboIndex > _playerStat.ComboLimit)
        {
            _comboIndex = Constants.FIRST_COMBO_INDEX;
        }
    }

}
