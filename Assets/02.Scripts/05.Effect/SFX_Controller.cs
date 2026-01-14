using UnityEngine;

public class SFX_Controller : BaseBehaviour, IPoolable
{
    [SerializeField] private ESFXType _type;
    [SerializeField] private float _lifeTime;
    private float _currentLifeTime;
    protected override void Initialize()
    {
        base.Initialize();
        _currentLifeTime = _lifeTime;
    }
    private void Update()
    {
        _currentLifeTime -= Time.deltaTime;
        if (_currentLifeTime <= 0)
        {
            if (_type == ESFXType.Disposable)
            {
                Destroy(this.gameObject);
            }
            else if (_type == ESFXType.Reuseable)
            {
                PoolManager.Instance.Return(this.gameObject);
            }
        }
    }

    public void OnSpawnFromPool()
    {
        _currentLifeTime = _lifeTime;
    }

    public void OnReturnToPool()
    {

    }
}
