public class Player_DashState : PlayerState
{
    private float _originalGravity;
    private int _dashDirection;

    public Player_DashState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (_player.MoveInput == 0)
        {
            _dashDirection = _player.FacingDirection;
        }
        else
        {
            _dashDirection = (int)_player.MoveInput;
        }
        _player.HandleFlip(_dashDirection);
        _originalGravity = _rigid.gravityScale;
        _stateTimer = _playerSkills.Dash.Data.Duration;
        _rigid.gravityScale = 0f;
        StartSkillInvincible();
        _playerSkills.Dash.UseSkill();
    }

    public override void Update()
    {
        base.Update();
        _player.SetVelocity(_playerSkills.Dash.Data.Speed * _dashDirection, 0f);
        if (_stateTimer < 0f)
        {
            ReturnToGroundedState();
        }
    }

    public override void Exit()
    {
        base.Exit();

        _player.SetVelocity(0f, 0f);
        _rigid.gravityScale = _originalGravity;
        EndSkillInvincible(_playerSkills.Dash.Data.AfterInvincibleTime);
    }
}
