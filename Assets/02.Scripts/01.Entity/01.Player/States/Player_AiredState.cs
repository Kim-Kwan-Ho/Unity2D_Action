public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Update()
    {
        base.Update();

        HandleAiredMovement();

        if (_input.Character.Attack.WasPressedThisFrame())
        {
            _stateMachine.ChangeState(_player.AiredAttackState);
        }
        if (_input.Character.Jump.WasPressedThisFrame() && _player.CurrentJumpCount < _playerStat.MaxJumpCount)
        {
            _stateMachine.ChangeState(_player.JumpState);
        }
    }

}
