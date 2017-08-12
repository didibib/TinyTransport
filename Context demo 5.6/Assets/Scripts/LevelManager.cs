using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public float fadeSpeed = 0.8f;

    IEnumerator num_NextLevel() {
        float fadeTime = Fading.instance.BeginFade(1, fadeSpeed);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    IEnumerator num_BackLevel()
    {
        float fadeTime = Fading.instance.BeginFade(1, fadeSpeed);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(Application.loadedLevel - 1);
    }

    IEnumerator Quit() {
        float fadeTime = Fading.instance.BeginFade(1, fadeSpeed);
        yield return new WaitForSeconds(fadeTime);
        Application.Quit();
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    public void NextLevel()
    {
        StartCoroutine(num_NextLevel());
    }

    public void BackLevel()
    {
        StartCoroutine(num_BackLevel());
    }
}
