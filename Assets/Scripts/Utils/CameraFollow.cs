using UnityEngine;

namespace Utils
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform followTarget;
        [SerializeField] private float followSpeed = 5.0f;
        [SerializeField] private Vector3 targetOffset;

        [Header("Limits")]
        [SerializeField] private bool limitCamera;
        [SerializeField] private Collider2D cameraLimits;
        
        [Header("Constraints")] [SerializeField] private bool ignoreY = false;

        private Vector3 _targetInitialPosition;
        
        private Camera _cam;

        private float _boundHeight;
        private float _boundWidth;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Start()
        {
            _boundHeight = _cam.orthographicSize;
            _boundWidth = _boundHeight * _cam.aspect;
            
            if(!followTarget) return;
            _targetInitialPosition = followTarget.position;
        }

        void LateUpdate()
        {
            if (!followTarget) return;

            Vector3 targetPosition = followTarget.position;
            targetPosition.y = !ignoreY ? followTarget.position.y : _targetInitialPosition.y;
            targetPosition += targetOffset;
            targetPosition.z = -10.0f;
            
            if (limitCamera && cameraLimits)
            {
                Bounds bounds = cameraLimits.bounds;

                var minX = bounds.min.x + _boundWidth;
                var maxX = bounds.max.x - _boundWidth;
                
                var minY = bounds.min.y + _boundHeight;
                var maxY = bounds.max.y - _boundHeight;

                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            }
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}