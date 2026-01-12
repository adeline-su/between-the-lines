using UnityEngine;
using System.Collections;

public class TwoJournalCutscene : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    // public GameObject normalGameplayCamera;   // your main camera (optional, but recommended)
    // public GameObject cutsceneCamera;         // end cutscene camera object
    public Animator cutsceneAnimator;         // animator on cutscene camera (plays clip)
    public string cutsceneStateName = "EndCutscene"; // name of animator state/clip

    public GameObject journalFirst;
    public GameObject journalSecond;

    [Header("Timing")]
    public float delayBeforeFirstJournal = 1f;
    public float firstJournalDuration = 8f;
    public float timeSecondJournalAppears = 8f;
    public float secondJournalDuration = 17f;    

    bool isPlaying = false;

    public void Play()
    {
        if (isPlaying) return;
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        isPlaying = true;

        // Hide UI to start clean
        if (journalFirst) journalFirst.SetActive(false);
        if (journalSecond) journalSecond.SetActive(false);

        // Disable player
        if (player) player.SetActive(false);

        // Switch cameras
        // if (normalGameplayCamera) normalGameplayCamera.SetActive(false);
        gameObject.SetActive(true);

        // Restart the cutscene animation from the beginning
        if (cutsceneAnimator)
            cutsceneAnimator.Play(cutsceneStateName, 0, 0f);

        // Journal 1
        yield return new WaitForSeconds(delayBeforeFirstJournal);
        if (journalFirst) journalFirst.SetActive(true);

        yield return new WaitForSeconds(firstJournalDuration);
        if (journalFirst) journalFirst.SetActive(false);

        // Wait until time for Journal 2
        float elapsed = delayBeforeFirstJournal + firstJournalDuration;
        float wait = Mathf.Max(0f, timeSecondJournalAppears - elapsed);
        if (wait > 0f) yield return new WaitForSeconds(wait);

        // Journal 2
        if (journalSecond) journalSecond.SetActive(true);

        yield return new WaitForSeconds(secondJournalDuration);
        if (journalSecond) journalSecond.SetActive(false);

        // End cutscene: restore gameplay
        // if (cutsceneCamera) cutsceneCamera.SetActive(false);
        gameObject.SetActive(false);
        // if (normalGameplayCamera) normalGameplayCamera.SetActive(true);

        if (player) player.SetActive(true);

        isPlaying = false;
    }
}
