using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/AOE/Damage Ability Component")]
public class AOEDamageAbilityComponentData : AbilityComponent
{
    [SerializeField] private float damageRadius;
    [SerializeField] private float damage;

    private Dictionary<UnitController, HashSet<LagCompensatedHit>> _damagedEnemies = new();

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        damage = -Mathf.Abs(damage);
        var enemyLayer = TeamManager.Instance.GetEnemyLayer(unit.Team.GetTeamNumber());
        List<LagCompensatedHit> hits = new ();
        unit.Runner.LagCompensation.OverlapSphere(unit.transform.position, damageRadius, unit.Object.InputAuthority, hits, enemyLayer);
        
        foreach (var hit in hits)
        {
            if (hit.GameObject == null) continue;
            if (_damagedEnemies.ContainsKey(unit))
            {
                if (_damagedEnemies[unit].Contains(hit)) continue;
            }
            else _damagedEnemies[unit] = new HashSet<LagCompensatedHit>();

            hit.GameObject.GetComponent<IHealth>().ModifyHealth(damage, null, new AttackDecalInfo());
            _damagedEnemies[unit].Add(hit);
        }
    }

    public override void DeactivateComponent(UnitController unit)
    {
        if (_damagedEnemies.ContainsKey(unit))
            _damagedEnemies[unit].Clear();
    }
}
