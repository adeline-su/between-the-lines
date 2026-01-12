using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class Teleporter : Interactable
{
    public string targetScene;      // name of the scene to load
    public Transform targetLocation; // position to teleport to in the new scene
    

    public override void Interact()
    {
        // don't let the players interact with Teleporters until they have had
        // their first conversation with the MC
        if (!GameManager.Instance.hasMetMC) return;
        base.Interact();
        Debug.Log("Interact - Teleporter");

        // destroy whatever item the player is holding
        GameManager.Instance.clearCurrentlyHeldItem();

        // change location within the current scene, to desired position in the target scene
        if (targetLocation != null) { 
            Debug.Log("Teleporting!");
            GameObject player = GameObject.FindWithTag("Player");
            GameManager.Instance.nextScenePosition = targetLocation.position;

            // CharacterController cc = player.GetComponent<CharacterController>();
            
            // cc.enabled = false;
            // player.transform.position = targetLocation.position;
            // player.transform.rotation = Quaternion.LookRotation(Vector3.forward); // north
            // cc.enabled = true;

            // Debug.Log("Teleported to " + player.transform.position);
            
        }

        // // change scene using fade transition
        if (!string.IsNullOrEmpty(targetScene))
        { 
            // call the singleton ScreenTransition
            if (ScreenTransition.Instance != null) {
                ScreenTransition.Instance.FadeToScene(targetScene);
            } else { 
                SceneManager.LoadScene(targetScene);
            }
        }
        // SceneManager.LoadScene(targetScene);

    }
}
