public class Player_MoveState : Player_GroundState
{
    public Player_MoveState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Update()
    {
        base.Update();


        if (_player.MoveInput == 0)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
        _player.SetVelocity(_player.MoveInput * _playerStat.MoveSpeed, _rigid.velocity.y);
    }
}