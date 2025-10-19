using UnityEngine;
using Cinemachine;

public class PlayerInteraction : MonoBehaviour {
    public float interactDistance = 3f;
    public LayerMask interactableLayer;
    public CinemachineVirtualCamera playerCam;

    private Transform playerCamera;
    private Interactable currentHighlighted; // Track the currently highlighted object

    void Awake() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Players " + players.Length);
        if (players.Length > 1) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // keep Player GameObject between scenes
                                        // TODO (maybe): move this to a different Player script to seperate concerns with interactions
    }
    
    void Start() {
        Debug.Log(playerCam.Priority);
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
