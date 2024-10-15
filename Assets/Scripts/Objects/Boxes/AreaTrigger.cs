using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public string areaName;
    public bool unlocksObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player_Elara") || other.CompareTag("Player_Happy"))
        {
            ObjectivesManager.Instance.NotifyAreaEntered(areaName, unlocksObj);
            Destroy(gameObject); // One-time use
        }
    }
}
