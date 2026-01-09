using UnityEngine;
using System.Collections;

public class StartCutscene : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public GameObject journalDeskUI;   // first journal UI
    public GameObject journalRoomUI;   // second journal UI

    [Header("Timing (seconds)")]
    public float delayBeforeFirstJournal;
    public float firstJournalDuration;
    public float secondJournalDuration;
    public float timeSecondJournalAppears;

    void Start()
    {
        if (journalDeskUI) journalDeskUI.SetActive(false);
        if (journalRoomUI) journalRoomUI.SetActive(false);
        if (player) player.SetActive(false);

        gameObject.SetActive(true);

        StartCoroutine(RunCutscene());
    }

    IEnumerator RunCutscene()
    {
        // wait, then show first journal
        yield return new WaitForSeconds(delayBeforeFirstJournal);
        if (journalDeskUI) journalDeskUI.SetActive(true);

        yield return new WaitForSeconds(firstJournalDuration);
        if (journalDeskUI) journalDeskUI.SetActive(false);

        // wait until it's time for the second journal
        float elapsedSoFar = delayBeforeFirstJournal + firstJournalDuration;
        float waitUntilSecond = Mathf.Max(0f, timeSecondJournalAppears - elapsedSoFar);
        if (waitUntilSecond > 0f) yield return new WaitForSeconds(waitUntilSecond);

        // show second journal
        if (journalRoomUI) journalRoomUI.SetActive(true);

        yield return new WaitForSeconds(secondJournalDuration);
        if (journalRoomUI) journalRoomUI.SetActive(false);

        // end cutscene
        if (player) player.SetActive(true);
        gameObject.SetActive(false);
    }
}
