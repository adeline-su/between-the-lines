using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Renderer rend; // the GameObject’s Renderer component (responsible for drawing it).
    private SkinnedMeshRenderer skinnedRend;

    private uint renderingLayerMask_Default = 1u << 0;
    private uint renderingLayerMask_LightLayer2 = 1u << 2;

    void Start()
    {
        rend = GetComponent<Renderer>(); // finds the GameObject’s Renderer
        skinnedRend = GetComponentInChildren<SkinnedMeshRenderer>();

        Debug.Log("rend " + rend);
        Debug.Log("skinnedRend " + skinnedRend);
    }

    public void Highlight(bool isActive)
    {
        // for characters
        if (skinnedRend != null) {
            Debug.Log("rend.renderingLayerMask " + skinnedRend.renderingLayerMask);
            if (isActive) {
                Debug.Log("Highlighted " + gameObject.name);
                // set Rendering Layer Mask to "Default" and "Light Layer 2" 
                skinnedRend.renderingLayerMask = renderingLayerMask_Default | renderingLayerMask_LightLayer2;
            }
            else {
                // reset Rendering Layer Mask to "Default" 
                skinnedRend.renderingLayerMask = renderingLayerMask_Default;
            }
        } 
        
        // for objects
        else {
            if (isActive) {
                Debug.Log("Highlighted " + gameObject.name);
                Debug.Log(rend);
                rend.renderingLayerMask = renderingLayerMask_Default | renderingLayerMask_LightLayer2;
            }
            else {
                rend.renderingLayerMask = renderingLayerMask_Default;
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

}
