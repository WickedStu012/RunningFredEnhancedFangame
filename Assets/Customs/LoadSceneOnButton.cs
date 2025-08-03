using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnButton : MonoBehaviour
{
    [Header("Type the exact scene name from Build Settings")]
    public string sceneToLoad;

    // This function can be called from a UI Button
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is empty! Please type a scene name in the Inspector.");
        }
    }
}
