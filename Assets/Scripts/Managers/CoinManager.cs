
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 0.5f;

    private List<float> _startPositions = new List<float>();
    private void Start()
    {
        foreach (Transform coin in transform)
        {
            _startPositions.Add(coin.position.y);
        }
    }

    void Update()
    {
        var auxiliarIndex = 0;
        foreach (Transform coin in transform)
        {
            var posY = _startPositions[auxiliarIndex] + (amplitude * Mathf.Sin(Time.time * frequency));
            coin.localPosition = new Vector3(coin.localPosition.x,  posY, 0);

            auxiliarIndex++;
        }
    }
}
