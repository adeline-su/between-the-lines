using UnityEngine;

public class CanvasStart : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance.hasVisitedAtLeastOneLocation) {
            gameObject.SetActive(false);
        }
    }
}
