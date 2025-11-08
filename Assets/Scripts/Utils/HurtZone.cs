using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class HurtZone : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onExit;

        private PlayerController _player;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            onEnter?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            onExit?.Invoke();
        }
    }
}
