using UnityEngine;

[System.Serializable]
public class Objective
{
    public enum ObjectiveType
    {
        Collect,
        Activation,
        Explore,
        Timed
    }

    public ObjectiveType type;
    public string title;
    public string description;
    public bool isCompleted = false;
    public int nextObjectiveIndex = -1;

    // For Collect objectives
    public string itemName;
    public int requiredQuantity;
    public int currentQuantity;
    // For Activation objectives
    public string objectActivatedName;

    // For Explore objectives
    public string areaName;
    public Transform markerLocation;

    // For Timed objectives
    public float timeLimit;
    [HideInInspector] public float timeRemaining;

    public string GetObjectiveStatus()
    {
        switch (type)
        {
            case ObjectiveType.Collect:
                return isCompleted ? "Objective Completed" : $"Collected {currentQuantity}/{requiredQuantity}";
            case ObjectiveType.Explore:
                return isCompleted ? "Area Explored" : $"Go to {areaName}";
            case ObjectiveType.Activation:
                return isCompleted ? "{Objective Completed}" : $"{objectActivatedName} activated";
            case ObjectiveType.Timed:
                return isCompleted ? "Timed Objective Completed" : $"Time Left: {timeRemaining:F2} seconds";
            default:
                return "";
        }
    }

    public void ProgressObjective(int quantity = 1)
    {
        switch (type)
        {
            case ObjectiveType.Collect:
                currentQuantity += quantity;
                if (currentQuantity >= requiredQuantity)
                {
                    currentQuantity = requiredQuantity;
                    isCompleted = true;
                }
                break;
            case ObjectiveType.Explore:
                isCompleted = true;
                break;
            case ObjectiveType.Activation:
                isCompleted = true;
                break;
            case ObjectiveType.Timed:
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0f)
                {
                    isCompleted = true;
                }
                break;
            default:
                Debug.Log("Objective error");
                break;
        }
    }

    public void StartTimer()
    {
        if (type == ObjectiveType.Timed)
        {
            timeRemaining = timeLimit;
        }
    }
}
