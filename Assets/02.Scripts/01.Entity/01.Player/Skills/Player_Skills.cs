using UnityEngine;



// Todo: 모든 스킬을 직접 지정하는 방식이 아닌, 현재 장착중인 스킬로 변경할 것
public class Player_Skills : BaseBehaviour
{
    [SerializeField] private Skill_Dash _dash;
    public Skill_Dash Dash { get { return _dash; } }
    [SerializeField] private Skill_Plunge _plunge;
    public Skill_Plunge Plunge { get { return _plunge; } }
    [SerializeField] private Skill_FlashStrike _flashStrike;
    public Skill_FlashStrike FlashStrike { get { return _flashStrike; } }

    public void ResetAllSkillCooldowns()
    {
        _dash.ResetSkill();
        _plunge.ResetSkill();
        _flashStrike.ResetSkill();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _dash = GetComponentInChildren<Skill_Dash>();
        _plunge = GetComponentInChildren<Skill_Plunge>();
        _flashStrike = GetComponentInChildren<Skill_FlashStrike>();
    }
#endif
}
