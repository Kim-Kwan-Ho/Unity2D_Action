public class PlayerState : EntityState
{
    protected Player _player;
    protected PlayerStatSo _playerStat;
    protected Player_Skills _playerSkills;
    protected Player_Input _input;
    public PlayerState(Player player, StateMachine stateMachine, int animationHash) : base(stateMachine, animationHash)
    {
        _player = player;
        _animator = _player.Animator;
        _rigid = _player.Rigid;
        _playerSkills = _player.Skills;
        _playerStat = _player.Stat;
        _input = _player.Input;
    }

    // Todo1: Skill을 직접 지정하는 것이 아닌, 해당 슬롯에 있는 스킬을 사용하도록 개선 (스킬 변경을 고려하여 개선)
    // Todo2: 입력 버퍼를 제작해 동시 키 입력 처리 및 선입력 관련 고려할 것
    public override void Update()
    {
        base.Update();

        if (_input.Character.Dash.WasPressedThisFrame() && CanDash())
        {
            _stateMachine.ChangeState(_player.DashState);
            return;
        }
        if (_input.Character.ASkill.WasPressedThisFrame() && CanPlunge())
        {
            _stateMachine.ChangeState(_player.PlungeState);
            return;
        }
        if (_input.Character.SSkill.WasPressedThisFrame() && CanFlashStrike())
        {
            _stateMachine.ChangeState(_player.FlashStrikeState);
            return;
        }
    }

    private bool CanUseSkill(SkillBase skill, EntityState skillState)
    {
        if (!skill.CanUseSkill())
            return false;
        if (_stateMachine.CurrentState == skillState)
            return false;
        return true;
    }

    private bool CanDash()
    {
        return CanUseSkill(_playerSkills.Dash, _player.DashState);
    }
    private bool CanFlashStrike()
    {
        return CanUseSkill(_playerSkills.FlashStrike, _player.FlashStrikeState);
    }
    private bool CanPlunge()
    {
        return  CanUseSkill(_playerSkills.Plunge, _player.PlungeState);
    }


    protected void HandleAiredMovement()
    {
        if (_player.MoveInput == _player.FacingDirection && _player.IsWallDetected)
        {
            // Todo: 벽타기 추가
        }
        else if (_player.MoveInput != 0)
        {
            _player.SetVelocity(_player.MoveInput * _playerStat.MoveSpeed, _rigid.velocity.y);
        }
    }

    protected void ReturnToGroundedState()
    {
        if (_player.IsGrounded)
            _stateMachine.ChangeState(_player.IdleState);
        else
            _stateMachine.ChangeState(_player.FallState);
    }


    protected void StartSkillInvincible()
    {
        _player.ChangeInvincible(true, 0);
    }


    protected void EndSkillInvincible(float afterInvincibleTime)
    {
        _player.ChangeInvincible(false, afterInvincibleTime);
    }

}
