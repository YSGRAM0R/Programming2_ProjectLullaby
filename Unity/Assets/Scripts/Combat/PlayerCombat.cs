using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private InputController _inputController;

    [SerializeField] private Transform playerHands;
    private WeaponInventory _weaponInventory;
    
    [Header("Weapon Details")]
    [SerializeField] private Weapon equippedWeapon;

    private void Awake()
    {
        _inputController = GetComponent<InputController>();
        _weaponInventory = GetComponent<WeaponInventory>();
        if (equippedWeapon == null)
        {
            equippedWeapon = GetComponentInChildren<Weapon>();
        }
    }

    private void Start()
    {
        _inputController.AttackEvent += UseWeapon;
        _inputController.AttackEventCancelled += StopUsingWeapon;
        _inputController.EquipEvent += EquipWeapon;
    }

    private void EquipWeapon(int weaponIndex)
    {
        Weapon weaponToEquip = _weaponInventory.ReturnWeapon(weaponIndex);
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        if (weaponToEquip != null)
        {
            equippedWeapon = Instantiate(weaponToEquip, playerHands);
        }
    }

    void UseWeapon()
    {
        equippedWeapon.Use();
    }
    void StopUsingWeapon()
    {
        equippedWeapon.StopUsing();
    }
}
