using UnityEngine;

public class GrabbableItem : Interactable
{
    public Vector3 holdOffset = Vector3.zero; // Optional offset when held
    public Vector3 holdRotation = Vector3.zero; // Optional rotation when held

    private bool isGrabbed = false;
    private Transform holdPoint;
    private Rigidbody rb;
    private Collider col;

    // Store original physics settings
    private bool originalUseGravity;
    private float originalDrag;
    private float originalAngularDrag;
    private RigidbodyConstraints originalConstraints;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        Debug.Log("grabbable item: rigidbody ", rb);
        Debug.Log("grabbable item: collider ", col);

        // Store original physics settings
        if (rb != null)
        {
            originalUseGravity = rb.useGravity;
            originalDrag = rb.linearDamping;
            originalAngularDrag = rb.angularDamping;
            originalConstraints = rb.constraints;
        }
    }

    public override void Interact()
    {
        base.Interact();

        // Find the player's hold point
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("GrabbableItem: Could not find Player!");
            return;
        }

        // Debug: Log all children of the player
        Debug.Log($"Player has {player.transform.childCount} children:");
        foreach (Transform child in player.transform)
        {
            Debug.Log($"  - {child.name}");
        }

        Transform playerHoldPoint = player.transform.Find("HoldPoint");
        if (playerHoldPoint == null)
        {
            Debug.LogWarning("GrabbableItem: Player does not have a HoldPoint child object!");
            return;
        }

        if (!isGrabbed)
        {
            Grab(playerHoldPoint);
        }
        else
        {
            Release();
        }
    }

    private void Grab(Transform newHoldPoint)
    {
        Debug.Log("Grabbed " + gameObject.name);
        isGrabbed = true;
        holdPoint = newHoldPoint;

        // Parent to hold point
        transform.SetParent(holdPoint);

        // Apply hold offset and rotation
        if (holdOffset != null) transform.localPosition = holdOffset;
        if (holdRotation != null) transform.localEulerAngles = holdRotation;

        // Disable physics while held
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Optionally disable collision while held
        if (col != null)
        {
            col.enabled = false;
        }

        GameManager.Instance.currentlyHeldItem = gameObject;
        Debug.Log($"Grabbed {gameObject.name}, the currently held item is {GameManager.Instance.currentlyHeldItem}");
    }

    public void Release()
    {
        
        isGrabbed = false;

        // Unparent from hold point
        transform.SetParent(null);

        // Restore physics
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = originalUseGravity;
            rb.linearDamping = originalDrag;
            rb.angularDamping = originalAngularDrag;
            rb.constraints = originalConstraints;
        }

        // Re-enable collision
        if (col != null)
        {
            col.enabled = true;
        }

        Debug.Log($"Released {gameObject.name}");
    }

    // public bool IsGrabbed()
    // {
    //     return isGrabbed;
    // }
}
