public enum EStanceType
{
    Knockback, // 뒤로 밀리기
    StunKnockback, // 뒤로 밀리면서 스턴 상태로 전환
    Stance, // ForceKnockbackStun에만 반응
}

public enum EAttackType
{
    Normal,
    Knockback,
    KnockbackStun,
    ForceKnockbackStun,
}

public enum EDamageType
{
    Normal,
    Skill
}

public enum ESFXType
{
    Disposable,
    Reuseable
}

public enum EWaveState
{
    WaitingToStart,
    Spawning,
    InProgress,
    WaitingNextWave,
    Completed
}

public enum EGameState
{
    Waiting,
    Playing,
    GameOver,
    Clear

}

public enum ENoticeType
{
    Normal,
    Disappear,

}
