using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int CuriosityLevel = 0;
    private int SympathyLevel = 0;
    private int CynicismLevel = 0;
    private bool hasTriggeredEndCutscene = false;
    public Vector3 nextScenePosition = Vector3.zero;
    private TwoJournalCutscene endCutscene;

    // public fields
    List<string> pastConversations = new List<string>();
    public int InspirationLevel;
    public bool hasVisitedAtLeastOneLocation = false;
    public bool hasMetMC = false;
    public GameObject currentlyHeldItem;
    public string verdictText;

    private void Awake() {
        if (Instance != null) { // make sure there is only one GameManager at all times
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InspirationLevel = 0;
        DontDestroyOnLoad(gameObject);

        endCutscene = FindObjectOfType<TwoJournalCutscene>(true);
        endCutscene.gameObject.SetActive(false);

        // play beginning sequence 
        if (!hasVisitedAtLeastOneLocation) {
            StartCoroutine(FadeFromBlack());
        }
    }

    public void OnSceneLoaded(string sceneName) {
        Debug.Log("game manager - OnSceneLoaded");

        // disable the end cutscene
        endCutscene = FindObjectOfType<TwoJournalCutscene>(true);
        endCutscene.gameObject.SetActive(false);

        bool finishedGame = (CuriosityLevel + SympathyLevel + CynicismLevel) == 3;

        // TODO: uncomment this out later
        if (sceneName == "BedroomScene" && finishedGame && !hasTriggeredEndCutscene) {
        // if (sceneName == "BedroomScene" && CuriosityLevel == 1 && !hasTriggeredEndCutscene) {
        // if (sceneName == "BedroomScene") {
            Debug.Log("Returned to bedroom, creating the final verdict triggering end cutscene");

            // create the final verdict
            string verdict = "";
            if ((CuriosityLevel == SympathyLevel) && (SympathyLevel == CynicismLevel) && (CynicismLevel == 1)) {
                verdict = "curious, sympathetic and cynical.";
            } else if (CuriosityLevel >= 1 && SympathyLevel >= 1) {
                verdict = "curious and sympathetic.";
            } else if (CuriosityLevel >= 1 && CynicismLevel >= 1) {
                verdict = "curious and cynical.";
            } else if (SympathyLevel >= 1 && CynicismLevel >= 1) {
                verdict = "sympathetic and cynical.";
            } else if (SympathyLevel >= 1) {
                verdict = "sympathetic.";
            } else if (CuriosityLevel >= 1) {
                verdict = "curious.";
            } else if (CynicismLevel >= 1) {
                verdict = "cynical.";
            }
            Debug.Log("Verdict: " + verdict);
            verdictText = verdict; 

            // trigger end cutscene
            hasTriggeredEndCutscene = true;
            StartCoroutine(TriggerEndCutsceneDelayed());

            // show end screen after 20 seconds
            StartCoroutine(DelayedEndSequence());
        } else {

        }
    }

    private System.Collections.IEnumerator DelayedEndSequence() {
        yield return new WaitForSeconds(19);
        StartCoroutine(FadeToBlack());
    }


    private System.Collections.IEnumerator FadeToBlack() {
        AudioListener.volume = 0f;

        CanvasGroup endCanvas = GameObject.Find("CanvasEnd").GetComponent<CanvasGroup>();
        if (endCanvas == null) {
            Debug.Log("endCanvas is null");
            yield return null;
        }
        yield return Fade(0f, 1f, endCanvas);
    }

    private System.Collections.IEnumerator FadeFromBlack() {
        yield return new WaitForSeconds(1.5f);

        CanvasGroup startCanvas = GameObject.Find("CanvasStart").GetComponent<CanvasGroup>();
        if (startCanvas == null) {
            Debug.Log("startCanvas is null");
            yield return null;
        }
        yield return Fade(1f, 0f, startCanvas);

        // GameObject startCanvas = GameObject.Find("CanvasStart");
        // startCanvas.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator Fade(float start, float end, CanvasGroup endCanvas)
    {
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            endCanvas.alpha = Mathf.Lerp(start, end, elapsed / 0.5f);
            yield return null;
        }
        endCanvas.alpha = end;
    }

    System.Collections.IEnumerator TriggerEndCutsceneDelayed() {
        yield return null; // wait a frame 

        // TwoJournalCutscene endCutscene = FindObjectOfType<TwoJournalCutscene>(true);
        Debug.Log("TriggerEndCutsceneDelayed, endCutscene is " + endCutscene);
        if (endCutscene != null) {
            endCutscene.gameObject.SetActive(true);
            endCutscene.Play();
        } else {
            Debug.LogWarning("EndCutscene not found in BedroomScene!");
        }
    }

    public void IncCuriosity() {
        CuriosityLevel += 1;
        Debug.Log("CuriosityLevel " + CuriosityLevel);
    }

    public void IncSympathy() {
        SympathyLevel += 1;
        Debug.Log("SympathyLevel " + SympathyLevel);
    }

    public void IncCynicism() {
        CynicismLevel += 1;
        Debug.Log("CynicismLevel " + CynicismLevel);
    }

    public int getCuriosityLevel() {
        return CuriosityLevel;
    }

    public int getSympathyLevel() {
        return SympathyLevel;
    }

    public int getCynicismLevel() {
        return CynicismLevel;
    }

    public void DebugGameState() {
        Debug.Log("(1/2) GameManager says that the InspirationLevel is " + Instance?.InspirationLevel
            + " and (Curiosity, Sympathy, Cynicism) is " + CuriosityLevel + " " + SympathyLevel + " " + CynicismLevel);

        Debug.Log("(2/2) GameManager says that the list of pastConversations is " + string.Join(", ", pastConversations));
    }

    public bool isFirstTimeMeeting(GameObject npc) {
        Debug.Log("Checking if " + npc.name + " is a member of " + string.Join(", ", pastConversations));
        if (pastConversations.Contains(npc.name)) {
            Debug.Log("False - not first time meeting " + npc.name);
            return false;
        } else {
            Debug.Log("True - first time meeting " + npc.name);
            return true;
        }
    }

    public void addToListOfPastConversations(GameObject npc) {
        pastConversations.Add(npc.name);
        Debug.Log("Added " + npc.name + " to list of pastConversations: " + string.Join(", ", pastConversations));
    }

    public void clearCurrentlyHeldItem() {
        Debug.Log("GameManager, clearCurrentlyHeldItem");
        if (currentlyHeldItem == null) return;

        Destroy(currentlyHeldItem.gameObject);
        currentlyHeldItem = null;
    }

}
