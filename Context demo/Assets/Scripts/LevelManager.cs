using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    IEnumerator num_NextLevel() {
        float fadeTime = GameObject.FindWithTag("GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    IEnumerator num_BackLevel()
    {
        float fadeTime = GameObject.FindWithTag("GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(Application.loadedLevel - 1);
    }

    IEnumerator Quit() {
        float fadeTime = GameObject.FindWithTag("GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.Quit();
    }

    public void QuitGame()
    {
        StartCoroutine("Quit");
    }

    public void NextLevel()
    {
        StartCoroutine("num_NextLevel");
    }

    public void BackLevel()
    {
        StartCoroutine("num_BackLevel");
    }
}
