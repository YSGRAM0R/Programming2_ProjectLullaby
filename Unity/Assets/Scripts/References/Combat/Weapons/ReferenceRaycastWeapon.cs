using UnityEngine;

public class ReferenceRaycastWeapon : ReferenceWeapon
{
    [SerializeField] float range = 100f;
    [SerializeField] private LayerMask targetMask;

    public override void Shoot()
    {
        Debug.DrawRay(muzzle.transform.position, transform.up * range, Color.red, 10f);
        if (Physics.Raycast(muzzle.transform.position, transform.up, out RaycastHit hit, range, targetMask))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }
    
}
