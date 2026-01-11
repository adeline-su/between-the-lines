using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int InspirationLevel;

    private int CuriosityLevel = 0;
    private int SympathyLevel = 0;
    private int CynicismLevel = 0;

    List<string> pastConversations = new List<string>();

    private bool hasTriggeredEndCutscene = false;

    public bool hasVisitedAtLeastOneLocation = false;

    private void Awake() {
        if (Instance != null) { // make sure there is only one GameManager at all times
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InspirationLevel = 0;
        DontDestroyOnLoad(gameObject);
    }

    public void OnSceneLoaded(string sceneName) {
        Debug.Log("game manager - OnSceneLoaded");
        bool finishedGame = (CuriosityLevel + SympathyLevel + CynicismLevel) == 3;

        // TODO: uncomment this out later
        // if (scene.name == "BedroomScene" && finishedGame && !hasTriggeredEndCutscene) {
        if (sceneName == "BedroomScene" && CuriosityLevel == 1 && !hasTriggeredEndCutscene) {
            Debug.Log("Returned to bedroom, triggering end cutscene");
            hasTriggeredEndCutscene = true;
            StartCoroutine(TriggerEndCutsceneDelayed());
        }
    }

    System.Collections.IEnumerator TriggerEndCutsceneDelayed() {
        yield return null; // wait a frame 

        TwoJournalCutscene cutscene = FindObjectOfType<TwoJournalCutscene>(true);
        if (cutscene != null) {
            cutscene.gameObject.SetActive(true);
            cutscene.Play();
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

}
