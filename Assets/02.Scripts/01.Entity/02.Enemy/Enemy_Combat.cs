using UnityEngine;
public class Enemy_Combat : Entity_Combat
{
    protected override void OnAttackHit(Collider2D target, AttackInfoSo attackInfo)
    {
        // Enemy는 현재 타격 이펙트가 없음
    }
}
