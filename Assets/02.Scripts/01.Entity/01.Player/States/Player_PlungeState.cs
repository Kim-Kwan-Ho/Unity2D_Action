using UnityEngine;

public class Player_PlungeState : PlayerState
{
    private bool _touchGround;
    public Player_PlungeState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _rigid.velocity = Vector2.zero;
        _touchGround = false;
        StartSkillInvincible();
    }
    public override void Update()
    {
        _rigid.velocity = new Vector2(0, -_playerSkills.Plunge.Data.PlungeSpeed);
        if (_player.IsGrounded && _touchGround == false)
        {
            _touchGround = true;
            _playerSkills.Plunge.UseSkill();
            _player.SetVelocity(0, 0);
        }
        else if (_player.IsGrounded)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        EndSkillInvincible(_playerSkills.Plunge.Data.AfterInvincibleTime);

    }
}
