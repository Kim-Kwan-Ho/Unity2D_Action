using UnityEngine;
public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _rigid.velocity = new Vector2(0, _rigid.velocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (_player.IsGrounded)
        {
            _rigid.simulated = false;
        }
    }
}
