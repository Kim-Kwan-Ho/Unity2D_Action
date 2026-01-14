using UnityEngine;


[CreateAssetMenu(fileName = "DashSkill", menuName = "ScriptableObjects/Skills/Dash")]
public class DashSkillSo : BaseSkillSo
{
    [Header("Dash")]
    [SerializeField] private float _duration;
    public float Duration { get { return _duration; } }
    [SerializeField] private float _speed;
    public float Speed { get { return _speed; } }
}
