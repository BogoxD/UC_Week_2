using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int sceneIndex;
    public void LoadSceneIndex()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
