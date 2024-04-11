using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UILevelUp : MonoBehaviour
{
    [SerializeField] private TMP_Text newLevelNumberText;
    [SerializeField] private Transform cardUpgradesContainer;
    [SerializeField] private AssetReferenceGameObject cardPrefab;
    [SerializeField] private UpgradesList upgradesList;
    [SerializeField] private GameObject player;
    [SerializeField] private LevelCard[] levelCards;
    private void CheckPlayerSystems()
    {
        PlayerSystem[] playerSystems = (PlayerSystem[])Enum.GetValues(typeof(PlayerSystem));
        List<Upgrade> actualUpgrades = new List<Upgrade>();

        foreach (PlayerSystem playerSystem in playerSystems)
        {
            if (TryGetPlayerSystem(playerSystem, out MonoBehaviour systemComponent))
            {
                actualUpgrades.Add(upgradesList.GetUpgrade(playerSystem));
            }
        }
    }

    private bool TryGetPlayerSystem<T>(PlayerSystem system, out T systemComponent) where T : Component
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
                if (player.TryGetComponent(out ThrowSystem ThrowSystem))
                    systemComponent = ThrowSystem as T;
                    break;
            case PlayerSystem.Health:
                if (player.TryGetComponent(out HealthSystem healthSystem))
                    systemComponent = healthSystem as T;
                    break;
                    default: return false;
        }
        return systemComponent != null;
    }
    [System.Serializable]
    private struct LevelCard
    {
        public Sprite ReverseSideSprite, ForegroundSprite, BackgroundSprite;
    }
}