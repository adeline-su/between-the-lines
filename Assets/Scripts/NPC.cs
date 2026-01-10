using UnityEngine;
using Cinemachine;
using DialogueEditor;
using UnityEngine.InputSystem;

public class NPC : Interactable
{
    public string Name;
    public CinemachineVirtualCamera dialogueCamera;
    public NPCConversation firstConversation;
    public NPCConversation secondConversation;

    private PlayerInput playerInput;
    // private bool firstMeeting = true;
    private int cameraPriorityHigh = 20;
    private int cameraPriorityLow = 5;
    private int layerDefault;
    private int layerInteractable;

    public AudioSource speakingAudio;

    void Awake() {
        layerDefault = LayerMask.NameToLayer("Default");
        layerInteractable = LayerMask.NameToLayer("Interactable");

        // Find the PlayerInput component once the scene starts
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (speakingAudio != null)
            speakingAudio.Stop();
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interact - this NPC's name is " + Name);

        if (GameManager.Instance.isFirstTimeMeeting(gameObject)) {
            StartConversation(firstConversation);
        } else {
            StartConversation(secondConversation);
        }
    }
    
    private void StartConversation(NPCConversation conversation) {
        Debug.Log("Starting conversation with " + Name);

        // disable WASD input
        playerInput.enabled = false;

        // switch to dialogueCamera
        dialogueCamera.Priority = cameraPriorityHigh;
        Debug.Log("Set active for dialogueCamera " + dialogueCamera);
        
        // make the NPC non-interactable so that 'E' button will not trigger a new conversation
        gameObject.layer = layerDefault;

        // play talking sound
        if (speakingAudio != null) {
            Debug.Log("Playing audio for " + Name);
            Debug.Log("AudioSource clip: " + (speakingAudio.clip != null ? speakingAudio.clip.name : "NULL"));
            Debug.Log("AudioSource volume: " + speakingAudio.volume);
            Debug.Log("AudioSource mute: " + speakingAudio.mute);
            Debug.Log("AudioSource spatialBlend (0=2D, 1=3D): " + speakingAudio.spatialBlend);
            Debug.Log("AudioSource enabled: " + speakingAudio.enabled);
            Debug.Log("AudioSource gameObject active: " + speakingAudio.gameObject.activeInHierarchy);

            // Check for Audio Listeners
            AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
            Debug.Log("Number of active Audio Listeners: " + listeners.Length);
            if (listeners.Length > 0) {
                Debug.Log("Audio Listener enabled: " + listeners[0].enabled);
            }

            // Check Unity's global audio volume
            Debug.Log("AudioListener.volume: " + AudioListener.volume);

            speakingAudio.Play();
            Debug.Log("AudioSource isPlaying after Play(): " + speakingAudio.isPlaying);
            Debug.Log("AudioSource time: " + speakingAudio.time);
        } else {
            Debug.LogWarning("speakingAudio is null for " + Name + "! Assign an AudioSource in the Inspector.");
        }

        ConversationManager.Instance.StartConversation(conversation);
        ConversationManager.OnConversationEnded += ConversationEndedHandler;
    }
    private void ConversationEndedHandler() {
        Debug.Log("Conversation with " + Name + " has ended.");

        speakingAudio.Stop();

        // enable WASD input
        playerInput.enabled = true;

        // switch to first person camera
        dialogueCamera.Priority = cameraPriorityLow;

        // make the NPC interactable again
        gameObject.layer = layerInteractable;

        if (GameManager.Instance.isFirstTimeMeeting(gameObject)) {
            GameManager.Instance.InspirationLevel += 1;
            // firstMeeting = false;
            GameManager.Instance.addToListOfPastConversations(gameObject);
        }
        Debug.Log("Updated the InspirationLevel to " + GameManager.Instance.InspirationLevel);

        ConversationManager.OnConversationEnded -= ConversationEndedHandler;
    }
}
