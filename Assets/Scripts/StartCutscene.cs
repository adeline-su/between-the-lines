using UnityEngine;
using System.Collections;

public class StartCutscene : MonoBehaviour
{
    public GameObject player;
    // public GameObject instructionText;
    public float cutsceneLength;

    void Start()
    {
        // Turn off player control
        player.SetActive(false);

        // Turn on cutscene camera
        gameObject.SetActive(true);

        // Show instructions
        // instructionText.SetActive(true);

        StartCoroutine(EndCutscene());
    }

    IEnumerator EndCutscene()
    {
        yield return new WaitForSeconds(cutsceneLength);

        // Hide instructions
        // instructionText.SetActive(false);

        // Enable player
        player.SetActive(true);

        // Turn off cutscene camera
        gameObject.SetActive(false);
    }
}
