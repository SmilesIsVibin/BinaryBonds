using UnityEngine;

[CreateAssetMenu(fileName = "CollectItemObjective", menuName = "Objectives/Collect Item")]
public class CollectItemObjective : Objective
{
    public string itemRequired;
    public int quantityRequired;

    public override void OnObjectiveComplete()
    {
        // Specific logic when this objective is completed
        ObjectivesManager.Instance.CompleteObjective();
    }
}

