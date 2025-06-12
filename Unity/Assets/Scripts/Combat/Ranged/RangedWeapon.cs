using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Weapon Details")]
    [SerializeField] protected Transform muzzle;
    [SerializeField] private bool bAutomatic;
    private bool _autoActive;
    
    [Header("Ammunition Details")]
    [SerializeField] private int ammoCost = 1;
    [SerializeField] private int maxAmmo;
    private int _currentAmmo;
    
    private void Start()
    {
        _currentAmmo = maxAmmo;
    }
    
    void Update()
    {
        if (_autoActive)
        {
            Use();
        }
    }

    public override void Use()
    {
        base.Use();
        _currentAmmo = Mathf.Clamp(_currentAmmo - ammoCost, 0, maxAmmo);
        if(bAutomatic) _autoActive = true;
    }
    
    public override void StopUsing()
    {
        if(bAutomatic) _autoActive = false;
    }
    
    protected bool CanShoot()
    {
        return _currentAmmo >= ammoCost && !_onCooldown;
    }
}
