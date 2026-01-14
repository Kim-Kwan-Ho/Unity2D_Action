using System.Collections.Generic;
using UnityEngine;


public class PoolManager : SingletonBehaviour<PoolManager>
{

    private Dictionary<int, PoolData> _pools = new Dictionary<int, PoolData>();
    private Dictionary<GameObject, int> _activeObjectToPoolKey = new Dictionary<GameObject, int>();
    private Transform _poolRoot;


    public override void Init()
    {
    }


    public void CreatePool(PoolObjectDataSo data)
    {
        int poolKey = data.Prefab.GetInstanceID();
        if (_pools.ContainsKey(poolKey))
        {
            return;
        }

        PoolData poolData = new PoolData();
        poolData.Data = data;
        poolData.PoolParent = new GameObject($"{data.Prefab.name} Pool").transform;
        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("PoolRoot").transform;
        }
        poolData.PoolParent.SetParent(_poolRoot);

        for (int i = 0; i < data.InitialSize; i++)
        {
            GameObject obj = CreateNewObject(data.Prefab, poolData.PoolParent);
            poolData.InactiveObjects.Enqueue(obj);
        }
        _pools.Add(poolKey, poolData);
    }

    public GameObject Spawn(PoolObjectDataSo data, Vector3 position)
    {
        return Spawn(data, position, Quaternion.identity);
    }

    public GameObject Spawn(PoolObjectDataSo data, Vector3 position, Quaternion rotation)
    {

        int poolKey = data.Prefab.GetInstanceID();
        if (!_pools.ContainsKey(poolKey))
        {
            CreatePool(data);
        }

        PoolData poolData = _pools[poolKey];
        GameObject obj;

        if (poolData.InactiveObjects.Count > 0)
        {
            obj = poolData.InactiveObjects.Dequeue();
        }
        else
        {
            if (poolData.Data.MaxSize > 0)
            {
                int totalCount = poolData.ActiveObjects.Count + poolData.InactiveObjects.Count;
                if (totalCount >= poolData.Data.MaxSize)
                {
                    return null;
                }
            }

            obj = CreateNewObject(poolData.Data.Prefab, poolData.PoolParent);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        poolData.ActiveObjects.Add(obj);
        _activeObjectToPoolKey[obj] = poolKey;

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnSpawnFromPool();
        }
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (!_activeObjectToPoolKey.TryGetValue(obj, out int poolKey))
        {
            return;
        }

        if (!_pools.TryGetValue(poolKey, out PoolData poolData))
        {
            return;
        }

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnReturnToPool();
        }

        obj.SetActive(false);
        obj.transform.SetParent(poolData.PoolParent);

        poolData.ActiveObjects.Remove(obj);
        poolData.InactiveObjects.Enqueue(obj);
        _activeObjectToPoolKey.Remove(obj);
    }


    public void ReturnAll(GameObject prefab)
    {

        int poolKey = prefab.GetInstanceID();

        if (!_pools.TryGetValue(poolKey, out PoolData poolData))
        {
            return;
        }

        List<GameObject> activeObjectsCopy = new List<GameObject>(poolData.ActiveObjects);
        foreach (GameObject obj in activeObjectsCopy)
        {
            Return(obj);
        }
    }

    public void ReturnAll()
    {
        foreach (var poolData in _pools.Values)
        {
            List<GameObject> activeObjectsCopy = new List<GameObject>(poolData.ActiveObjects);

            foreach (GameObject obj in activeObjectsCopy)
            {
                Return(obj);
            }
        }
    }

    public void ClearPool(GameObject prefab)
    {
        int poolKey = prefab.GetInstanceID();

        if (!_pools.TryGetValue(poolKey, out PoolData poolData))
        {
            return;
        }

        foreach (GameObject obj in poolData.ActiveObjects)
        {
            _activeObjectToPoolKey.Remove(obj);
            Destroy(obj);
        }

        foreach (GameObject obj in poolData.InactiveObjects)
        {
            Destroy(obj);
        }

        if (poolData.PoolParent != null)
        {
            Destroy(poolData.PoolParent.gameObject);
        }

        _pools.Remove(poolKey);
    }

    public void ClearAllPools()
    {
        List<int> poolKeys = new List<int>(_pools.Keys);

        foreach (int poolKey in poolKeys)
        {
            PoolData poolData = _pools[poolKey];
            ClearPool(poolData.Data.Prefab);
        }
    }

    private GameObject CreateNewObject(GameObject prefab, Transform parent)
    {
        GameObject obj = Instantiate(prefab, parent);
        obj.SetActive(false);
        return obj;
    }

}
