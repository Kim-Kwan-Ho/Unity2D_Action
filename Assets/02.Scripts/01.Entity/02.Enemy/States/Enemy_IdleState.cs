using UnityEngine;
public class Enemy_IdleState : Enemy_NormalState
{
    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.SetVelocity(0f, _rigid.velocity.y);
        float idleTime = Random.Range(_enemyStat.MinIdleTime, _enemyStat.MaxIdleTime);
        _stateTimer = idleTime;
    }
    public override void Update()
    {
        base.Update();
        if (_stateTimer <= 0f)
        {
            _stateMachine.ChangeState(_enemy.MoveState);
        }
    }
}
