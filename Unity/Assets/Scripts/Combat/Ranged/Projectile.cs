using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Base Projectile Details")]
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected float projectileLifeTime = 3f;
    [SerializeField] protected ParticleSystem impactParticles;

    WaitForSeconds projectileWait;
    
    private void Awake()
    {
        projectileWait = new WaitForSeconds(projectileSpeed);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateProjectile());
    }

    IEnumerator DeactivateProjectile()
    {
        yield return projectileWait;
        gameObject.SetActive(false);
    }
}
