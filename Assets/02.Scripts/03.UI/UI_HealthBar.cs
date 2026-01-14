using UnityEngine;

public class UI_HealthBar : BaseBehaviour
{
    [SerializeField] private Entity _entity;

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        _entity.OnFlipped += HandleFlip;
    }
    private void OnDisable()
    {
        _entity.OnFlipped -= HandleFlip;
    }

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _entity = GetComponentInParent<Entity>();
    }
    #endif
}
