using UnityEngine;

public class ReferenceEnemy : MonoBehaviour
{
    public void TakeDamage(float damage)
    {
        Debug.Log("Took: " + damage + " damage");
    }
}
