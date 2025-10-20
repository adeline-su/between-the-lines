using UnityEngine;

public class ChoiceHandler : MonoBehaviour
{

    // private GameManager gameManager;

    void Awake() {
        // gameManager = FindFirstObjectByType<GameManager>();

        Debug.Log("ChoiceHandler awake.");
        GameManager.Instance.DebugGameState();

    }
    public void OnChoseCuriosity()
    {
        Debug.Log("Player chose Curiosity");
        GameManager.Instance.IncCuriosity();
    }

    public void OnChoseSympathy()
    {
        Debug.Log("Player chose Sympathy");
        GameManager.Instance.IncSympathy();
    }

    public void OnChoseCynicism()
    {
        Debug.Log("Player chose Cynicism");
        GameManager.Instance.IncCynicism();
    }
}
