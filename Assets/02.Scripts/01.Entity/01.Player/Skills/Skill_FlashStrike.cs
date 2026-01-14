using UnityEngine;

public class Skill_FlashStrike : Skill_Base<FlashStrikeSkillSo>
{
    [Header("FlashStrike Setting")]
    [SerializeField] private TrailRenderer _trail;
    private RaycastHit2D[] _hits;
    private int _hitCount;

    protected override void Initialize()
    {
        base.Initialize();
        _hits = new RaycastHit2D[_data.MaxTargetCount];
    }

    public override void UseSkill()
    {
        base.UseSkill();
        _trail.enabled = true;
    }
    public override void ResetSkill()
    {
        base.ResetSkill();
        _trail.enabled = false;
    }
    public void StartFlashStrike(Vector3 startPos, int direction, out Vector3 destPos)
    {
        Vector3 directionVector = Vector3.right * direction;
        float actualDistance = _data.Range;

        RaycastHit2D wallHit = Physics2D.Raycast(startPos, directionVector, _data.Range, _data.WallLayer);
        if (wallHit.collider != null)
        {
            actualDistance = wallHit.distance - _data.WallOffSetDist;
        }
        _hitCount = Physics2D.RaycastNonAlloc(startPos, directionVector, _hits, actualDistance, _data.TargetLayer);
        for (int i = 0; i < _hitCount; i++)
        {
            _player.SFX.CreateFlashStrikeEffect(_hits[i].transform);
        }
        destPos = startPos + directionVector * actualDistance;
    }
    public void ExecuteFlashStrike()
    {

        int comboCount = 0;
        for (int i = 0; i < _hitCount; i++)
        {
            IDamageable target = _hits[i].collider.GetComponent<IDamageable>();
            if (target != null)
            {
                _player.Combat.PerformAttackOnTarget(target, _data.AttackInfo);
                comboCount++;
            }
        }
        _player.Combo.AddCombo(comboCount);
        _player.SFX.ShakeCamera(_data.SFXCameraVelocity);
        _trail.enabled = false;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_player.transform.position, _player.transform.position + Vector3.right * _data.Range);
    }
#endif
}
