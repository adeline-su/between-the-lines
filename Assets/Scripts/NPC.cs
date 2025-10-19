using UnityEngine;
using Cinemachine;

public class NPC : Interactable
{
    public string Name;
    public CinemachineVirtualCamera dialogueCamera;

    private int cameraPriorityHigh = 20;
    private int cameraPriorityLow = 5;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interact - this NPC's name is " + Name);

        // dialogueCamera.enabled = true;
        dialogueCamera.Priority = cameraPriorityHigh;
        Debug.Log("Set active for dialogueCamera " + dialogueCamera);

        GameManager.Instance.InspirationLevel += 1;
        Debug.Log("Updated the InspirationLevel to " + GameManager.Instance.InspirationLevel);
    }
}
