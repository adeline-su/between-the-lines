using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int InspirationLevel;

    private void Awake() {
        Debug.Log("GameManager says that the InspirationLevel is " + Instance?.InspirationLevel);


        if (Instance != null) { // make sure there is only one GameManager at all times
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InspirationLevel = 0;
        DontDestroyOnLoad(gameObject);
    }
}
