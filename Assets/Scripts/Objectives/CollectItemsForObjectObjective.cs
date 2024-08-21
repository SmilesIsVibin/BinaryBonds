using UnityEngine;

[CreateAssetMenu(fileName = "CollectItemsForObjectObjective", menuName = "Objectives/Collect Items For Object")]
public class CollectItemsForObjectObjective : Objective
{
    public string objectToPower;
    public string itemRequired;
    public int quantityRequired;

    public override void OnObjectiveComplete()
    {
        // Logic for completing the objective related to powering an object
        ObjectivesManager.Instance.CompleteObjective();
    }
}
