using UnityEngine;

public class BoxInteractionTrigger : MonoBehaviour
{
    public BoxInteraction boxInteraction; // Reference to the BoxInteraction script
    private RobotController playerController; // Reference to the player's RobotController script

    public float interactionRange = 3f; // Range within which the player can interact with the box
    public LayerMask playerLayer; // Layer mask to ensure only the player triggers interaction

    private bool isPlayerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if ((playerLayer & (1 << other.gameObject.layer)) != 0)
        {
            playerController = other.GetComponent<RobotController>();
            if (playerController != null)
            {
                isPlayerInRange = true;
                ShowInteractionPrompt(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((playerLayer & (1 << other.gameObject.layer)) != 0)
        {
            isPlayerInRange = false;
            ShowInteractionPrompt(false);
            if (playerController != null)
            {
                playerController.EndInteraction();
            }
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerController != null)
            {
                if (playerController.isInteractingWithBox)
                {
                    EndInteraction();
                }
                else
                {
                    BeginInteraction();
                }
            }
        }
    }

    private void BeginInteraction()
    {
        playerController.BeginInteraction(transform, boxInteraction);
        boxInteraction.BeginInteraction(playerController);
    }

    private void EndInteraction()
    {
        playerController.EndInteraction();
        boxInteraction.EndInteraction();
    }

    private void ShowInteractionPrompt(bool show)
    {
        // Implement the UI prompt logic here
        // e.g., Set active/inactive, show/hide prompt
    }
}
