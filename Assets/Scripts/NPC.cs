using UnityEngine;

public class NPC : Interactable
{
    public string Name;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interact - this NPC's name is " + Name);

        GameManager.Instance.InspirationLevel += 1;

        Debug.Log("Updated the InspirationLevel to " + GameManager.Instance.InspirationLevel);
    }
}
