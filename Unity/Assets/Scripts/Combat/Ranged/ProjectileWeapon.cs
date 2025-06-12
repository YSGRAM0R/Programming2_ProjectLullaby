using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : RangedWeapon
{
    [Header("Projectile Details")]
    [SerializeField] private Projectile projectile;
    
    private List<Projectile> projectilePool = new List<Projectile>();
    
    public override void Use()
    {
        if (!CanShoot()) return;
        base.Use();
        
        Projectile projectileToShoot = GetProjectile();
        projectileToShoot.transform.position = muzzle.transform.position;
        projectileToShoot.transform.rotation = muzzle.transform.rotation;
        projectileToShoot.gameObject.SetActive(true);
    }

    Projectile GetProjectile()
    {
        foreach (Projectile projectile in projectilePool)
        {
            if (!projectile.gameObject.activeInHierarchy)
            {
                return projectile;
            }
        }
        Projectile newProjectile = Instantiate(projectile);
        projectilePool.Add(newProjectile);
        return newProjectile;
    }
}
