public class EnemyState : EntityState
{
    protected Enemy _enemy;
    protected EnemyStatSo _enemyStat;
    public EnemyState(Enemy enemy, StateMachine stateMachine, int animationHash) : base(stateMachine, animationHash)
    {
        _enemy = enemy;
        _enemyStat = _enemy.Stat;
        _animator = _enemy.Animator;
        _rigid = _enemy.Rigid;
    }


    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        _animator.SetFloat(AnimationToHash.XVelocity, _rigid.velocity.x);
    }
}
