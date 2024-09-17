using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lever : MonoBehaviour
{
    public bool isOn = false; // Current state of this lever
    public LeverManager leverManager; // Reference to the Lever Manager
    public TMP_Text interactionPrompt; // Interaction UI prompt
    public Lever[] adjacentLevers; // Adjacent levers that will be affected
    public GameObject handle;

    private bool playerInRange = false;

    private void Start(){
        interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLever();
        }
    }

    private void ToggleLever()
    {
        isOn = !isOn;
        SwitchHandle();
        if(adjacentLevers.Length >0)
        {
            foreach (Lever l in adjacentLevers)
            {
                l.ToggleState();
            }
        }
        // Notify the manager that this lever was toggled
        leverManager.LeverToggled();

        // Hide the interaction prompt briefly after interacting
        interactionPrompt.gameObject.SetActive(false);
        Invoke(nameof(ShowPromptIfStillInRange), 3f);
    }

    public void SwitchHandle()
    {
        if (isOn)
        {
            handle.transform.Rotate(90, 0, 0);
        }
        else
        {
            handle.transform.Rotate(-90, 0, 0);
        }
    }

    public void ToggleState()
    {
        isOn = !isOn;
        SwitchHandle();
        leverManager.LeverToggled();
    }

    private void ShowPromptIfStillInRange()
    {
        if (playerInRange)
        {
            interactionPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player_Elara") || other.CompareTag("Player_Happy"))
        {
            playerInRange = true;
            interactionPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player_Elara") || other.CompareTag("Player_Happy"))
        {
            playerInRange = false;
            interactionPrompt.gameObject.SetActive(false);
        }
    }
}
