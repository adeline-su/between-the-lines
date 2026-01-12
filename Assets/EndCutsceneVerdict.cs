using UnityEngine;
using TMPro;

public class EndCutsceneVerdict : MonoBehaviour
{
    public TMP_Text verdict;

    void Start()
    {
        Debug.Log("verdict is being set to " + GameManager.Instance.verdictText);
        verdict.text = GameManager.Instance.verdictText;
    }
}
