using UnityEngine;


// Todo: 무기 별로 관리할 수 있도록 개선
public abstract class Entity_Combat : BaseBehaviour
{
    [SerializeField] private Transform _targetCheckTrs;
    [SerializeField] private float _targetCheckRadius = 1;
    [SerializeField] protected LayerMask _targetLayer;
    private AttackInfoSo[] _attackInfos;

    public void SetAttackInfos(AttackInfoSo[] attackInfos)
    {
        _attackInfos = attackInfos;
    }

    public void SetAttackInfo(AttackInfoSo attackInfo)
    {
        _attackInfos = new AttackInfoSo[] { attackInfo };
    }

    public virtual void PerformAttack(int attackIndex)
    {
        AttackInfoSo attackInfo = GetAttackInfo(attackIndex);

        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null)
                continue;

            if (damageable.TakeDamage(attackInfo, transform))
            {
                OnAttackHit(target, attackInfo);
            }
        }
    }


    protected abstract void OnAttackHit(Collider2D target, AttackInfoSo attackInfo);

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(_targetCheckTrs.position, _targetCheckRadius, _targetLayer);
    }
    public void PerformAttackOnTarget(IDamageable target, AttackInfoSo attackInfo)
    {
        if (target.TakeDamage(attackInfo, transform))
        {
            // Todo: 타격 이펙트 추가
        }
    }

    protected virtual AttackInfoSo GetAttackInfo(int index)
    {
        return _attackInfos[index - 1];
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_targetCheckTrs.position, _targetCheckRadius);
    }

#endif
}
