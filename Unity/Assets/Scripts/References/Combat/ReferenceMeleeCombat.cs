using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour
{
    [SerializeField] private ReferenceMeleeWeaponConfig _weaponConfig;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Transform _aimTarget; 
    ReferenceInputController _referenceInputController;
    
    private bool _canAttack = true;
    private Vector3 _facingDirection = Vector3.forward;

    private void Awake()
    {
        _referenceInputController = GetComponent<ReferenceInputController>();
        _referenceInputController.FireEvent += HandleAttackInput;
    }

    private void HandleAttackInput() 
    {
        if (_canAttack)
        {
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        _canAttack = false;
        
        PerformHitDetection();

        yield return new WaitForSeconds(_weaponConfig._attackCooldown);
        _canAttack = true;
    }

    private void PerformHitDetection()
    {
        Vector3 hitboxCenter = transform.position 
                               + _weaponConfig._hitboxOffset.x * transform.right 
                               + _weaponConfig._hitboxOffset.y * transform.up 
                               + _weaponConfig._hitboxOffset.z * transform.forward;
            
        Vector3 hitboxSize = _weaponConfig._hitboxSize * 0.5f;
        Collider[] hitEnemies = Physics.OverlapBox(hitboxCenter, hitboxSize, transform.rotation, _enemyLayerMask);

        if (hitEnemies != null)
        {
            foreach (Collider enemy in hitEnemies)
            {
                ReferenceEnemy hitEnemy = enemy.gameObject.GetComponent<ReferenceEnemy>();
                hitEnemy.TakeDamage(_weaponConfig._damage);
                
                Rigidbody enemyRigidbody = hitEnemy.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    Vector3 knockbackDirection = (hitEnemy.transform.position - transform.position).normalized;
                    enemyRigidbody.AddForce(knockbackDirection * _weaponConfig._knockbackForce, ForceMode.Impulse);
                }
            }
        }
    }
    
        private void OnDrawGizmosSelected()
        {
            if (_weaponConfig == null) return;

            Vector3 hitboxCenter = transform.position 
                                   + _weaponConfig._hitboxOffset.x * transform.right 
                                   + _weaponConfig._hitboxOffset.y * transform.up 
                                   + _weaponConfig._hitboxOffset.z * transform.forward;
            
            Vector3 hitboxSize = _weaponConfig._hitboxSize;
    
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitboxCenter, hitboxSize);
        }
}