public class Enemy_SpawnState : EnemyState
{
    public Enemy_SpawnState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.SetLayerToUntargetable();
        _enemy.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();
        if (_isTriggerCalled)
        {
            _stateMachine.ChangeState(_enemy.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.SetLayerToDefault();
    }
}
