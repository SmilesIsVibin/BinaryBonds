using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevelTrigger : MonoBehaviour
{
    public string tagName;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            GameManager.Instance.WinLevel();
        }
    }
}
