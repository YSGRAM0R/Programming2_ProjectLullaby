using UnityEngine;

public class HitscanWeapon : RangedWeapon
{
    [Header("Hitscan Details")]
    [SerializeField] float range = 100f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private ParticleSystem hitParticles;
    
    public override void Use()
    {
        if (!CanShoot()) return;
        base.Use();
        
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.up, out RaycastHit hit, range, targetMask))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.transform.gameObject);
            }
            Instantiate(hitParticles, hit.point, Quaternion.identity);
        }
    }
}
