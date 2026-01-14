using UnityEngine;

[CreateAssetMenu(fileName = "Pool_", menuName = "ScriptableObjects/Pool/ObjectData")]
public class PoolObjectDataSo : ScriptableObject
{
    [Header("Pool Object")]
    [SerializeField] private GameObject _prefab;
    public GameObject Prefab { get { return _prefab; } }

    [Header("Pool Settings")]
    [SerializeField] private int _initialSize = 10;
    public int InitialSize { get { return _initialSize; } }
    [Tooltip("-1은 제한 없음")]
    [SerializeField] private int _maxSize = -1; 
    public int MaxSize { get { return _maxSize; } }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_prefab != null && _prefab.GetComponent<IPoolable>() == null)
        {
            Debug.LogError($"{this.name} IPoolable 설정 필요");
        }
    }
    #endif
}
