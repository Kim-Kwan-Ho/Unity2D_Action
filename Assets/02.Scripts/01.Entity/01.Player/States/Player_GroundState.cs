public class Player_GroundState : PlayerState
{
    public Player_GroundState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.ResetJumpCount();
    }
    public override void Update()
    {
        base.Update();

        if (_rigid.velocity.y < 0 && !_player.IsGrounded)
        {
            _stateMachine.ChangeState(_player.FallState);
        }
        if (_input.Character.Jump.WasPressedThisFrame())
        {
            _stateMachine.ChangeState(_player.JumpState);
        }
        if (_input.Character.Attack.WasPressedThisFrame())
        {
            _stateMachine.ChangeState(_player.BasicAttackState);
        }
    }
}
