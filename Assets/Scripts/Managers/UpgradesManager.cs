using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradesManager
{
    private Dictionary<Upgrade, int> _upgradesLevel;
    
    private GameObject player;

    public UpgradesManager(GameObject player)
    {
        this.player = player;
        _upgradesLevel = new Dictionary<Upgrade, int>();
    }

    public void ApplyUpgrade<T>(T upgrade) where T : Upgrade
    {
        if (upgrade is UpgradeSystem system) UpgradeSystem(system);
        else if (upgrade is Weapon weapon)
        {
            
        }
        else if (upgrade is Ability ability)
        {
            
        }
    }

    private void UpgradeSystem(UpgradeSystem upgrade)
    {
        if (TryGetPlayerSystem(upgrade.System, out BaseSystem component))
        {
            int upgradeLevel = GetUpgrateLevel(upgrade);
            component.Upgrade(upgrade.LevelValues[upgradeLevel - 1]);
            CheckUpgrade(upgrade);
            UpgradeLevelUp(upgrade);
        }
        else
        {
            Debug.LogError("Failed for find player system with upgrade " + upgrade.Title);
        }
    }
    
    private void UpgradeLevelUp(Upgrade upgrade)
    {
        if (_upgradesLevel.ContainsKey(upgrade))
        {
            int newLevel = _upgradesLevel[upgrade] + 1;
            if (newLevel > upgrade.LevelValues.Length)
            {
                Debug.LogError("Trying to add over max level to " + upgrade.Title);
                return;
            }
            
            _upgradesLevel[upgrade] = newLevel;
            Debug.Log($"Upgrade {upgrade.Title} level upped. Now level is {_upgradesLevel[upgrade]}");
        }
    }

    private void CheckUpgrade(Upgrade upgrade)
    {
        if (_upgradesLevel.ContainsKey(upgrade)) return;

        _upgradesLevel.Add(upgrade, 1);
    }

    public int GetUpgrateLevel(Upgrade upgrade)
    {
        if (_upgradesLevel.Count == 0 || _upgradesLevel.ContainsKey(upgrade) == false) return 1;

        foreach (var upgradeLevel in _upgradesLevel)
        {
            if (upgrade == upgradeLevel.Key) return upgradeLevel.Value;
        }

        return 1;
    }

    private bool TryGetPlayerSystem<T>(PlayerSystem system, out T systemComponent) where T : BaseSystem
    {
        systemComponent = default;

        switch (system)
        {
            case PlayerSystem.Move:
                if (player.TryGetComponent(out MoveSystem moveSystem))
                    systemComponent = moveSystem as T;
                break;
            case PlayerSystem.Dash:
                if (player.TryGetComponent(out DashSystem dashSystem))
                    systemComponent = dashSystem as T;
                break;
            case PlayerSystem.Attack:
                if (player.TryGetComponent(out AttackSystem attackSystem))
                    systemComponent = attackSystem as T;
                break;
            case PlayerSystem.Collect:
                if (player.TryGetComponent(out CollectSystem collectSystem))
                    systemComponent = collectSystem as T;
                break;
            case PlayerSystem.Throw:
                if (player.TryGetComponent(out ThrowSystem throwSystem))
                    systemComponent = throwSystem as T;
                break;
            case PlayerSystem.Health:
                if (player.TryGetComponent(out HealthSystem healthSystem))
                    systemComponent = healthSystem as T;
                break;
            default: return false;
        }

        return systemComponent != null;
    }

}