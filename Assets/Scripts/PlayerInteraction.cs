using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    public float interactDistance = 3f;
    public LayerMask interactableLayer;
    private Transform playerCamera;

    private Interactable currentHighlighted; // Track the currently highlighted object

    void Start() {
        playerCamera = Camera.main.transform;
    }

    void Update() {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Perform raycast only on interactable layer
        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer)) {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
                if (currentHighlighted != interactable) {
                    if (currentHighlighted != null)
                        currentHighlighted.Highlight(false);

                    currentHighlighted = interactable;
                    currentHighlighted.Highlight(true);
                }

                // Interact when pressing E
                if (Input.GetKeyDown(KeyCode.E)) {
                    currentHighlighted.Interact();
                }
            }
            else {
                ClearHighlight(); // Hit something on the layer, but it's not interactable
            }
        }
        else {
            ClearHighlight(); // Hit nothing
        }
    }

    private void ClearHighlight() {
        if (currentHighlighted != null) {
            currentHighlighted.Highlight(false);
            currentHighlighted = null;
        }
    }
}
