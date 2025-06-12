using System;
using UnityEngine;

public class ReferenceProjectile : MonoBehaviour
{
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] float projectileLifeTime = 3f;
    [SerializeField] protected ParticleSystem deathParticles;

    private void Start()
    {
        Destroy(gameObject, projectileLifeTime);
    }

    private void OnDestroy()
    {
        if (deathParticles == null) return;
        ParticleSystem finalParticles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        finalParticles.Play();
    }
}
