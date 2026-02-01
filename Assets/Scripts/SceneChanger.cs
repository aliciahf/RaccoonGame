using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadNextScene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene button pressed");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit button pressed, doesn't quit in editor");
    }
}
