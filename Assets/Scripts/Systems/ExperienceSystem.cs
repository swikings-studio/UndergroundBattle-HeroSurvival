using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwiKinGsStudio.UI;
using UnityEngine.Events;

public class ExperienceSystem : MonoBehaviour
{
    [Range(0f, 10f), SerializeField] private float radius;
    [SerializeField] private LayerMask neededLayerMask;
    [SerializeField] private Slider experienceBar;
    [SerializeField] private UnityEvent onLevelUp;
    private int level = 1, experience = 0, neededExperience = 4;
    private const float actionTime = 0.15f;
    private Collider[] colliders;
    private void Start()
    {
        UpdateExperienceBar();
    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, neededLayerMask, QueryTriggerInteraction.Collide);
        if (colliders.Length > 0)
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out ExperienceOrb experienceOrb))
                {
                    experienceOrb.MoveTo(transform, AddExperiencePoints);
                }
            }
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
}