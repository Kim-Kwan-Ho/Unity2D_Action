using UnityEngine;

public interface IDamageable
{
    public bool TakeDamage(AttackInfoSo attackInfo, Transform damageDealer);
}
