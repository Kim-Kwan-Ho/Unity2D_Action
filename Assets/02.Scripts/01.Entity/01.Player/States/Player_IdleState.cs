public class Player_IdleState : Player_GroundState
{
    public Player_IdleState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetVelocity(0f, _rigid.velocity.y);
    }

    public override void Update()
    {
        base.Update();


        if (_player.MoveInput == _player.FacingDirection && _player.IsWallDetected)
            return;

        if (_player.MoveInput != 0)
            _stateMachine.ChangeState(_player.MoveState);
    }
}
