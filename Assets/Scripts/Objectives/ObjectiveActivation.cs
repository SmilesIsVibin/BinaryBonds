using System.Collections.Generic;
using UnityEngine;

public class ObjectiveActivation : MonoBehaviour
{
    public string activationObjectName; // Set this to match the activationObjectName in the Objective
    public GameObject interactionPrompt;
    public GameObject spawnedObject;

    public int requiredObjectiveCompleted;

    private bool isActivated = false;

    private bool canBeActivated;

    private void Start(){
        interactionPrompt.SetActive(false);
    }

    void Update(){
        if(canBeActivated && Input.GetKeyDown(KeyCode.E)){
            ActivateObjective();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            if (ObjectivesManager.Instance.allObjectives[requiredObjectiveCompleted].isCompleted){
                interactionPrompt.SetActive(true);
                canBeActivated = true;
                return;
            }
        }
    }

    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player") && !isActivated)
        {
            if (ObjectivesManager.Instance.allObjectives[requiredObjectiveCompleted].isCompleted){
                interactionPrompt.SetActive(true);
                canBeActivated = true;
                return;
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
        interactionPrompt.SetActive(false);
        if(spawnedObject != null){
            spawnedObject.SetActive(true);
        }
        ObjectivesManager.Instance.NotifyActivationTriggered(activationObjectName);
        Destroy(this);
    }
}
