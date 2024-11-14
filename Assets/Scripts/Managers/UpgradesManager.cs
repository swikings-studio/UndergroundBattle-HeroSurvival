using System;
using System.Collections.Generic;
using UnityEngine;
public class UpgradesManager
{
    private static Dictionary<Upgrade, int> _upgradesLevel;
    
    private readonly GameObject _player;
    private readonly AttackSystem _attackSystem;
    public UpgradesManager(GameObject player)
    {
        _player = player;
        _attackSystem = player.GetComponent<AttackSystem>();
        _upgradesLevel = new Dictionary<Upgrade, int>();
    }

    public void ApplyUpgrade<T>(T upgrade) where T : Upgrade
    {
        switch (upgrade)
        {
            case UpgradeSystem system:
                UpgradeSystem(system);
                break;
            case Weapon:
            case Ability:
                UpgradeLevelUp(upgrade);
                _attackSystem.AddAttackUpgrade(upgrade as AttackUpgrade);
                break;
        }
    }

    private void UpgradeSystem(UpgradeSystem upgrade)
    {
        var components = _player.GetComponents<IUpgradable>();
        foreach (var component in components)
        {
            if (component.PlayerSystem != upgrade.System) continue;
            
            UpgradeLevelUp(upgrade);
            component.Upgrade(upgrade.Value);
            return;
        }
        
        Debug.LogError("Component not founded for upgrade system from " + upgrade.Title);
    }
    
    private void UpgradeLevelUp(Upgrade upgrade)
    {
        if (_upgradesLevel.TryAdd(upgrade, 0))
        {
            Debug.Log("New upgrade added " + upgrade.Title);
        }
        else
        {
            var newLevel = _upgradesLevel[upgrade] + 1;
            if (newLevel > upgrade.LevelValues.Length)
            {
                Debug.LogError("Trying to add over max level to " + upgrade.Title);
                return;
            }
            
            _upgradesLevel[upgrade] = newLevel;
            Debug.Log($"Upgrade {upgrade.Title} level upped. Now level is {_upgradesLevel[upgrade]}");
        }
    }
    
    public static int GetUpgradeLevel(Upgrade upgrade)
    {
        try
        { 
            return _upgradesLevel.GetValueOrDefault(upgrade, 0);
        }
        catch
        {
            return 0;
        }
    }
}