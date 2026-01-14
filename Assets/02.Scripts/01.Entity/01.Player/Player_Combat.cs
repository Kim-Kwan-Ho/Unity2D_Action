using UnityEngine;


// Todo: 무기 별로 변경할 수 있도록 개선
public class Player_Combat : Entity_Combat
{
    [SerializeField] private Player _player;
    [SerializeField] private int _maxHitEffect = 3;
    private int _currentHitCount;
    private int _hitCountForCombo;

    public override void PerformAttack(int attackIndex)
    {
        _currentHitCount = 0;
        _hitCountForCombo = 0;

        base.PerformAttack(attackIndex);
        if (_hitCountForCombo > 0)
        {
            _player.Combo.AddCombo(_hitCountForCombo);
        }
    }

    protected override void OnAttackHit(Collider2D target, AttackInfoSo attackInfo)
    {
        _hitCountForCombo++;
        if (_currentHitCount >= _maxHitEffect)
            return;

        _player.SFX.CreateAttackEffect(target.transform.position);
        _currentHitCount++;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _player = GetComponent<Player>();
    }
#endif
}
