using System;
using UnityEngine;

public class ReferenceProjectileBullet : ReferenceProjectile
{
    void Update()
    {
        transform.Translate(transform.up * (projectileSpeed * Time.deltaTime), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        
        Destroy(gameObject);
    }
}
