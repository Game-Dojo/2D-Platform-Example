using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LocalizedString localizedGreeting;
        [SerializeField] private Transform dummy;
        [SerializeField] private Transform player;
        void Start()
        {
            //LoadTranslations();

            /*dummy.DOMoveX(player.position.x, 3.0f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(1.0f);*/
            
            dummy.DOJump(player.position, 4, 1, 6.0f)
                .OnComplete(() =>
                {
                    player.position += Vector3.right * 4;

                    Camera.main.DOShakePosition(0.2f, 1.0f);
                });
        }

        
        private void LoadTranslations()
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
