using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeaponConfig", menuName = "Ref Game Configs/Melee Weapon Config")]
public class ReferenceMeleeWeaponConfig : ScriptableObject
{
    public float _damage;
    public Vector3 _hitboxSize; 
    public Vector3 _hitboxOffset; 
    public float _knockbackForce;
    public float _attackCooldown; 
}