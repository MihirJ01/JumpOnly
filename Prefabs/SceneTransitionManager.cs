using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator Transition;
    public float time = 1f;

    // Load next level based on current build index
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    // Load scene by name
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    // Load scene by index
    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
    }

    // Coroutine to load scene by index
    IEnumerator LoadLevel(int levelIndex)
    {
        Transition.SetTrigger("start");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(levelIndex);
    }

    // Coroutine to load scene by name
    IEnumerator LoadLevel(string sceneName)
    {
        Transition.SetTrigger("start");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
    }
}
