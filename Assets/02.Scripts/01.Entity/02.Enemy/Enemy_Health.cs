
public class Enemy_Health : Entity_Health
{
    protected override void ReduceHealth(float damage, EDamageType damageType, bool isCritical)
    {
        base.ReduceHealth(damage, damageType, isCritical);
        ShowDamageEffects(damage, damageType, isCritical, false);
    }
}
