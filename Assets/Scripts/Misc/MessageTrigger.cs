using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    [SerializeField] private int messageIndex;
    [SerializeField] private string playerTag;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            MessageManager.Instance.TriggerMessage(messageIndex);
            Destroy(gameObject);
        }
    }
}
