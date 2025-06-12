using System;
using UnityEngine;

public class ReferenceProjectileGrenade : ReferenceProjectile
{
    [SerializeField] private int numberOfBounces = 3;
    [SerializeField] private LayerMask layerMask;
    private Rigidbody _rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(transform.up * projectileSpeed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }
        
        numberOfBounces--;
        if (numberOfBounces == 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        Debug.Log("Kaboom!");
        RaycastHit[] hitObjects = Physics.SphereCastAll(transform.position, .5f, Vector3.up, 0f, layerMask);
        foreach (RaycastHit hit in hitObjects)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        
        Destroy(gameObject);
    }
}
