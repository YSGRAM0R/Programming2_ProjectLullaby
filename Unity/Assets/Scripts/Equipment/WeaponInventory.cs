using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] Weapon[] weaponList;

    public Weapon ReturnWeapon(int index)
    {
        if (index > weaponList.Length - 1) return null;
        return weaponList[index];
    }
}
