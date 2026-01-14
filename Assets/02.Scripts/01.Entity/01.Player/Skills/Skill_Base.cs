using System;
using System.Collections;
using UnityEngine;

// Todo: 스킬을 모듈처럼 활용할 수 있도록 개선
public abstract class SkillBase : BaseBehaviour
{
    public event Action OnCooldownStart;
    public event Action OnCooldownEnd;

    [SerializeField] protected Player _player;
    protected float _coolTime;
    protected float _remainingCooldown;
    private Coroutine _cooldownRoutine;

    protected void InitializeCoolTime(float coolTime)
    {
        _coolTime = coolTime;
        _remainingCooldown = 0f;
    }

    public virtual void UseSkill()
    {
        SetSkillCoolTime();
    }

    public virtual bool CanUseSkill()
    {
        if (IsCoolTime())
            return false;
        return true;
    }

    protected bool IsCoolTime()
    {
        return _remainingCooldown > 0;
    }

    private void SetSkillCoolTime()
    {
        if (_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);

        _remainingCooldown = _coolTime;
        OnCooldownStart?.Invoke();
        _cooldownRoutine = StartCoroutine(CoCooldown());
    }

    private IEnumerator CoCooldown()
    {
        while (_remainingCooldown > 0)
        {
            _remainingCooldown -= Time.deltaTime;
            yield return null;
        }

        _remainingCooldown = 0;
        OnCooldownEnd?.Invoke();
        _cooldownRoutine = null;
    }

    public virtual void ResetSkill()
    {
        if (_cooldownRoutine != null)
        {
            StopCoroutine(_cooldownRoutine);
            _cooldownRoutine = null;
        }
        _remainingCooldown = 0;
        OnCooldownEnd?.Invoke();
    }

    public float GetCooldownRatio()
    {
        if (!IsCoolTime())
            return 0f;
        return _remainingCooldown / _coolTime;
    }

    public float GetRemainingCooldown()
    {
        return _remainingCooldown;
    }

    public bool IsCooldownReady()
    {
        return !IsCoolTime();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _player = GetComponentInParent<Player>();
    }
#endif
}

public class Skill_Base<T> : SkillBase where T : BaseSkillSo
{
    [SerializeField] protected T _data;
    public T Data { get { return _data; } }

    protected override void Initialize()
    {
        base.Initialize();
        InitializeCoolTime(_data.CoolTime);
    }
}
