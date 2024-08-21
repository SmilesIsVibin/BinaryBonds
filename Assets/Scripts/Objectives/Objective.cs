using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string title;
    public string description;
    public bool requiresAreaTransition;
    public Objective prerequisiteObjective; // For objectives that depend on previous ones
    public Objective nextObjective; // The next objective to transition to upon completion

    public abstract void OnObjectiveComplete();
}
