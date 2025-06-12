using System.Collections;
using UnityEngine;

public class ReferenceWeapon : MonoBehaviour
{
    [Header("Default Weapon Details")]
    [SerializeField] private int maxAmmo = 100;
    private int _currentAmmo = 0;
    [SerializeField] private int ammoRequired = 1;
    [SerializeField] private float shootDelay = .5f;
    [SerializeField] protected bool bAutomatic = false;
    [SerializeField] protected Transform muzzle;
    private bool _onCooldown;
    private WaitForSeconds _cooldownWait;
    private bool _autoActive;
    
    private void Start()
    {
        _cooldownWait = new WaitForSeconds(shootDelay);
        _currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (_autoActive && CanShoot())
        {
            Shoot();
        }
    }

    public virtual void Shoot()
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo -= ammoRequired, 0, maxAmmo);
        StartCoroutine(ShootCooldown());
        if (bAutomatic)
        {
            _autoActive = true;
        }
        // Could play a sound here
        // Could also play a particle effect
    }

    public void StopShoot()
    {
        if (bAutomatic)
        {
            _autoActive = false;
        }
    }

    protected bool CanShoot()
    {
        return ammoRequired <= _currentAmmo && !_onCooldown;
    }

    public virtual void Reload(int ammoToAdd)
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo + ammoToAdd, 0, maxAmmo);
    }

    IEnumerator ShootCooldown()
    {
        _onCooldown = true;
        yield return _cooldownWait;
        _onCooldown = false;
    }
}
