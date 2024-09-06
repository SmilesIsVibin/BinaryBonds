using UnityEngine;

public class ObjectiveActivation : MonoBehaviour
{
    public string activationObjectName; // Set this to match the activationObjectName in the Objective
    public GameObject interactionPrompt;

    private bool isActivated = false;

    private void Start(){
        interactionPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player") && !isActivated)
        {
                        interactionPrompt.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E)){
            ActivateObjective();
            }
        }
    }
    
    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void ActivateObjective(){
        isActivated = true;
        ObjectivesManager.Instance.NotifyActivationTriggered(activationObjectName);
        Destroy(this);
    }
}
