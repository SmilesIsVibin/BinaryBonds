using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeycardReader : MonoBehaviour
{
    public GameObject interactionPrompt;
    public GameObject gate;
    public string requiredKeycard;
    public TMP_Text interactionText;
    public bool canActivate;

    void Start(){
        interactionPrompt.SetActive(false);
    }

    private void CheckForKeycard(){
        bool isCompleted = false;
        foreach(Objective obj in ObjectivesManager.Instance.allObjectives){
            if(obj.type == Objective.ObjectiveType.Collect && obj.itemName == requiredKeycard && obj.isCompleted){
                isCompleted = true;
            }
        }

        if(isCompleted == true){
            interactionText.text = "E";
            interactionPrompt.SetActive(true);
            canActivate = true;
        }else{
            interactionText.text = $"Requires {requiredKeycard} to open";
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            CheckForKeycard();
            if(canActivate && Input.GetKeyDown(KeyCode.E)){
                gate.SetActive(false);
                interactionPrompt.SetActive(false);
                this.enabled = false;
            }
        }
    }

    private void OnTriggerStay(Collider other){
        if(other.CompareTag("Player")){
            CheckForKeycard();
            if(canActivate && Input.GetKeyDown(KeyCode.E)){
                gate.SetActive(false);
                interactionPrompt.SetActive(false);
                Destroy(this);
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            interactionPrompt.SetActive(false);
            canActivate = false;
        }
    }
}
