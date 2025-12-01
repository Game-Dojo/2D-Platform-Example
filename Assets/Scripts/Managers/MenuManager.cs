using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public enum Scenes
    {
        MenuScene,
        GameScene
    }
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainGroup;
        [SerializeField] private CanvasGroup settingsGroup;

        private bool _settingsOpen = false;

        private IEnumerator Start()
        {
            mainGroup.alpha = 0;
            mainGroup.DOFade(1, 2.0f);
            
            foreach (RectTransform button in mainGroup.transform)
            {
                button.localPosition = new Vector3(button.localPosition.x + 2.0f, button.localPosition.y, button.localPosition.z);
                button.DOAnchorPosX(0, 2.0f);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void PlayGame()
        {
            SceneManager.LoadScene((int) Scenes.GameScene);
        }

        public void Settings()
        {
            ToggleSettings();
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            
            Application.Quit();
        }

        private void ToggleSettings()
        {
            _settingsOpen = !_settingsOpen;
            
            settingsGroup.alpha = (_settingsOpen) ? 1 : 0;
            settingsGroup.interactable = _settingsOpen;
            settingsGroup.blocksRaycasts = _settingsOpen;
        }
    }
}
