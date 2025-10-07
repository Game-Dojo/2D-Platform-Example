using System.Collections;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private enum States
        {
            Idle,
            Run,
            Jump,
            Attack,
            Hit,
            Dead,
            Fall,
            Land
        }
    
        [SerializeField] private States currentState = States.Idle;
        [Header("User Interface")]
        [SerializeField] private TMP_Text stateLabel;
    
        private PlayerController _controller;
        private PlayerAnimation _animation;
    
        #region Unity Methods
        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _animation = GetComponent<PlayerAnimation>();
        }
        
        private void Update()
        {
            OnUpdateState();
        }

        private void FixedUpdate()
        {
            OnFixedState();
        }
        #endregion
        
        #region State Management
        private void OnEnterState(States newState)
        {
            switch (newState)
            {
                case States.Idle:
                    _controller.Stop();
                    _controller.SetGroundGravity();
                    _animation.SetIdleAnimation();
                    break;
                case States.Run:
                    _controller.SetGroundGravity();
                    _animation.SetRunAnimation();
                    break;
                case States.Jump:
                    _controller.Jump();
                    _animation.SetJumpAnimation();
                    break;
                case States.Fall:
                    _controller.SetFallGravity();
                    _animation.SetFallAnimation();
                    break;
                case States.Land:
                    _animation.SetLandAnimation();
                    break;
                case States.Attack:
                    _controller.Stop();
                    _animation.SetAttackAnimation();
                    break;
            }
        }

        private void OnUpdateState()
        {
            switch (currentState)
            {
                #region Idle State
                case States.Idle:
                    if (_controller.CheckHorizontalInput() != 0)
                        SetState(States.Run);
                
                    if (_controller.CheckJump())
                        SetState(States.Jump);
                    
                    if (_controller.CheckAttackInput())
                        SetState(States.Attack);
                    
                    if (_controller.CheckFall())
                        SetState(States.Fall);
                    break;
                #endregion
            
                #region Run State
                case States.Run:
                    if (_controller.CheckJump())
                        SetState(States.Jump);
                
                    if (_controller.CheckFall())
                        SetState(States.Fall);
                
                    if (_controller.CheckHorizontalInput() == 0)
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

        private void OnFixedState()
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
        
        private void SetState(States newState)
        {
            if (Equals(newState, currentState)) return;
            OnEnterState(newState);
            if (stateLabel) stateLabel.text = newState.ToString();
            currentState = newState;
        }
    }
}
