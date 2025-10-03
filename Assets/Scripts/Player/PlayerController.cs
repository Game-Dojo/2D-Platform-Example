using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int Run = Animator.StringToHash("Run");

        [Header("Properties")] 
        [SerializeField] private PlayerData playerData;
        
        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask slopeLayer;

        private PlayerStateMachine _stm;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _renderer;

        private float _coyoteCounter = 0;
        private float _jumpBufferCounter = 0;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            
            _stm = GetComponent<PlayerStateMachine>();
        }

        private void Start()
        {
            _stm.SetState(PlayerStateMachine.States.Idle);
        }

        private void Update()
        {
            if (IsGrounded())
                _coyoteCounter = playerData.coyoteTime;
            else
                _coyoteCounter -= Time.deltaTime;
            
            if (Input.GetButtonDown("Jump"))
                _jumpBufferCounter = playerData.jumpBufferTime;
            else
                _jumpBufferCounter -= Time.deltaTime;
        }
        
        #region Animations
        
        public void ToggleRunAnimation(bool state)
        {
            _animator.SetBool(Run, state);
        }
        public void TriggerAnimation(string state)
        {
            _animator.SetTrigger(state);
        }
        public void ResetTrigger(string state)
        {
            _animator.ResetTrigger(state);
        }
        
        public void CheckFlip()
        {
            float vx = _rb.linearVelocity.x;

            if (Mathf.Abs(vx) > 0.01f)
                _renderer.flipX = vx < 0;
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

        public void ToggleKinematic(bool state)
        {
            _rb.bodyType = (state) ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        }
        
        #endregion
        
        #region Checkers
        public float GetHorizontalInput() => Input.GetAxisRaw("Horizontal");
        public bool CheckJump() => _jumpBufferCounter > 0f && _coyoteCounter > 0f;
        public bool CheckJumpReleased() => Input.GetButtonUp("Jump");
        public bool CheckFall() => !IsGrounded() && _rb.linearVelocityY < 0;
        public bool CheckLand() => IsGrounded() && _rb.linearVelocityY <= 0;
        public bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
        //public bool IsOnSlope() => Physics2D.OverlapCircle(groundCheck.position, 0.15f, slopeLayer);
        public bool IsMoving() => _rb.linearVelocityX > 0.1f;
        
        #endregion
        
        private void OnDrawGizmos()
        {
            Gizmos.color =(IsGrounded()) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.15f);
        }
    }
}
