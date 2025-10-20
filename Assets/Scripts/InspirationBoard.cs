using UnityEngine;
using TMPro;

public class InspirationBoard : MonoBehaviour
{
    public TMP_Text inspirationText;
    public TMP_Text curiosityLevelText;
    public TMP_Text sympathylevelText;
    public TMP_Text cynicismLevelText;

    void Start()
    {
        // inspirationText = GetComponentInChildren<TMP_Text>(); // Get TMP component from child
        UpdateText();
    }

    public void UpdateText()
    {
        inspirationText.text = GameManager.Instance.InspirationLevel.ToString();

        curiosityLevelText.text = GameManager.Instance.getCuriosityLevel().ToString();
        sympathylevelText.text = GameManager.Instance.getSympathyLevel().ToString();
        cynicismLevelText.text = GameManager.Instance.getCynicismLevel().ToString();
    }
}
