using UnityEngine;
using Cinemachine;

public class PlayerInteraction : MonoBehaviour {
    public static PlayerInteraction Instance;

    public float interactDistance = 3f;
    public LayerMask interactableLayer;
    public CinemachineVirtualCamera playerCam;

    private Transform playerCamera;
    public Interactable currentHighlighted;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        playerCamera = Camera.main.transform;
    }

    void Update() {
        bool hitInteractable = false;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer)) {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null) {
                hitInteractable = true;

                if (currentHighlighted != interactable) {
                    if (currentHighlighted != null)
                        currentHighlighted.Highlight(false);

                    currentHighlighted = interactable;
                    currentHighlighted.Highlight(true);
                }

                if (Input.GetKeyDown(KeyCode.E)) {
                    currentHighlighted.Interact();
                }
            }
        }

        // If no interactable detected
        if (!hitInteractable) {
            ClearHighlight();

            if (Input.GetKeyDown(KeyCode.E)) {
                OnEmptyInteract();   // Call whatever you want here
            }
        }
    }

    private void ClearHighlight() {
        if (currentHighlighted != null) {
            currentHighlighted.Highlight(false);
            currentHighlighted = null;
        }
    }

    private void OnEmptyInteract() {
        Debug.Log("Pressed E with no interactable!");
        GrabbableItem item = GameManager.Instance.currentlyHeldItem.GetComponent<GrabbableItem>();
        
        if (currentHighlighted is Interactable) {
            Debug.Log("Interact will not release " + gameObject.name + 
                " because player is looking at another interactable object, " + PlayerInteraction.Instance.currentHighlighted.name);
        } else if (item == null) {
            Debug.Log("Trying to release a non-grabbable item " + GameManager.Instance.currentlyHeldItem.name);
        } else {
            Debug.Log("Releasing " + gameObject.name);
            GameManager.Instance.currentlyHeldItem = null;
            item.Release();
        }
    }
}
