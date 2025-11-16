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
        public void PlayGame()
        {
            SceneManager.LoadScene((int) Scenes.GameScene);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
