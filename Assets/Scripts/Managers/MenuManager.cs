using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
