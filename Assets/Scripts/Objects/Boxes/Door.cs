using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public GameObject interactionPrompt;
    public Animator doorAnimator;
    public int doorState;
    private bool canOpen;

    void Start(){
        interactionPrompt.SetActive(false);
    }

    void Update(){
        if(canOpen && Input.GetKeyDown(KeyCode.E)){
                AnimateDoor();
                if(doorState == 0){
                    doorState = 1;
                }else{
                    doorState = 0;
                }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            interactionPrompt.SetActive(true);
            canOpen = true;
        }
    }

    private void OnTriggerStay(Collider other){
        if(other.CompareTag("Player")){
            interactionPrompt.SetActive(true);
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            interactionPrompt.SetActive(false);
            canOpen = false;
        }
    }

    private void AnimateDoor(){
        switch(doorState){
            case 0:
                doorAnimator.SetTrigger("Swing");
                break;
            case 1:
                doorAnimator.SetTrigger("Close");
                break;
            default:
                doorAnimator.SetTrigger("Swing");
                break;
        }
    }
}
