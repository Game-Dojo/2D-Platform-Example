using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onStay;
        [SerializeField] private UnityEvent onExit;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            onEnter?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            onStay?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            onExit?.Invoke();
        }
    }
}
