using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwiKinGsStudio.UI;
using UnityEngine.Events;

public class CollectSystem : BaseSystem, IUpgradable
{
    [Range(0f, 10f), SerializeField] private float radius;
    [SerializeField] private LayerMask neededLayerMask;
    [SerializeField] private Slider experienceBar;
    [SerializeField] private UnityEvent onLevelUp, onCoinCollect;
    private int _level = 1, _experience = 0, _neededExperience = 4, _coins;
    private const float ActionTime = 0.15f;
    private Collider[] _colliders;
    private const int Interval = 3;
    public int Level => _level;
    private void Start()
    {
        UpdateExperienceBar();
    }

    private void Update()
    {
        if (Time.frameCount % Interval != 0) return;

        _colliders = Physics.OverlapSphere(transform.position, radius, neededLayerMask, QueryTriggerInteraction.Collide);
        if (_colliders.Length > 0)
            foreach (Collider collider in _colliders)
            {
                if (collider.TryGetComponent(out CollectableItem collectableItem) && collectableItem.IsTriggered == false)
                {
                    UnityAction<int> callbackOnComplete = null;

                    if (collider.CompareTag("ExperienceOrb")) callbackOnComplete = AddExperiencePoints;
                    else if (collider.CompareTag("Coin")) callbackOnComplete = AddCoins;

                    collectableItem.MoveTo(transform, callbackOnComplete);
                }
            }
    }

    private void AddCoins(int count)
    {
        _coins += count;
    }

    private void AddExperiencePoints(int count)
    {
        _experience += count;
        if (_experience >= _neededExperience)
        {
            LevelUp();
        }
        UpdateExperienceBar();
    }
    private void LevelUp()
    {
        _level++;
        _neededExperience *= 2;
        _experience = 0;
        GameTimeManager.SetTimeScaleSmoothly(0.5f, 0, 0.5f);
        onLevelUp.Invoke();
    }
    private void UpdateExperienceBar()
    {
        if (experienceBar == null) return;

        float neededValue = (float)_experience / _neededExperience;
        experienceBar.SetSmoothlyValue(neededValue, ActionTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public PlayerSystem PlayerSystem { get; set; } = PlayerSystem.Collect;

    public void Upgrade(float value)
    {
        radius += value;
    }
}