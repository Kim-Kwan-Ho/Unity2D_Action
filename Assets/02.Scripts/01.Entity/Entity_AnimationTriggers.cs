using UnityEngine;

public class Entity_AnimationTriggers : BaseBehaviour
{
    [SerializeField] private Entity _entity;
    [SerializeField] private Entity_Combat _entityCombat;
    private void CurrentStateTrigger()
    {
        _entity.CurrentStateAnimationTrigger();
    }
    private void AttackTrigger(int index)
    {
        _entityCombat.PerformAttack(index);
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _entity = GetComponentInParent<Entity>();
        _entityCombat = GetComponentInParent<Entity_Combat>();
    }
    #endif
}
