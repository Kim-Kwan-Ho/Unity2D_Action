using UnityEngine;

public class Archer_Arrow : BaseBehaviour
{

    [Header("Compoents")]
    [SerializeField] private Rigidbody2D _rigid;
    private Entity_Combat _entityCombat;

    [Header("Arrow Settings")]
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _arrowSpeed;
    private AttackInfoSo _attackInfo;

    public void SetupArrow(int direction, AttackInfoSo attackInfo, Entity_Combat entityCombat)
    {
        _entityCombat = entityCombat;
        _attackInfo = attackInfo;
        _rigid.velocity = new Vector2(_arrowSpeed * direction, 0);
        if (direction == -1)
        {
            transform.Rotate(0, 180, 0);            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.GetComponent<IDamageable>();
        if (target != null)
        {
            _entityCombat.PerformAttackOnTarget(target, _attackInfo);
        }
        Destroy(this.gameObject);
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _rigid = GetComponent<Rigidbody2D>();
    }
#endif
}
