using UnityEngine;


[CreateAssetMenu(fileName = "FlashStrikeSkill", menuName = "ScriptableObjects/Skills/FlashStrike")]
public class FlashStrikeSkillSo : BaseSkillSo
{
    [Header("FlashStrike")]
    [SerializeField] private AttackInfoSo _attackInfo;
    public AttackInfoSo AttackInfo { get { return _attackInfo; } }
    [SerializeField] private float _freezeTime;
    public float FreezeTime { get { return _freezeTime; } }


    [Header("TargetCheck")]
    [SerializeField] private int _maxTargetCount;
    public int MaxTargetCount { get { return _maxTargetCount; } }
    [SerializeField] private float _range;
    public float Range { get { return _range; } }
    [SerializeField] private float _wallOffSetDist;
    public float WallOffSetDist { get { return _wallOffSetDist; } }
    [SerializeField] private LayerMask _wallLayer;
    public LayerMask WallLayer { get { return _wallLayer; } }
    [SerializeField] private LayerMask _targetLayer;
    public LayerMask TargetLayer { get { return _targetLayer; } }

    [Header("Effect")]
    [SerializeField] private Vector3 _sfxCameraVelocity;
    public Vector3 SFXCameraVelocity { get { return _sfxCameraVelocity; } }



}
