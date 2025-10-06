using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int Run = Animator.StringToHash("Run");
    
        private Animator _animator;
        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;
    
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }
    
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
        
        public void CheckFlip( float vx )
        {
            if (Mathf.Abs(vx) > 0.01f)
                _renderer.flipX = vx < 0;
        }
    }
}
