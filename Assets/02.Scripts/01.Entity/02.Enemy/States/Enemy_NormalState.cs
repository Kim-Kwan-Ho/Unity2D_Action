public class Enemy_NormalState : EnemyState
{
    public Enemy_NormalState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Update()
    {
        base.Update();
        if (_enemy.PlayerDetected())
        {
            _stateMachine.ChangeState(_enemy.BattleState);
        }
    }
}
