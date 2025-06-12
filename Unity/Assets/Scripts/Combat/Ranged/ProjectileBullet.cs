using UnityEngine;

public class ProjectileBullet : Projectile
{
    private void Update()
    {
        transform.Translate(Vector3.up * (projectileSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        Instantiate(impactParticles, transform.position, transform.rotation);
        gameObject.SetActive(false);
        // Play impact particles
    }
}
