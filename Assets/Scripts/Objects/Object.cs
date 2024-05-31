using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public GameObject objectHome;
    private bool isTriggered;
    public string tag1, tag2;

    private void Update()
    {
        if (isTriggered)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                objectHome.SetActive(false);
                this.enabled = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag1) || other.CompareTag(tag2)) 
        {
            isTriggered = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tag1) || other.CompareTag(tag2))
        {
            isTriggered = false;
        }
    }
}
