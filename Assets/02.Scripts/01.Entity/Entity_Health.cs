using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : BaseBehaviour, IDamageable
{
    public event Action OnHealthUpdate;
    [SerializeField] private Entity _entity;
    [SerializeField] protected Entity_SFX _sfx;
    protected bool _isDead;
    public bool IsDead { get { return _isDead; } }
    protected bool _canTakeDamage;
    public bool CanTakeDamage { get { return _canTakeDamage; } }


    [Header("Header")]
    private float _maxHealth;
    public float MaxHealth { get { return _maxHealth; } }
    protected float _currentHealth;
    public float CurrentHealth { get { return _currentHealth; } }

    [Header("UI")]
    [SerializeField] protected Transform _damageSpawnTrs;
    [SerializeField] private Slider _healthBar;


    protected override void Initialize()
    {
        base.Initialize();
        _isDead = false;
        _canTakeDamage = true;
        OnHealthUpdate += UpdateHelathBar;
    }

    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        OnHealthUpdate?.Invoke();
    }

    public void SetCanTakeDamage(bool canTakeDamage)
    {
        _canTakeDamage = canTakeDamage;
    }

    public void ResetHealth()
    {
        _isDead = false;
        _canTakeDamage = true;
        _currentHealth = _maxHealth;

        if (_healthBar != null)
        {
            _healthBar.gameObject.SetActive(true);
        }

        OnHealthUpdate?.Invoke();
    }
    public virtual bool TakeDamage(AttackInfoSo attackInfo, Transform damageDealer)
    {
        if (_isDead || !_canTakeDamage)
            return false;

        float finalDamage = CalculateDamage(attackInfo, out bool isCritical);
        ReduceHealth(finalDamage, attackInfo.DamageType, isCritical);
        ApplyKnockbackByStance(attackInfo, damageDealer);

        return true;
    }

    private void ApplyKnockbackByStance(AttackInfoSo attackInfo, Transform damageDealer)
    {
        switch (_entity.StanceType)
        {
            case EStanceType.Stance:
                ApplyStanceKnockback(attackInfo, damageDealer);
                break;
            case EStanceType.Knockback:
                ApplyKnockbackStance(attackInfo, damageDealer);
                break;
            case EStanceType.StunKnockback:
                ApplyStunKnockbackStance(attackInfo, damageDealer);
                break;
        }
    }

    private void ApplyStanceKnockback(AttackInfoSo attackInfo, Transform damageDealer)
    {
        if (attackInfo.AttackType == EAttackType.ForceKnockbackStun)
        {
            TakeStunKnockback(attackInfo, damageDealer);
        }
    }

    private void ApplyKnockbackStance(AttackInfoSo attackInfo, Transform damageDealer)
    {
        switch (attackInfo.AttackType)
        {
            case EAttackType.KnockbackStun:
            case EAttackType.Knockback:
                TakeKnockback(attackInfo, damageDealer);
                break;
            case EAttackType.ForceKnockbackStun:
                TakeStunKnockback(attackInfo, damageDealer);
                break;
        }
    }

    private void ApplyStunKnockbackStance(AttackInfoSo attackInfo, Transform damageDealer)
    {
        switch (attackInfo.AttackType)
        {
            case EAttackType.KnockbackStun:
            case EAttackType.ForceKnockbackStun:
                TakeStunKnockback(attackInfo, damageDealer);
                break;
            case EAttackType.Knockback:
                TakeKnockback(attackInfo, damageDealer);
                break;
        }
    }

    protected virtual float CalculateDamage(AttackInfoSo attackInfo, out bool isCritical)
    {
        float baseDamage = UnityEngine.Random.Range(attackInfo.MinDamage, attackInfo.MaxDamage);
        isCritical = UnityEngine.Random.Range(0f, 100f) < attackInfo.CriticalChance;
        if (isCritical)
        {
            baseDamage *= attackInfo.CriticalRatio;
        }

        return baseDamage;
    }
    protected virtual void ReduceHealth(float damage, EDamageType damageType, bool isCritical)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
        OnHealthUpdate?.Invoke();
    }

    protected void ShowDamageEffects(float damage, EDamageType damageType, bool isCritical, bool isPlayerHit)
    {
        UIManager.Instance.CreateDamageText(_damageSpawnTrs.position, (int)damage, damageType, isCritical, isPlayerHit);
        _sfx.CreateTakeHitEffect(damageType, isCritical);
    }

    // Todo: 죽음 이펙트 추가
    protected virtual void Die()
    {
        _sfx.CreateDeathEffect();
        _isDead = true;
        _entity.EntityDeath();
    }
    private void UpdateHelathBar()
    {
        if (_healthBar == null)
            return;
        if (_isDead)
        {
            _healthBar.gameObject.SetActive(false);
            return;
        }
        _healthBar.value = GetHealthRatio();

    }

    public float GetHealthRatio()
    {
        return _currentHealth / _maxHealth;
    }

    private void TakeKnockback(AttackInfoSo attackInfo, Transform damageDealer)
    {
        if (_isDead || attackInfo.KnockbackDuration <= 0)
            return;
        Vector2 knockBack = CalculateKnockback(attackInfo, damageDealer);
        _entity.ReceiveKnockback(knockBack, attackInfo.KnockbackDuration);
    }

    private void TakeStunKnockback(AttackInfoSo attackInfo, Transform damageDealer)
    {
        TakeKnockback(attackInfo, damageDealer);
        if (!_isDead)
            _entity.ReceiveStunKnockback(attackInfo.KnockbackForce, attackInfo.StunDuration);
    }

    private Vector2 CalculateKnockback(AttackInfoSo attackInfo, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        return new Vector2(direction * attackInfo.KnockbackForce.x, attackInfo.KnockbackForce.y);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _entity = GetComponent<Entity>();
        _sfx = GetComponent<Entity_SFX>();
    }
#endif

}
