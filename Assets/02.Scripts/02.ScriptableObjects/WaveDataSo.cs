using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "ScriptableObjects/Wave/WaveData")]
public class WaveDataSo : ScriptableObject
{

    [Header("Spawn Setting")]
    [SerializeField] private float _spawnDelay = 0.5f;
    public float SpawnDelay { get { return _spawnDelay; } }
    [SerializeField] private float _nextWaveDelay = 3f;
    public float NextWaveDelay { get { return _nextWaveDelay; } }


    [Header("Enemy Configuration")]
    [SerializeField] private EnemySpawnInfo[] _enemySpawnInfos;
    public EnemySpawnInfo[] EnemySpawnInfos { get { return _enemySpawnInfos; } }

}

