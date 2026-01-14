using UnityEngine;

[CreateAssetMenu(fileName = "PlungeSkill", menuName = "ScriptableObjects/Skills/Plunge")]
public class PlungeSkillSo : BaseSkillSo
{
    [Header("Plunge")]

    [SerializeField] private AttackInfoSo _attackInfo;
    public AttackInfoSo AttackInfo { get { return _attackInfo; } }
    [SerializeField] private float _speed;
    public float PlungeSpeed { get { return _speed; } }

    [Header("TargetCheck")]
    [SerializeField] private float _range;
    public float Range {get { return _range; }}

    [SerializeField] private LayerMask _targetLayer;
    public LayerMask TargetLayer {get { return _targetLayer; }}

    [Header("Effect")]
    [SerializeField] private Vector3 _sfxVelocity;
    public Vector3 SFXVelocity { get { return _sfxVelocity; } }
}
