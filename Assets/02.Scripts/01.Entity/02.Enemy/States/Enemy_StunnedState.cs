public class Enemy_StunnedState : EnemyState
{
    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.StunnedDuration;
    }
    public override void Update()
    {
        base.Update();

        if (_stateTimer <= 0f)
        {
            _stateMachine.ChangeState(_enemy.BattleState);
        }
    }
}
