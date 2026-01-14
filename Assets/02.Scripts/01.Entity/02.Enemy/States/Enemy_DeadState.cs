using UnityEngine;

public class Enemy_DeadState : EnemyState
{

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _rigid.velocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();

        if (_isTriggerCalled)
        {
            WaveManager.Instance.OnEnemyDeathCallback(_enemy);
            PoolManager.Instance.Return(_enemy.gameObject);
        }
    }
}
