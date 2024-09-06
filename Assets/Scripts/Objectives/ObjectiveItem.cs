using UnityEngine;

public class ObjectiveItem : MonoBehaviour
{
    public string itemName;
    public int quantity = 1;
    public GameObject interactionPopup;

    private void Start()
    {
        interactionPopup.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPopup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPopup.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) // Assuming "E" is the interact key
        {
            ObjectivesManager.Instance.NotifyItemCollected(itemName, quantity);
            Destroy(gameObject);
        }
    }
}
