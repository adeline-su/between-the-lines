using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Renderer rend; // the GameObject’s Renderer component (responsible for drawing it).
    private SkinnedMeshRenderer skinnedRend;

    private uint renderingLayerMask_Default = 1u << 0;
    private uint renderingLayerMask_LightLayer2 = 1u << 2;

    protected virtual void Start()
    {
        rend = GetComponent<Renderer>(); // finds the GameObject’s Renderer
        skinnedRend = GetComponentInChildren<SkinnedMeshRenderer>();

        Debug.Log("start: interactable " + gameObject.name);
        Debug.Log("rend " + rend);
        Debug.Log("skinnedRend " + skinnedRend);
    }

    public void Highlight(bool isActive)
    {
        if ((gameObject.name != "MC") && !GameManager.Instance.hasMetMC) return;
        // for objects with a Renderer on this GameObject
        Debug.Log("trying to Highlight " + gameObject.name);
        if (rend != null) {
            Debug.Log("Interactable - Highlight for OBJECTS " + gameObject.name);

            if (isActive) {
                Debug.Log("Highlighted " + gameObject.name);
                rend.renderingLayerMask = renderingLayerMask_Default | renderingLayerMask_LightLayer2;
            }
            else {
                rend.renderingLayerMask = renderingLayerMask_Default;
            }
        }
        // for characters with SkinnedMeshRenderer on a child
        else if (skinnedRend != null) {
            Debug.Log("Interactable - Highlight for CHARACTERS " + gameObject.name);
            if (isActive) {
                Debug.Log("Highlighted " + gameObject.name);
                skinnedRend.renderingLayerMask = renderingLayerMask_Default | renderingLayerMask_LightLayer2;
            }
            else {
                skinnedRend.renderingLayerMask = renderingLayerMask_Default;
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

}
