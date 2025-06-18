using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform cam;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptyMagSound;

    float timeSinceLastShot;

    private void Start() 
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReload() 
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload() 
    {
        gunData.reloading = true;

        // Play reload sound
        if (audioSource != null && reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot() 
    {
        if (gunData.reloading)
            return;

        if (gunData.currentAmmo > 0 && CanShoot()) 
        {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance))
            {
                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                damageable?.TakeDamage(gunData.damage);
            }

            gunData.currentAmmo--;
            timeSinceLastShot = 0;
            OnGunShot();
        }
        else if (gunData.currentAmmo <= 0 && !gunData.reloading)
        {
            PlayEmptyMagSound();
        }
    }

    private void Update() 
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance);
    }

    private void OnGunShot()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void PlayEmptyMagSound()
    {
        if (audioSource != null && emptyMagSound != null)
        {
            audioSource.PlayOneShot(emptyMagSound);
        }
    }
}