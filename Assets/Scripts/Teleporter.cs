using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class Teleporter : Interactable
{
    public string targetScene;      // name of the scene to load
    public Transform targetLocation; // position to teleport to in the new scene
    

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interact - Teleporter");

        if (targetLocation != null) { // change location within the current scene, to desired position in the target scene
            Debug.Log("Teleporting!");
            GameObject player = GameObject.FindWithTag("Player");
            CharacterController cc = player.GetComponent<CharacterController>();
            
            cc.enabled = false;
            player.transform.position = targetLocation.position;
            player.transform.rotation = Quaternion.LookRotation(Vector3.forward); // north
            cc.enabled = true;

            Debug.Log("Teleported to " + player.transform.position);
        }
        if (!string.IsNullOrEmpty(targetScene)) { // change scene
            SceneManager.LoadScene(targetScene);
        }
    }

}
