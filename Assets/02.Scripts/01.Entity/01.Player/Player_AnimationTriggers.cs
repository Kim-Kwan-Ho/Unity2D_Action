using UnityEngine;

public class Player_AnimationTriggers : Entity_AnimationTriggers
{
    [SerializeField] private Player _player;


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _player = GetComponentInParent<Player>();
    }
    #endif
}
