using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int JumpPreparation = Animator.StringToHash("JumpPrep");
        private static readonly int Land = Animator.StringToHash("Land");
        
        private static readonly int MovingParam = Animator.StringToHash("Moving");

        private Animator _animator;
        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;
    
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void CheckFlip( float velocityX )
        {
            if (Mathf.Abs(velocityX) > 0.01f)
                transform.localScale = new Vector3((velocityX < 0) ? -1 : 1, transform.localScale.y, transform.localScale.z);
            //_renderer.flipX = velocityX < 0;
        }
        
        public void SetIdleAnimation()
        {
            _animator.SetBool(MovingParam, false);
        }

        public void SetRunAnimation()
        {
            _animator.SetBool(MovingParam, true);
        }
        
        public void SetFallAnimation()
        {
            SetAnimation(Fall);
        }
        
        public void SetJumpAnimation()
        {
            SetAnimation(JumpPreparation);
        }
        
        public void SetLandAnimation()
        {
            SetAnimation(Land);
        }
        
        public void SetAttackAnimation()
        {
            SetAnimation(Attack);
        }

        public void SetDeathAnimation()
        {
            SetAnimation(Death);
        }

        public void SetHitAnimation()
        {
            SetAnimation(Hit);
        }
        
        private void SetAnimation(int animHash)
        {
            _animator.CrossFadeInFixedTime(animHash, 0.05f);
        }
        
        public Animator GetAnimator() => _animator;
    }
}
