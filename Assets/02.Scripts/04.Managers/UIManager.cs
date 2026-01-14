using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    [Header("Notice")]
    [SerializeField] private UI_Notice _notice;

    [Header("Damage")]
    [SerializeField] private PoolObjectDataSo _damageTextData;
    [SerializeField] private Vector2 _damageSpawnRandomOffset;

    public override void Init()
    {
        PoolManager.Instance.CreatePool(_damageTextData);
    }
    public void CreateDamageText(Vector2 position, int damage, EDamageType damageType, bool isCritical, bool isPlayerHit)
    {
        Vector2 randomPos = MathUtils.GetRandomVector2(-_damageSpawnRandomOffset, _damageSpawnRandomOffset);
        UI_DamageText damageText = PoolManager.Instance.Spawn(_damageTextData, position + randomPos, Quaternion.identity).GetComponent<UI_DamageText>();
        damageText.InitializeText(damage, damageType, isCritical, isPlayerHit);
    }

    public void CreateNotice(ENoticeType noticeType, string message, float displayTime = 3)
    {
        HideNotice();
        _notice.SetNotice(noticeType, message, displayTime);
    }
    public void HideNotice()
    {
        _notice.HideNotice();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _notice = GetComponentInChildren<UI_Notice>();
    }
    #endif

}
