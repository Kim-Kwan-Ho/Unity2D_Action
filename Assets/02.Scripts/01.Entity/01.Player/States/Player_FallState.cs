public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if (_player.IsGrounded)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}
