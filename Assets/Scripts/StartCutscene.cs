using UnityEngine;
using System.Collections;

public class StartCutscene : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public GameObject journalFirst;   // first journal UI
    public GameObject journalSecond;   // second journal UI

    [Header("Timing (seconds)")]
    public float delayBeforeFirstJournal;
    public float firstJournalDuration;
    public float secondJournalDuration;
    public float timeSecondJournalAppears;

    private bool TESTING_skipStartCutscene = true;

    void Start()
    {
        if (TESTING_skipStartCutscene) {
            delayBeforeFirstJournal = 0.5f;
            firstJournalDuration = 0.5f;
            secondJournalDuration = 0.5f;
            timeSecondJournalAppears = 2f;
        }
        // Always ensure journals are hidden initially
        if (journalFirst) journalFirst.SetActive(false);
        if (journalSecond) journalSecond.SetActive(false);

        // Check if this is the first time playing
        if ((GameManager.Instance != null 
            && GameManager.Instance.InspirationLevel > 0 )
            || GameManager.Instance.hasVisitedAtLeastOneLocation
        ) {
            // Not first time - skip cutscene, make sure player is active
            if (player) player.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        // First time - play the cutscene
        if (player) player.SetActive(false);
        gameObject.SetActive(true);

        StartCoroutine(RunCutscene());
    }

    IEnumerator RunCutscene()
    {
        // if (TESTING_skipStartCutscene) yield break;

        // wait, then show first journal
        yield return new WaitForSeconds(delayBeforeFirstJournal);
        if (journalFirst) journalFirst.SetActive(true);

        yield return new WaitForSeconds(firstJournalDuration);
        if (journalFirst) journalFirst.SetActive(false);

        // wait until it's time for the second journal
        float elapsedSoFar = delayBeforeFirstJournal + firstJournalDuration;
        float waitUntilSecond = Mathf.Max(0f, timeSecondJournalAppears - elapsedSoFar);
        if (waitUntilSecond > 0f) yield return new WaitForSeconds(waitUntilSecond);

        // show second journal
        if (journalSecond) journalSecond.SetActive(true);

        yield return new WaitForSeconds(secondJournalDuration);
        if (journalSecond) journalSecond.SetActive(false);

        // end cutscene
        if (player) player.SetActive(true);
        gameObject.SetActive(false);
    }
}
