using UnityEngine;

namespace Utils
{
    public class HitDetector : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("Hit");

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Dummy")) return;
            other.gameObject.GetComponentInChildren<Animator>().SetTrigger(Hit);
        }
    }
}
