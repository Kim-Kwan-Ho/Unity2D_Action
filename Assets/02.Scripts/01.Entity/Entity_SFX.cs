using UnityEngine;
using Cinemachine;
using System.Collections;
public class Entity_SFX : BaseBehaviour
{

    [Header("TakeHit")]
    [SerializeField] protected float _damagedTime;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Material _normalMaterial;
    [SerializeField] protected Material _damagedMaterial;
    protected Coroutine _damagedRoutine;


    [Header("Attack")]
    [SerializeField] private Vector2 _attackRandPos;
    [SerializeField] private PoolObjectDataSo _attackEffectData;
    [SerializeField] private CinemachineImpulseSource _cineImpulse;

    protected override void Initialize()
    {
        base.Initialize();
        InitializeEffectPools();
    }
    protected virtual void InitializeEffectPools()
    {
        if (_attackEffectData != null)
            PoolManager.Instance.CreatePool(_attackEffectData);
    }

    public void ShakeCamera(Vector3 velocity)
    {
        _cineImpulse.GenerateImpulse(velocity);
    }

    public void CreateAttackEffect(Vector2 targetPos)
    {

        if (_attackEffectData == null)
            return;

        Vector2 spawnPos = targetPos + MathUtils.GetRandomVector2(-_attackRandPos, _attackRandPos);
        PoolManager.Instance.Spawn(_attackEffectData, spawnPos);
    }
    public virtual void CreateTakeHitEffect(EDamageType damageType, bool isCritical)
    {
        if (_damagedRoutine != null)
        {
            StopCoroutine(_damagedRoutine);
        }
        _damagedRoutine = StartCoroutine(CoDamagedEffect());
    }

    protected virtual IEnumerator CoDamagedEffect()
    {
        _spriteRenderer.material = _damagedMaterial;
        yield return new WaitForSeconds(_damagedTime);
        _spriteRenderer.material = _normalMaterial;
    }
    public virtual void CreateDeathEffect()
    {
        // 현재 플레이어는 사망 이펙트가 없기에 EnemySFX에서 처리
    }
    
    public virtual void ResetEffect()
    {
        if (_damagedRoutine != null)
        {
            StopCoroutine(_damagedRoutine);
        }
        _spriteRenderer.material = _normalMaterial;
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _cineImpulse = GetComponentInChildren<CinemachineImpulseSource>();
    }
#endif
}
