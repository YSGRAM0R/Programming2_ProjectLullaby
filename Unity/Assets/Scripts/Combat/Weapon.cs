using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Details")]
    [SerializeField] private float weaponCooldown = .2f;
    protected bool _onCooldown;

    public virtual void Use()
    {
        StartCoroutine(InitiateWeaponCooldown());
    }

    public virtual void StopUsing()
    {
        
    }

    IEnumerator InitiateWeaponCooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(weaponCooldown);
        _onCooldown = false;
    }
}
