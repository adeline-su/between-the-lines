using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Renderer rend; // the GameObject’s Renderer component (responsible for drawing it).

    void Start()
    {
        rend = GetComponent<Renderer>(); // finds the GameObject’s Renderer
    }

    public void Highlight(bool isActive)
    {
        if (isActive) {
            Debug.Log("Highlighted " + gameObject.name);
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", Color.yellow);
        }
        else {
            rend.material.SetColor("_EmissionColor", Color.black);
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

}
