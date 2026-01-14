using System.Collections;
using UnityEngine;
public class Player_SFX : Entity_SFX
{
    [Header("Player Effects")]
    [SerializeField] private float _blinkInterval;
    [SerializeField] private CanvasGroup _tDEffectCanvasGroup;
    [SerializeField] private Vector2 _takeHitRandPos;

    [Header("Pool Settings")]
    [SerializeField] private PoolObjectDataSo _takeHitData;
    [SerializeField] private PoolObjectDataSo _jumpData;
    [SerializeField] private PoolObjectDataSo _plungeData;
    [SerializeField] private PoolObjectDataSo _flashStrikeData;

    protected override void InitializeEffectPools()
    {
        base.InitializeEffectPools();
        // Todo: Jump 이펙트 생성
        if (_jumpData != null)
        {
            PoolManager.Instance.CreatePool(_jumpData);
        }
        PoolManager.Instance.CreatePool(_takeHitData);
        PoolManager.Instance.CreatePool(_plungeData);
        PoolManager.Instance.CreatePool(_flashStrikeData);
    }

    public void CreateJumpEffect()
    {
        PoolManager.Instance.Spawn(_jumpData, transform.position, Quaternion.identity);
    }

    public void CreatePlungeEffect(Vector2 position)
    {
        PoolManager.Instance.Spawn(_plungeData, position, Quaternion.identity);
    }

    public void CreateFlashStrikeEffect(Transform target)
    {
        PoolManager.Instance.Spawn(_flashStrikeData, target.position, Quaternion.identity);
    }

    public override void CreateTakeHitEffect(EDamageType damageType, bool isCritical)
    {
        Vector3 randPos = MathUtils.GetRandomVector2(-_takeHitRandPos, _takeHitRandPos);
        PoolManager.Instance.Spawn(_takeHitData, transform.position + randPos);
        if (_damagedRoutine != null)
        {
            StopCoroutine(_damagedRoutine);
        }
        _damagedRoutine = StartCoroutine(CoDamagedEffect());
    }

    protected override IEnumerator CoDamagedEffect()
    {
        float curTime = _damagedTime;
        float blinkTimer = 0;
        int count = 0;
        _tDEffectCanvasGroup.alpha = 1;
        while (curTime > 0)
        {
            blinkTimer += Time.unscaledDeltaTime;
            if (blinkTimer >= _blinkInterval)
            {

                blinkTimer = 0;
                if (count % 2 == 0)
                    _spriteRenderer.material = _damagedMaterial;
                else
                    _spriteRenderer.material = _normalMaterial;

                count++;

            }
            _tDEffectCanvasGroup.alpha = curTime / _damagedTime;
            curTime -= Time.unscaledDeltaTime;
            yield return null;
        }
        _tDEffectCanvasGroup.alpha = 0;
        _damagedRoutine = null;
        _spriteRenderer.material = _normalMaterial;
    }

}