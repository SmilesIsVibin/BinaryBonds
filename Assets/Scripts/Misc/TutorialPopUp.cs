using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    // The UI panel or element where the message is displayed
    public GameObject popupPanel;
    
    // The UI Text component where the message will be shown
    public Text messageText;

    // List of tutorial messages
    public List<string> tutorialMessages;

    // Time delay between messages
    public float delayBetweenMessages = 2.0f;

    // Animation (e.g., fade in/out) associated with the popup
    public Animator popupAnimator;

    // Internal index to track current message
    private int currentMessageIndex = 0;

    private void Start()
    {
        // Ensure the popup is hidden initially
        popupPanel.SetActive(false);

        // Start the tutorial queue
        StartCoroutine(PlayTutorialMessages());
    }

    // Coroutine to display tutorial messages one after another
    IEnumerator PlayTutorialMessages()
    {
        // Loop through all messages
        while (currentMessageIndex < tutorialMessages.Count)
        {
            // Show the popup panel if it's not active
            popupPanel.SetActive(true);

            // Set the message text
            messageText.text = tutorialMessages[currentMessageIndex];

            // Play animation (assuming "FadeIn" is the name of the animation)
            if (popupAnimator != null)
            {
                popupAnimator.SetTrigger("FadeIn");
            }

            // Wait for the animation to finish before delay (or adjust as needed)
            yield return new WaitForSeconds(0.5f);

            // Wait for the delay between messages
            yield return new WaitForSeconds(delayBetweenMessages);

            // Play the fade-out animation
            if (popupAnimator != null)
            {
                popupAnimator.SetTrigger("FadeOut");
            }

            // Wait for the fade-out animation to complete (0.5 seconds or adjust based on your animation length)
            yield return new WaitForSeconds(0.5f);

            // Move to the next message
            currentMessageIndex++;
        }

        // After all messages are shown, hide the popup panel
        popupPanel.SetActive(false);
    }
}
