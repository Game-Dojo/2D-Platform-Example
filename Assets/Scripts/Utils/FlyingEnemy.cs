using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    public class FlyingEnemy : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [Header("Forces")]
        [SerializeField] private float attackForce = 6.0f;
        [Header("Properties")]
        [SerializeField] private float slowdownTime = 0.8f;
        [SerializeField] private float airFriction = 1.6f;
        [SerializeField] private float maxVelocity = 15f;
        
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        
        private Vector3 _playerDirection;
        
        private bool _isInside = false;
        private bool _stopping = false;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            if (!_isInside || !_stopping) return;
            if (!(_rb.linearVelocity.magnitude < 0.01f)) return;
            StopMovement();
        }
        
        private void FixedUpdate()
        {
            if (_rb.linearVelocity.magnitude > maxVelocity)
                _rb.linearVelocity = _rb.linearVelocity.normalized * maxVelocity;
        }

        public void SetIsInside(bool isInside)
        {
            _isInside = isInside;
            if (_isInside) 
                InvokeRepeating(nameof(Attack), 0, 2.0f);
            else
                CancelInvoke(nameof(Attack));
        }
        
        private void Attack()
        {
            if (!_isInside) return;
            
            StopMovement();
            
            _sr.flipX = player.position.x < transform.position.x;
            
            var direction = (player.position - transform.position).normalized;
            _rb.AddForce(direction * attackForce, ForceMode2D.Impulse);
            
            CancelInvoke(nameof(Slowdown));
            Invoke(nameof(Slowdown), slowdownTime);
        }
        private void StopMovement()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.linearDamping = 0;
            _stopping = false;
        }
        private void Slowdown()
        {
            _stopping = true;
            _rb.linearDamping = airFriction;
        }
    }
}
