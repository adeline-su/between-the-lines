using UnityEngine;
using TMPro;

public class InspirationBoard : MonoBehaviour
{
    private TMP_Text inspirationText;

    void Start()
    {
        inspirationText = GetComponentInChildren<TMP_Text>(); // Get TMP component from child
        UpdateText();
    }

    public void UpdateText()
    {
        inspirationText.text = GameManager.Instance.InspirationLevel.ToString();
    }
}
