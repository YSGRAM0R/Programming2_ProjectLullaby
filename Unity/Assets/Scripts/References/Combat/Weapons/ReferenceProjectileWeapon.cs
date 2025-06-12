using UnityEngine;

public class ReferenceProjectileWeapon : ReferenceWeapon
{
    [SerializeField] ReferenceProjectile projectile;
    
    public override void Shoot()
    {
        if (!CanShoot()) return;
        
        base.Shoot();
        Instantiate(projectile, muzzle.transform.position, muzzle.transform.rotation);
    }
}
