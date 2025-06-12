using UnityEngine;

public class ReferenceCombat : MonoBehaviour
{
    [SerializeField] private ReferenceWeapon equippedWeapon;
    ReferenceInputController _referenceInputController;

    private void Awake()
    {
        _referenceInputController = GetComponent<ReferenceInputController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _referenceInputController.FireEvent += FireWeapon;
        _referenceInputController.FireEventCancelled += StopFiring;
    }

    public void FireWeapon()
    {
        equippedWeapon?.Shoot();
    }

    public void StopFiring()
    {
        equippedWeapon?.StopShoot();
    }
}
