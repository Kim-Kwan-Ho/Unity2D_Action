using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
    [SerializeField] private Enemy _enemy;


    private void SpecialAttackTrigger()
    {
        _enemy.PerformSpecialAttack();
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _enemy = GetComponentInParent<Enemy>();
    }
#endif
}
