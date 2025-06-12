using UnityEngine;

[CreateAssetMenu(fileName = "Melee Weapon Config", menuName = "Game Configs/Melee Weapon")]
public class MeleeWeaponConfig : ScriptableObject
{
    public Vector3 hitboxCenter;
    public Vector3 hitboxExtents;
    public LayerMask hitboxMask;
    public float damage;
    public float knockbackForce;
}
