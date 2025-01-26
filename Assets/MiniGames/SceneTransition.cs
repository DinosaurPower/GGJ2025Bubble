using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    private void Awake()
    {
        // If there?s already an instance and it?s not this, destroy this GameObject.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, make this the single instance and optionally persist across scenes.
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Header("UI Image used for fading")]
    public Image fadeImage; // Assign a full-screen black Image in the Inspector

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Time (in seconds) to fade in/out

    /// <summary>
    /// Call this method (e.g., from a button) to transition to the next scene.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(DoTransition(sceneName));
    }

    /// <summary>
    /// Main coroutine that handles the fade out, scene load, and fade in.
    /// </summary>
    private IEnumerator DoTransition(string sceneName)
    {
        // 1) Fade to black
        fadeImage.gameObject.SetActive(true);
        yield return StartCoroutine(Fade(0f, 1f));

        // 2) Load the new scene
        SceneManager.LoadScene(sceneName);

        // 3) Fade from black back to clear
        yield return StartCoroutine(Fade(1f, 0f));
        fadeImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fades the image alpha from startAlpha to endAlpha over fadeDuration.
    /// </summary>
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color originalColor = fadeImage.color;

        // Ensure the color’s starting alpha is set
        fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, startAlpha);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        // Ensure final alpha is set exactly
        fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, endAlpha);
    }

    public void TransitionToSceneMiniGame()
    {
        StartCoroutine(DoTransition("MiniGame"));
    }

        public void TransitionToSceneMain()
    {
        StartCoroutine(DoTransition("MainScene"));
    }

    

}
