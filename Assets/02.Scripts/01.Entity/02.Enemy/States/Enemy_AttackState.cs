public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Update()
    {
        base.Update();

        if (_isTriggerCalled)
        {
            _stateMachine.ChangeState(_enemy.BattleState);
        }
    }
}
