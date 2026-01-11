using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Renderer rend; // the GameObject’s Renderer component (responsible for drawing it).
    private SkinnedMeshRenderer skinnedRend;

    private uint renderingLayerMask_Default = 1u << 0;
    private uint renderingLayerMask_LightLayer2 = 1u << 2;
    private uint renderingLayerMask_LightLayer3 = 1u << 3;
    private uint renderingLayerMask_Highlight;

    protected virtual void Start()
    {
        rend = GetComponent<Renderer>(); // finds the GameObject’s Renderer
        skinnedRend = GetComponentInChildren<SkinnedMeshRenderer>();

        Debug.Log("start: interactable " + gameObject.name);
        Debug.Log("rend " + rend);
        Debug.Log("skinnedRend " + skinnedRend);
        
        if (this is GrabbableItem grabbableItem) { // grabbable items should be highlighted in green
            renderingLayerMask_Highlight = renderingLayerMask_LightLayer3;
        } else {
            renderingLayerMask_Highlight = renderingLayerMask_LightLayer2;
        }
    }

    public void Highlight(bool isActive)
    {
        if ((gameObject.name != "MC") && !GameManager.Instance.hasMetMC) return;

        // Apply highlight to this object first
        ApplyHighlightToRenderer(isActive, rend, skinnedRend);

        // Now apply highlight to all children
        var childRenderers = GetComponentsInChildren<Renderer>(includeInactive: false);
        var childSkinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: false);

        foreach (var r in childRenderers)
            ApplyHighlightToRenderer(isActive, r, null);

        foreach (var s in childSkinnedRenderers)
            ApplyHighlightToRenderer(isActive, null, s);
    }

    private void ApplyHighlightToRenderer(bool isActive, Renderer r, SkinnedMeshRenderer s)
    {
        if (r != null)
        {
            r.renderingLayerMask = isActive
                ? renderingLayerMask_Default | renderingLayerMask_Highlight
                : renderingLayerMask_Default;
        }
        else if (s != null)
        {
            s.renderingLayerMask = isActive
                ? renderingLayerMask_Default | renderingLayerMask_Highlight
                : renderingLayerMask_Default;
        }
    }



    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

}
