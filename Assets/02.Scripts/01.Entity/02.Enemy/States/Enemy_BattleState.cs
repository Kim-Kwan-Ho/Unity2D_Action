using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform _player;
    private float _lastTimeAttacked = Mathf.NegativeInfinity;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(enemy, stateMachine, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player = _enemy.Player;
        if (_player == null)
        {
            _stateMachine.ChangeState(_enemy.IdleState);
        }
    }
    public override void Update()
    {
        base.Update();

        if (!IsInAttackRange())
        {
            _enemy.SetVelocity(_enemyStat.MoveSpeed * DirectionToPlayer(), _rigid.velocity.y);
        }
        else
        {
            _enemy.SetVelocity(0f, _rigid.velocity.y);

            if (ShouldFlipToPlayer())
                _enemy.Flip();

            if (CanAttack())
            {
                _lastTimeAttacked = Time.time;
                _stateMachine.ChangeState(_enemy.AttackState);
            }
        }
    }

    private bool IsInAttackRange()
    {
        float distanceX = _player.position.x - _enemy.transform.position.x;
        return Mathf.Abs(distanceX) <= _enemyStat.AttackDistance;
    }

    private bool CanAttack()
    {
        return Time.time > _lastTimeAttacked + _enemyStat.AttackCoolTime;
    }
    private int DirectionToPlayer()
    {
        return _player.position.x > _enemy.transform.position.x ? 1 : -1;
    }

    private bool ShouldFlipToPlayer()
    {
        float distanceX = _player.position.x - _enemy.transform.position.x;

        if (Mathf.Abs(distanceX) < Constants.FLIP_THRESHOLD)
            return false;

        int directionToPlayer = distanceX > 0 ? 1 : -1;
        return directionToPlayer != _enemy.FacingDirection;
    }

    public void ResetAttackCooldown()
    {
        _lastTimeAttacked = Time.time;
    }
}
