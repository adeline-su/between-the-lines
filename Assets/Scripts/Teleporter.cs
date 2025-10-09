using UnityEngine;
using UnityEngine.SceneManagement; 

public class Teleporter : Interactable
{
    public string targetScene;      // name of the scene to load
    public Transform targetLocation; // position to teleport to (TODO: remove me later, this is just for proof of concept) 
    

    public override void Interact()
    {
        base.Interact();

        if (!string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
        else if (targetLocation != null)
        {
            Debug.Log("Teleporting!");
            GameObject player = GameObject.FindWithTag("Player");
            CharacterController cc = player.GetComponent<CharacterController>();
            
            cc.enabled = false;
            player.transform.position = targetLocation.position;
            cc.enabled = true;

            Debug.Log("Teleported to " + player.transform.position);
        }
    }
}
