public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.IncreaseJumpCount();
        _player.SetVelocity(_rigid.velocity.x, _playerStat.JumpForce);
    }

    public override void Update()
    {
        base.Update();
        if (_rigid.velocity.y < 0 && _stateMachine.CurrentState != _player.AiredAttackState)
        {
            _stateMachine.ChangeState(_player.FallState);
        }
        else if (_rigid.velocity.y == 0 && _player.IsGrounded)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}
