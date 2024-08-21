using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public string areaName; // Identifier for the area
    public Objective areaObjective; // Objective associated with this area
    public Objective prerequisiteObjective; // Prerequisite objective required before this one can be set

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectivesManager.Instance.NotifyAreaEntered(areaName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectivesManager.Instance.NotifyAreaExited(areaName);
        }
    }
}
