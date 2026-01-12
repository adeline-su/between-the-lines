using UnityEngine;
using TMPro;

public class InspirationBoard : MonoBehaviour
{
    public TMP_Text inspirationText;
    public TMP_Text curiosityLevelText;
    public TMP_Text sympathylevelText;
    public TMP_Text cynicismLevelText;

    public GameObject stickiesLevel1;
    public GameObject stickiesLevel2;
    public GameObject stickiesLevel3;

    void Start()
    {
        // inspirationText = GetComponentInChildren<TMP_Text>(); // Get TMP component from child
        UpdateBoard();
    }

    public void UpdateBoard()
    {
        int inspirationLevel = GameManager.Instance.InspirationLevel;
        inspirationText.text = inspirationLevel.ToString();

        curiosityLevelText.text = GameManager.Instance.getCuriosityLevel().ToString();
        sympathylevelText.text = GameManager.Instance.getSympathyLevel().ToString();
        cynicismLevelText.text = GameManager.Instance.getCynicismLevel().ToString();

        if (inspirationLevel >= 1) stickiesLevel1.SetActive(true);
        if (inspirationLevel >= 2) stickiesLevel2.SetActive(true);
        if (inspirationLevel >= 3) stickiesLevel3.SetActive(true);

    }
}
