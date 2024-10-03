using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwiKinGsStudio.UI;
using UnityEngine.Events;

public class CollectSystem : BaseSystem
{
    [Range(0f, 10f), SerializeField] private float radius;
    [SerializeField] private LayerMask neededLayerMask;
    [SerializeField] private Slider experienceBar;
    [SerializeField] private UnityEvent onLevelUp, onCoinCollect;
    private int level = 1, experience = 0, neededExperience = 4, coins;
    private const float actionTime = 0.15f;
    private Collider[] colliders;
    private const int interval = 3;
    private void Start()
    {
        UpdateExperienceBar();
    }

    private void Update()
    {
        if (Time.frameCount % interval != 0) return;

        colliders = Physics.OverlapSphere(transform.position, radius, neededLayerMask, QueryTriggerInteraction.Collide);
        if (colliders.Length > 0)
            foreach (Collider collider in colliders)
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
        coins += count;
    }

    private void AddExperiencePoints(int count)
    {
        experience += count;
        if (experience >= neededExperience)
        {
            LevelUp();
        }
        UpdateExperienceBar();
    }
    private void LevelUp()
    {
        level++;
        neededExperience *= 2;
        experience = 0;
        GameTimeManager.SetTimeScaleSmoothly(0.5f, 0, 0.5f);
        onLevelUp.Invoke();
    }
    private void UpdateExperienceBar()
    {
        if (experienceBar == null) return;

        float neededValue = (float)experience / neededExperience;
        experienceBar.SetSmoothlyValue(neededValue, actionTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public override void Upgrade(float value)
    {
        radius += value;
    }
}