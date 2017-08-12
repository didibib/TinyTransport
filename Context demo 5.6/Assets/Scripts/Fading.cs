using UnityEngine;
using System.Collections;

// https://www.youtube.com/watch?v=0HwZQt94uHQ

public class Fading : MonoBehaviour
{
    public static Fading instance = null;

    public Texture2D fadeOutTexture;
    private float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private float fadeDir = -1; // the direction to fade: in = -1 or out = 1;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    //DontDestroyOnLoad(gameObject);

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(float direction, float speed)
    {
        fadeSpeed = speed;
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1, fadeSpeed);
    }
}
