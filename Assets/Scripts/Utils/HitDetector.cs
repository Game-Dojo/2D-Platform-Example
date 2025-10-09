using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Utils
{
    public class HitDetector : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("Hit");
        
        [SerializeField] private GameObject hitEffect;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Dummy")) return;
            other.gameObject.GetComponentInChildren<Animator>().SetTrigger(Hit);

            if (!hitEffect) return;
            InstantiateFx(other.transform.position);
        }

        private void InstantiateFx(Vector3 position)
        {
            var randomPosition = position + Vector3.up + (Random.insideUnitSphere * 0.3f);
            GameObject go = Instantiate(hitEffect, randomPosition, Quaternion.identity);
            if (go) Destroy(go, 0.4f);
        }
    }
}
