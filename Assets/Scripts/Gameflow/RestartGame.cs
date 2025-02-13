using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameflow
{
    public class RestartGame : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}