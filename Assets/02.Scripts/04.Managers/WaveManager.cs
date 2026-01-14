using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : SingletonBehaviour<WaveManager>
{
    [Header("Data")]
    [SerializeField] private WaveDataSo[] _waves;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 _spawnAreaMin;
    [SerializeField] private Vector2 _spawnAreaMax;

    [Header("Wave State")]
    private int _currentWaveIndex;
    private EWaveState _waveState;
    private int _aliveEnemyCount;
    private List<Enemy> _activeEnemyList;

    public event Action<int> OnWaveComplete;
    public event Action OnAllWavesComplete;

    protected override void Initialize()
    {
        base.Initialize();
        _currentWaveIndex = 0;
        _aliveEnemyCount = 0;
        _waveState = EWaveState.WaitingToStart;
        _activeEnemyList = new List<Enemy>();
    }
    public override void Init()
    {
        InitializeEnemyPools();
        GameManager.Instance.OnGameStart += StartWave;
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= StartWave;
        }
    }

    private void InitializeEnemyPools()
    {
        foreach (var waveData in _waves)
        {
            foreach (var spawnInfo in waveData.EnemySpawnInfos)
            {
                PoolManager.Instance.CreatePool(spawnInfo.EnemyPoolData);
            }
        }
    }

    private void StartWave()
    {
        if (_waveState == EWaveState.InProgress || _waveState == EWaveState.Spawning)
        {
            return;
        }

        WaveDataSo currentWave = _waves[_currentWaveIndex];
        _waveState = EWaveState.Spawning;
        StartCoroutine(CoSpawnWave(currentWave));
    }

    private IEnumerator CoSpawnWave(WaveDataSo waveData)
    {
        UIManager.Instance.CreateNotice(ENoticeType.Disappear, $"Wave {_currentWaveIndex + 1}", waveData.NextWaveDelay);
        yield return new WaitForSeconds(waveData.NextWaveDelay);

        _aliveEnemyCount = 0;
        _activeEnemyList.Clear();

        foreach (var spawnInfo in waveData.EnemySpawnInfos)
        {
            for (int i = 0; i < spawnInfo.SpawnCount; i++)
            {
                SpawnEnemy(spawnInfo);
                yield return new WaitForSeconds(waveData.SpawnDelay);
            }
        }
        _waveState = EWaveState.InProgress;
    }

    private void SpawnEnemy(EnemySpawnInfo spawnInfo)
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        Enemy enemy = PoolManager.Instance.Spawn(spawnInfo.EnemyPoolData, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        if (spawnInfo.EnemyStat != null)
        {
            enemy.SetStat(spawnInfo.EnemyStat);
        }

        _activeEnemyList.Add(enemy);
        _aliveEnemyCount++;
    }

    public void OnEnemyDeathCallback(Enemy enemy)
    {
        if (_activeEnemyList.Contains(enemy))
        {
            _activeEnemyList.Remove(enemy);
            _aliveEnemyCount--;

            if (_aliveEnemyCount <= 0 && _waveState == EWaveState.InProgress)
            {
                OnWaveCleared();
            }
        }
    }

    public void ResetWaveManager()
    {
        StopAllCoroutines();
        _activeEnemyList.Clear();
        _aliveEnemyCount = 0;
        _currentWaveIndex = 0;
        _waveState = EWaveState.WaitingToStart;

    }

    private void OnWaveCleared()
    {
        _currentWaveIndex++;
        _waveState = EWaveState.WaitingNextWave;
        OnWaveComplete?.Invoke(_currentWaveIndex);
        if (_currentWaveIndex >= _waves.Length)
        {
            CompleteAllWaves();
        }
        else
        {
            StartWave();
        }
    }

    private void CompleteAllWaves()
    {
        _waveState = EWaveState.Completed;
        OnAllWavesComplete?.Invoke();
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float randomX = UnityEngine.Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
        float randomY = UnityEngine.Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
        return new Vector2(randomX, randomY);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 center = (_spawnAreaMin + _spawnAreaMax) / 2f;
        Vector2 size = _spawnAreaMax - _spawnAreaMin;
        Gizmos.DrawWireCube(center, size);
    }
#endif
}
