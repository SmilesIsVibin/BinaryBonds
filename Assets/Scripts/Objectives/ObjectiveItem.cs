using UnityEngine;

public class ObjectiveItem : MonoBehaviour
{
    public string itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify ObjectivesManager that an item has been collected
            ObjectivesManager.Instance.NotifyItemCollected(itemName);
            Destroy(gameObject); // Remove item from the scene
        }
    }
}
