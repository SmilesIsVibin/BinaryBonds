using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    public Animator animator;

    public void OpenTransition(){
        animator.SetTrigger("Open");
    }

    public void CloseTransition(){
        animator.SetTrigger("Close");
    }
}
