using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LocalizedString localizedGreeting;

        void Start()
        {
            // Sincronico
            string greeting = localizedGreeting.GetLocalizedString();
            Debug.Log(greeting);

            // Asíncrono 
            StartCoroutine(GetLocalizedGreeting());
        }
        
        IEnumerator GetLocalizedGreeting()
        {
            var stringOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Ingame Labels", "ingame.topText");
            yield return stringOperation;

            if (stringOperation.IsDone && stringOperation.Result != null)
            {
                string greeting = stringOperation.Result;
                Debug.Log(greeting);
            }
            else
            {
                Debug.LogError("Failed to get localized string.");
            }
        }
    }
}
