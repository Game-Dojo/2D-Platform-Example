using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    
    [Header("Jump")]
    public float jumpForce = 10f;
    public float jumpReleasedForce = 0.5f;
    
    [Header("Gravity")]
    public float groundGravityScale = 1.0f;
    public float fallGravityScale = 2.0f;

    [Header("Juice")] 
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
}
