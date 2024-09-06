using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public string areaName;
    public bool unlocksObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectivesManager.Instance.NotifyAreaEntered(areaName, unlocksObj);
            Destroy(gameObject); // One-time use
        }
    }
}
