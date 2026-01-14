using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Arrow")]
    [SerializeField] private Transform _arrowSpawnPointTrs;
    [SerializeField] private GameObject _arrowPrefab;

    public override void PerformSpecialAttack()
    {
        GameObject newArrow = Instantiate(_arrowPrefab, _arrowSpawnPointTrs.position, Quaternion.identity);
        newArrow.GetComponent<Archer_Arrow>().SetupArrow(FacingDirection, Stat.AttackInfo, _entityCombat);
    }
}
