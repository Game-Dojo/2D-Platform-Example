using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private PlayerData playerData;
        
        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask slopeLayer;

        private PlayerAnimation _playerAnimation;
        private Rigidbody2D _rb;

        private float _coyoteCounter = 0;
        private float _jumpBufferCounter = 0;
        
        private void Awake()
        {
            _playerAnimation = GetComponent<PlayerAnimation>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _playerAnimation.CheckFlip(_rb.linearVelocityX);
            
            if (IsGrounded())
                _coyoteCounter = playerData.coyoteTime;
            else
                _coyoteCounter -= Time.deltaTime;
            
            if (Input.GetButtonDown("Jump"))
                _jumpBufferCounter = playerData.jumpBufferTime;
            else
                _jumpBufferCounter -= Time.deltaTime;
        }
        
        #region Actions
        public void Jump()
        {
            _coyoteCounter = 0;
            _jumpBufferCounter = 0;
            
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, playerData.jumpForce);
        }

        public void SetFallGravity()
        {
            _rb.gravityScale = playerData.fallGravityScale;
        }
        
        public void SetGroundGravity()
        {
            _rb.gravityScale = playerData.groundGravityScale;
        }

        public void Move()
        {
            _rb.linearVelocity = new Vector2(GetHorizontalInput() * playerData.moveSpeed, _rb.linearVelocity.y);
        }

        public void Stop()
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }

        public void ApplyHalfJump()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _rb.linearVelocityY * playerData.jumpReleasedForce);
        }
        
        #endregion
        
        #region Checkers
        public float GetHorizontalInput() => Input.GetAxisRaw("Horizontal");
        public bool CheckJump() => _jumpBufferCounter > 0f && _coyoteCounter > 0f;
        public bool CheckJumpReleased() => Input.GetButtonUp("Jump");
        public bool CheckFall() => !IsGrounded() && _rb.linearVelocityY < 0;
        public bool CheckLand() => IsGrounded() && _rb.linearVelocityY <= 0;
        public bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, groundLayer);
        public bool IsMoving() => _rb.linearVelocityX > 0.1f;
        
        #endregion
        
        private void OnDrawGizmos()
        {
            Gizmos.color =(IsGrounded()) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        }
    }
}
