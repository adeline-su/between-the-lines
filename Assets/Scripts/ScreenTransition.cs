using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenTransition : MonoBehaviour
{
    public static ScreenTransition Instance;

    [Header("UI References")]
    public Image backgroundImage;
    public Image destinationImage;     // changes depending on destination
    public CanvasGroup canvasGroup;    // used for fading

    [Header("Settings")]
    public float fadeDuration = 0.5f;

    // Map scene names to images
    public List<SceneImageMapping> sceneImageMappings;
    private Dictionary<string, Sprite> sceneToSprite;

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build lookup dictionary
        sceneToSprite = new Dictionary<string, Sprite>();
        foreach (var mapping in sceneImageMappings)
            sceneToSprite[mapping.sceneName] = mapping.sprite;

        Debug.Log("ScreenTransition awake " + sceneToSprite);

    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        Debug.Log("in Transition function " + sceneName);

        // Set destination image
        if (sceneToSprite.TryGetValue(sceneName, out var sprite)) {
            destinationImage.sprite = sprite;
            destinationImage.enabled = true;
        } else {
            destinationImage.enabled = false; 
        }

        // Show background
        if (backgroundImage != null)
            backgroundImage.enabled = true;

        // Fade in
        yield return Fade(0f, 1f);

        // load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        // reposition player if needed
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = GameManager.Instance.nextScenePosition;
            // GameManager.Instance.nextScenePosition = null;
            cc.enabled = true;

            Debug.Log("Teleported to " + player.transform.position);
        }

        // Fade out
        yield return Fade(1f, 0f);

        // Hide images 
        destinationImage.enabled = false;
    }


    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    [System.Serializable]
    public class SceneImageMapping
    {
        public string sceneName;
        public Sprite sprite;
    }
}
