# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 프로젝트 개요
Unity 2D 액션 게임 프로젝트 (Hack & Slash 장르)
- Unity 2021.3+ 버전 사용
- 2D Feature 패키지 활용
- State Machine 패턴 기반 캐릭터 제어

## 주요 개발 명령어

### Unity 에디터
- Unity Hub에서 프로젝트 열기 권장
- Scene: `Assets/01.Scenes/SampleScene.unity`

### 빌드
Unity 에디터에서:
- `File > Build Settings` → 플랫폼 선택 후 빌드

## 프로젝트 구조

### 폴더 구조
```
Assets/
├── 01.Scenes/          # Unity 씬 파일
├── 02.Scripts/         # 모든 C# 스크립트
│   ├── 00.Base/        # 기본 베이스 클래스
│   ├── 01.Entity/      # 엔티티 관련 (플레이어, 적)
│   └── 99.Utils/       # 유틸리티 (Enums 등)
├── 03.Prefabs/         # 프리팹
├── 04.Animations/      # 애니메이션 파일
├── 05.DownloadAssets/  # 외부 에셋
└── 06.Others/          # 기타 파일
```

### 핵심 아키텍처

#### 1. 베이스 클래스 계층 구조
```
BaseBehaviour (MonoBehaviour)
    ↓
Entity (추상 클래스)
    ↓
├── Player
└── Enemy
```

**BaseBehaviour** (`Assets/02.Scripts/00.Base/BaseBehaviour.cs`)
- 모든 커스텀 MonoBehaviour의 기반 클래스
- Unity 에디터 전용 유틸리티 메서드 제공:
  - `OnBindField()`: 컴포넌트 자동 바인딩
  - `OnButtonField()`: 에디터 테스트용 버튼
  - `FindObjectInAsset<T>()`: 에셋 검색
  - `CheckNullValue()`: Null 체크 유틸리티
- 에디터에서 "Bind Objects", "Active Button" 버튼으로 접근 가능

**Entity** (`Assets/02.Scripts/01.Entity/Entity.cs`)
- Player와 Enemy의 공통 기능 제공
- 주요 기능:
  - StateMachine 관리
  - Rigidbody2D 기반 물리 처리
  - Ground/Wall 충돌 감지 (Raycast)
  - Flip 처리 (좌우 방향 전환)
  - `SetVelocity(float velX, float velY)`: 속도 설정

#### 2. State Machine 패턴

**StateMachine** (`Assets/02.Scripts/01.Entity/StateMachine.cs`)
- 상태 관리 및 전환 처리
- `Initialize(EntityState)`: 초기 상태 설정
- `ChangeState(EntityState)`: 상태 전환
- `UpdateActiveState()`: 매 프레임 현재 상태 업데이트
- `SwitchOffStateMachine()`: 상태 전환 비활성화

**EntityState** (`Assets/02.Scripts/01.Entity/EntityState.cs`)
- 모든 상태의 기반 클래스
- 생명주기: `Enter()` → `Update()` → `Exit()`
- `AnimationTrigger()`: 애니메이션 이벤트에서 호출
- `_stateTimer`: 상태별 타이머

**State 계층 구조:**
```
EntityState
    ↓
├── PlayerState (공통 플레이어 로직, 대시 입력 처리)
│   ├── Player_GroundState (지상 상태 기반)
│   │   ├── Player_IdleState
│   │   └── Player_MoveState
│   ├── Player_AiredState (공중 상태 기반)
│   │   ├── Player_JumpState
│   │   └── Player_FallState
│   ├── Player_DashState
│   └── Player_BasicAttackState
│
└── EnemyState
    ├── Enemy_NormalState (평시 행동 기반)
    │   ├── Enemy_IdleState
    │   └── Enemy_MoveState
    ├── Enemy_BattleState
    ├── Enemy_AttackState
    ├── Enemy_StunnedState
    └── Enemy_DeadState
```

#### 3. 플레이어 시스템

**Player** (`Assets/02.Scripts/01.Entity/01.Player/Player.cs`)
- 플레이어 컨트롤 및 상태 관리
- Stats (향후 ScriptableObject로 이동 예정):
  - `_moveSpeed`: 이동 속도
  - `_jumpForce`: 점프 힘
  - `_dashSpeed`, `_dashDuration`: 대시 속성
  - `_attackVelocities[]`: 공격별 이동 속도
  - `_comoboResetTime`: 콤보 리셋 시간
- `EnterAttackStateWithDelay()`: 공격 상태 진입 (딜레이 처리)

**Player_Skills** (`Assets/02.Scripts/01.Entity/01.Player/Skills/Player_Skills.cs`)
- 플레이어 스킬 관리 컨테이너
- 스킬별 컴포넌트 참조 보유

**Skill_Base** (`Assets/02.Scripts/01.Entity/01.Player/Skills/Skill_Base.cs`)
- 모든 스킬의 기반 클래스
- 쿨다운 시스템 내장:
  - `CanUseSkill()`: 사용 가능 여부
  - `SetSkillColldown()`: 쿨다운 시작
  - `ResetSkillDown()`: 쿨다운 초기화
- 스킬 타입: `ESkillType` enum으로 구분

#### 4. 적 시스템

**Enemy** (`Assets/02.Scripts/01.Entity/02.Enemy/Enemy.cs`)
- AI 기반 적 캐릭터
- Stats (향후 ScriptableObject로 이동 예정):
  - `_idleTime`, `_moveTime`: 행동 지속 시간
  - `_moveSpeed`: 이동 속도
  - `_attackDistance`: 공격 사거리
  - `_attackCoolDown`: 공격 쿨다운
  - `_stunnedDuration`: 스턴 지속시간
  - `_stunnedVelocity`: 스턴 시 밀림 속도
- 플레이어 감지:
  - `PlayerDetected()`: Raycast 기반 감지
  - `GetPlayerReference()`: 플레이어 Transform 참조

#### 5. 애니메이션 시스템

**Entity_AnimationTriggers** / **Player_AnimationTriggers** / **Enemy_AnimationTriggers**
- Animation Event에서 호출되는 메서드 정의
- 상태 머신의 `AnimationTrigger()` 호출하여 상태 간 동기화

**Combat System**
- `Entity_Combat`: 공격 처리 기반 클래스 (현재 구현 중)
- `Player_Combat`: 플레이어 전투 로직

## 개발 가이드

### 새로운 상태 추가
1. `EntityState` 또는 `PlayerState`/`EnemyState` 상속
2. `Enter()`, `Update()`, `Exit()` 오버라이드
3. Entity 클래스에 상태 필드 추가
4. `InitializeStates()`에서 초기화
5. 애니메이션 파라미터 이름 일치시키기

### 새로운 스킬 추가
1. `Skill_Base` 상속 클래스 생성
2. `ESkillType`에 스킬 타입 추가
3. `Player_Skills`에 스킬 참조 추가
4. `TryUseSkill()` 구현

### 에디터 바인딩 사용
- Inspector에서 "Bind Objects" 버튼 클릭
- `OnBindField()` 오버라이드하여 자동 바인딩 로직 추가
- 주로 `GetComponent`, `GetComponentInChildren` 활용

### 물리 및 충돌 감지
- Ground Check: `_gorundCheckTrs` 위치에서 Raycast
- Wall Check: `_wallCheckTrs` 위치에서 Raycast
- LayerMask 활용하여 감지 대상 제한

## 알려진 TODO
- Player/Enemy Stats를 ScriptableObject로 리팩토링
- Entity_Combat 시스템 구현 완료 필요
- 콤보 시스템 확장
