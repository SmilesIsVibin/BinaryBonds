using UnityEngine;

[CreateAssetMenu(fileName = "CollectItemsForDoorObjective", menuName = "Objectives/Collect Items For Door")]
public class CollectItemsForDoorObjective : Objective
{
    public string doorName;
    public string itemRequired;
    public int quantityRequired;

    public override void OnObjectiveComplete()
    {
        // Logic for completing the objective related to opening a door
        ObjectivesManager.Instance.CompleteObjective();
    }
}
