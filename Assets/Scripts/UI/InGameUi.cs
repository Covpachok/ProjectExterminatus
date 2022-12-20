using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class InGameUi : MonoBehaviour
    {
        public static bool gameIsPaused = false;
        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Pause()
        {
            if (!gameIsPaused)
            {
                gameIsPaused = true;
                Time.timeScale = 0f;
            }
            else
            {
                gameIsPaused = false;
                Time.timeScale = 1f;
            }
        }
    }
}
