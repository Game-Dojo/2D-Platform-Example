using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public enum States
        {
            Idle,
            Run,
            Jump,
            Fall,
            Land
        }
    
        [SerializeField] private States currentState = States.Idle;
        [Header("User Interface")]
        [SerializeField] private TMP_Text stateLabel;
    
        private PlayerController _controller;
        private PlayerAnimation _animation;
    
        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _animation = GetComponent<PlayerAnimation>();
        }

        private void Start()
        {
            _controller.SetGroundGravity();
        }

        #region Loops
        private void Update()
        {
            switch (currentState)
            {
                #region Idle State
                case States.Idle:
                    if (_controller.GetHorizontalInput() != 0)
                        SetState(States.Run);
                
                    if (_controller.CheckJump())
                        SetState(States.Jump);
                    break;
                #endregion
            
                #region Run State
                case States.Run:
                    if (_controller.CheckJump())
                        SetState(States.Jump);
                
                    if (_controller.CheckFall())
                        SetState(States.Fall);
                
                    if (_controller.GetHorizontalInput() == 0)
                        SetState(States.Idle);
                    break;
                #endregion
            
                #region Jump State
                case States.Jump:
                    if (_controller.CheckFall())
                        SetState(States.Fall);
                
                    if (_controller.CheckLand())
                        SetState(States.Land);

                    if (_controller.CheckJumpReleased())
                        _controller.ApplyHalfJump();
                    break;
                #endregion
            
                #region Fall State
                case States.Fall:
                    if (_controller.CheckJump())
                        SetState(States.Jump);

                    if (_controller.CheckLand())
                        SetState(States.Land);
                    break;
                #endregion
            
                #region Land State
                case States.Land:
                    SetState(_controller.IsMoving() ? States.Run : States.Idle);
                    break;
                #endregion
            
                default:
                    SetState(States.Idle);
                    break;
            }
        }

        private void FixedUpdate()
        {
            switch (currentState)
            {
                case States.Run:
                case States.Jump:
                case States.Fall:
                    _controller.Move();
                    break;
            }
        }
        #endregion
    
        #region State Management
        public void SetState(States newState)
        {
            if (Equals(newState, currentState)) return;
        
            switch (newState)
            {
                case States.Idle:
                    _controller.Stop();
                    _controller.SetGroundGravity();
                    
                    _animation.ToggleRunAnimation(false);
                    _animation.ResetTrigger("Fall");
                    break;
                case States.Run:
                    _controller.SetGroundGravity();
                    
                    _animation.ToggleRunAnimation(true);
                    _animation.ResetTrigger("Fall");
                    break;
                case States.Jump:
                    _controller.Jump();
                    
                    _animation.ToggleRunAnimation(false);
                    _animation.TriggerAnimation("Jump");
                    break;
                case States.Fall:
                    _controller.SetFallGravity();
                    
                    _animation.TriggerAnimation("Fall");
                    break;
                case States.Land:
                    _animation.ResetTrigger("Fall");
                    _animation.TriggerAnimation("Land");
                    break;
            }
        
            if (stateLabel) stateLabel.text = newState.ToString();
        
            currentState = newState;
        }
        #endregion
    }
}
