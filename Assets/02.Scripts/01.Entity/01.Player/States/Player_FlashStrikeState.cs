using System.Collections;
using UnityEngine;

public class Player_FlashStrikeState : PlayerState
{
    private bool _skillExecuted;

    public Player_FlashStrikeState(Player player, StateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _skillExecuted = false;
        _player.SetVelocity(0, 0);
        StartSkillInvincible();
        _playerSkills.FlashStrike.UseSkill();
        _player.StartCoroutine(CoExecuteFlashStrike());
    }

    private IEnumerator CoExecuteFlashStrike()
    {
        Vector3 destPos;
        _playerSkills.FlashStrike.StartFlashStrike(_player.transform.position, _player.FacingDirection, out destPos);
        _player.Rigid.position = destPos;
        yield return new WaitForFixedUpdate();
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(_playerSkills.FlashStrike.Data.FreezeTime);
        _playerSkills.FlashStrike.ExecuteFlashStrike();
        Time.timeScale = 1f;
        _skillExecuted = true;
    }

    public override void Update()
    {
        if (_skillExecuted)
        {
            ReturnToGroundedState();
        }
    }

    public override void Exit()
    {
        base.Exit();
        Time.timeScale = 1f;
        EndSkillInvincible(_playerSkills.FlashStrike.Data.AfterInvincibleTime);
    }
}
