using System;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{

    [Header("Player")]
    private Player _player;
    private Vector3 _playerInitialPosition;
    private Quaternion _playerInitialRotation;

    [Header("Input")]
    private Player_Input _uiInput;
    private EGameState _gameState;

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameClear;

    private void Start()
    {
        Init();
        InitializePlayer();
        InitializeUIInput();
        SubscribeToEvents();
        LoadComplete();
    }
    public override void Init()
    {
        Cursor.visible = false;
        PoolManager.Instance.Init();
        UIManager.Instance.Init();
        WaveManager.Instance.Init();
    }
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
        if (_uiInput != null)
        {
            _uiInput.Dispose();
        }
    }
    private void InitializeUIInput()
    {
        _uiInput = new Player_Input();
        _uiInput.Game.Start.performed += ctx => OnStartInput();
    }

    private void SubscribeToEvents()
    {
        WaveManager.Instance.OnAllWavesComplete += HandleGameClear;
    }
    private void UnsubscribeFromEvents()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnAllWavesComplete -= HandleGameClear;
        }
    }
    private void LoadComplete()
    {
        _gameState = EGameState.Waiting;
        _uiInput.Enable();
        UIManager.Instance.CreateNotice(ENoticeType.Normal,
        "<b><size=58>게임 시작</size></b>\n" +
        "<size=24><color=#FFD700>R</color>(키보드) / <color=#FFD700>Menu</color>(XBox) 키를 눌러 시작\n\n" +
        "<b>조작법 (키보드)</b>\n" +
        "<color=#00FF00>Z</color> 대쉬 | <color=#FF6B6B>X</color> 공격 | <color=#4ECDC4>C</color> 점프 | <color=#FFD93D>A, S</color> 스킬\n\n" +
        "<b>조작법 (XBox 컨트롤러)</b>\n" +
        "<color=#00FF00>B</color> 대쉬 | <color=#FF6B6B>X</color> 공격 | <color=#4ECDC4>A</color> 점프 | <color=#FFD93D>LT, RT</color> 스킬</size>");
    }

    private void OnStartInput()
    {
        UIManager.Instance.HideNotice();
        if (_gameState == EGameState.Waiting)
        {
            _uiInput.Disable();
            _player.Input.Enable();
            StartGame();
        }
        else if (_gameState == EGameState.GameOver || _gameState == EGameState.Clear)
        {
            RestartGame();
        }
    }
    private void InitializePlayer()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("씬에 플레이어가 없습니다!");
            return;
        }

        _playerInitialPosition = _player.transform.position;
        _playerInitialRotation = _player.transform.rotation;
    }

    private void StartGame()
    {
        _gameState = EGameState.Playing;
        OnGameStart?.Invoke();
    }

    public void OnPlayerDeath()
    {
        HandleGameOver();
    }

    private void HandleGameOver()
    {
        _gameState = EGameState.GameOver;
        if (_player != null && _player.Input != null)
        {
            _player.Input.Disable();
        }
        _uiInput.Enable();
        UIManager.Instance.CreateNotice(ENoticeType.Normal,
            "<b><size=58><color=#FF4444>게임 오버!</color></size></b>\n\n" +
            "<size=24><color=#FFD700>R</color>(키보드) / <color=#FFD700>Menu</color>(XBox) 키를 눌러 재시작</size>");
        OnGameOver?.Invoke();
    }

    private void HandleGameClear()
    {
        _gameState = EGameState.Clear;
        _uiInput.Enable();
        UIManager.Instance.CreateNotice(ENoticeType.Normal,
            "<b><size=58><color=#00FF00>모든 웨이브 클리어!</color></size></b>\n\n" +
            "<size=24><color=#FFD700>R</color>(키보드) / <color=#FFD700>Menu</color>(XBox) 키를 눌러 재시작</size>");
        OnGameClear?.Invoke();
    }

    public void RestartGame()
    {
        _uiInput.Disable();
        WaveManager.Instance.ResetWaveManager();
        PoolManager.Instance.ReturnAll();
        ResetPlayer();
        StartGame();
    }

    private void ResetPlayer()
    {
        _player.ResetPlayer(_playerInitialPosition, _playerInitialRotation);
    }
}
