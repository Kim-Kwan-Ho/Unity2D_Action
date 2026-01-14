using UnityEngine;

public class Enemy_MoveState : Enemy_NormalState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        float moveTime = Random.Range(_enemyStat.MinMoveTime, _enemyStat.MaxMoveTime);
        _stateTimer = moveTime;
        if (_enemy.IsWallDetected)
        {
            _enemy.Flip();
        }
    }
    public override void Update()
    {
        base.Update();

        _enemy.SetVelocity(_enemyStat.MoveSpeed * _enemy.FacingDirection, _rigid.velocity.y);
        if (_enemy.IsWallDetected)
        {
            _stateMachine.ChangeState(_enemy.IdleState);
        }

        if (_stateTimer <= 0f)
        {
            _stateMachine.ChangeState(_enemy.IdleState);
        }
    }
}
