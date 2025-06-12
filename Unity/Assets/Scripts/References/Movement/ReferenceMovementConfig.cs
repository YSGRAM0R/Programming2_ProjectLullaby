using UnityEngine;

[CreateAssetMenu(fileName = "NewMovementConfig", menuName = "Ref Game Configs/Reference Movement Config")]
public class ReferenceMovementConfig : ScriptableObject
{
    public float targetMoveSpeed = 5f;
    public float baseJumpForce = 8f;
    public float gravityMultiplier = 5f;
    public float accelerationRate = 10f;
    public float airAccelerationRate = 2f;
    public float lookSpeed = .2f;
}
