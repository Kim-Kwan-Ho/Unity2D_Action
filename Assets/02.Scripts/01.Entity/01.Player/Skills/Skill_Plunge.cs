using UnityEngine;

public class Skill_Plunge : Skill_Base<PlungeSkillSo>
{
    [Header("Plunge Setting")]
    [SerializeField] private Transform _plungePointTrs;

    public override bool CanUseSkill()
    {
        if (!base.CanUseSkill())
            return false;

        return !_player.IsGrounded;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        Collider2D[] cols = Physics2D.OverlapCircleAll(_plungePointTrs.position, _data.Range, _data.TargetLayer);
        int comboCount = 0;
        foreach (var col in cols)
        {
            IDamageable target = col.GetComponent<IDamageable>();
            if (target != null)
            {
                _player.Combat.PerformAttackOnTarget(target, _data.AttackInfo);
                comboCount++;
            }
        }
        _player.Combo.AddCombo(comboCount);
        _player.SFX.ShakeCamera(_data.SFXVelocity);
        _player.SFX.CreatePlungeEffect(_plungePointTrs.transform.position);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_plungePointTrs.position, _data.Range);
    }

    
#endif
}
