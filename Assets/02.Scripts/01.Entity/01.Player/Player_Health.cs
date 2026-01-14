using UnityEngine;


public class Player_Health : Entity_Health
{
    [SerializeField] private Player _player;

    protected override void ReduceHealth(float damage, EDamageType damageType, bool isCritical)
    {
        base.ReduceHealth(damage, damageType, isCritical);
        ShowDamageEffects(damage, damageType, isCritical, true);

        // 피격 시 콤보 초기화
        _player.Combo.ResetCombo();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _player = GetComponent<Player>();
    }
#endif
}
