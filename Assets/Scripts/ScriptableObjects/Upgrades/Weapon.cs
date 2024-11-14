using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "SwiKinGs Studio/Create Weapon", order = 0)]
public class Weapon : AttackUpgrade
{
    public AttackType attackType;
    public HitVariation animationVariation;
    public bool spawnAsWeapon = true;
    public float DelayBeforeEffect;

    public float AnimationHitVariation => (float)animationVariation;

    public IEnumerator EnableParticlesWithDelay(GameObject weaponObject)
    {
        if (weaponObject is null) yield break;
        
        var weaponParticles = weaponObject.GetComponentInChildren<ParticleSystem>();
        if (weaponParticles)
        {
            yield return new WaitForSeconds(DelayBeforeEffect);
            weaponParticles.Play();
        }
    }
}