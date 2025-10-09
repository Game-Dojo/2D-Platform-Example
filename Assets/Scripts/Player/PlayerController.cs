using System;
using Scriptables;
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

        [Header("Debug")]
        [SerializeField] private bool showStateLabel = false;
        
        private PlayerAnimation _playerAnimation;
        private Rigidbody2D _rb;

        private float _coyoteCounter = 0;
        private float _jumpBufferCounter = 0;
        
        #region Unity Methods
        private void Awake()
        {
            _playerAnimation = GetComponent<PlayerAnimation>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SetGroundGravity();
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
        #endregion
        
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
            _rb.linearVelocity = new Vector2(CheckHorizontalInput() * playerData.moveSpeed, _rb.linearVelocity.y);
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
        public float CheckHorizontalInput() => Input.GetAxisRaw("Horizontal");
        public bool CheckJumpReleased() => Input.GetButtonUp("Jump");
        public bool CheckAttackInput() => Input.GetButtonDown("Fire1");
        public bool CheckHitButton() => Input.GetKeyDown(KeyCode.E);
        public bool CheckJump() => _jumpBufferCounter > 0f && _coyoteCounter > 0f;
        public bool CheckFall() => !IsGrounded() && _rb.linearVelocityY < 0;
        public bool CheckLand() => IsGrounded() && _rb.linearVelocityY <= 0;
        
        public bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, groundLayer);
        public bool IsMoving() => Mathf.Abs(_rb.linearVelocityX) > 0.01f;
        
        #endregion

        public bool ShowStateLabel => showStateLabel;
        private void OnDrawGizmos()
        {
            Gizmos.color =(IsGrounded()) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        }
    }
}
