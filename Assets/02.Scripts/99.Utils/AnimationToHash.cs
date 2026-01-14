using UnityEngine;

public static class AnimationToHash
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Move = Animator.StringToHash("Move");
    public static readonly int Jump = Animator.StringToHash("Jump");
    public static readonly int Fall = Animator.StringToHash("Fall");
    public static readonly int AiredAttack = Animator.StringToHash("AiredAttack");
    public static readonly int Dash = Animator.StringToHash("Dash");
    public static readonly int BasicAttack = Animator.StringToHash("BasicAttack");
    public static readonly int ComboIndex = Animator.StringToHash("ComboIndex");
    public static readonly int XVelocity = Animator.StringToHash("XVelocity");
    public static readonly int Plunge = Animator.StringToHash("Plunge");
    public static readonly int FlashStrike = Animator.StringToHash("FlashStrike");
    public static readonly int Spawn = Animator.StringToHash("Spawn");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Dead = Animator.StringToHash("Dead");
    public static readonly int Stunned = Animator.StringToHash("Stunned");
    public static readonly int Battle = Animator.StringToHash("Battle");
}
