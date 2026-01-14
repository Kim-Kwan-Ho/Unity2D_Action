using UnityEngine;

public class Enemy_SFX : Entity_SFX
{
    [Header("Enemy TakeHit")]
    [SerializeField] private PoolObjectDataSo _criticalHitData;

    [Header("Death")]
    [SerializeField] private PoolObjectDataSo _deathData;


    protected override void InitializeEffectPools()
    {
        base.InitializeEffectPools();
        PoolManager.Instance.CreatePool(_criticalHitData);
        PoolManager.Instance.CreatePool(_deathData);
    }
    public override void CreateTakeHitEffect(EDamageType damageType, bool isCritical)
    {
        base.CreateTakeHitEffect(damageType, isCritical);
        if (isCritical)
        {
            PoolManager.Instance.Spawn(_criticalHitData, transform.position, Quaternion.identity);
        }
    }

    public override void CreateDeathEffect()
    {
        PoolManager.Instance.Spawn(_deathData, transform.position, Quaternion.identity);
    }


}
