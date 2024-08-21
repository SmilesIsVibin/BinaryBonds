using UnityEngine;
[CreateAssetMenu(fileName = "GoToAreaObjective", menuName = "Objectives/Go To Area")]
public class GoToAreaObjective : Objective
{
    public string areaName;

    public override void OnObjectiveComplete()
    {
        // Logic for completing the objective related to reaching a specific area
        ObjectivesManager.Instance.CompleteObjective();
    }
}
