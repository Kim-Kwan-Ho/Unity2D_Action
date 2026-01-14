public class Player_AiredAttackState : PlayerState
{

    public Player_AiredAttackState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Update()
    {
        base.Update();

        HandleAiredMovement();

        if (_isTriggerCalled)
        {
            ReturnToGroundedState();
        }
    }
}
