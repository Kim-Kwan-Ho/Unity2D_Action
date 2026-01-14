using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Mathematics;



public class UI_DamageText : BaseBehaviour, IPoolable
{
    [SerializeField] private DamageTextUISo _data;
    [SerializeField] private TextMeshPro _text;
    private Coroutine _effectRoutine;
    private float _originalFontSize;
    

    public void InitializeText(float damage, EDamageType damageType, bool isCritical, bool isPlayerHit)
    {
        _text.text = damage.ToString();
        if (isCritical)
        {
            _text.fontSize *= _data.CriticalSizeRatio;
            _text.fontStyle = FontStyles.Bold;
        }
        if (isPlayerHit)
        {
            if (isCritical)
            {
                _text.color = _data.PHitCriticalNormalColor;
            }
            else
            {
                _text.color = _data.PHitNormalColor;
            }
        }
        else
        {
            if (damageType == EDamageType.Normal)
            {
                if (isCritical)
                {
                    _text.color = _data.EHitCriticalNormalColor;
                }
                else
                {
                    _text.color = _data.EHitNormalColor;
                }
            }
            else if (damageType == EDamageType.Skill)
            {
                if (isCritical)
                {
                    _text.color = _data.EHitCriticalSkillColor;
                }
                else
                {
                    _text.color = _data.EHitSkillColor;
                }
            }
        }
        if (_effectRoutine != null)
        {
            StopCoroutine(_effectRoutine);
        }
        _effectRoutine = StartCoroutine(CoTextEffect());
    }

    private IEnumerator CoTextEffect()
    {
        var color = _text.color;
        float curTime = 0;
        while (curTime < _data.LifeTime)
        {
            transform.position += Vector3.up * _data.MoveSpeed * Time.unscaledDeltaTime;
            color.a = math.lerp(1, 0, curTime / _data.LifeTime);
            _text.color = color;
            curTime += Time.unscaledDeltaTime;
            yield return null;
        }
        PoolManager.Instance.Return(this.gameObject);
    }

    public void OnSpawnFromPool()
    {
        if (_originalFontSize == 0)
        {
            _originalFontSize = _text.fontSize;
        }

        _text.fontSize = _originalFontSize;
        _text.fontStyle = FontStyles.Normal;

        var color = _text.color;
        color.a = 1f;
        _text.color = color;
    }

    public void OnReturnToPool()
    {
        if (_effectRoutine != null)
        {
            StopCoroutine(_effectRoutine);
            _effectRoutine = null;
        }
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _text = GetComponent<TextMeshPro>();
    }
#endif
}
