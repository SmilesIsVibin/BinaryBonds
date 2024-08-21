using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;
    public Objective currentObjective;
    public string currentArea; // Track the current area

    public TMP_Text objectiveTitleText;
    public TMP_Text objectiveDescriptionText;
    public TMP_Text objectiveItemsText;

    public GameObject pauseMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateObjectiveUI();
    }

    public void SetObjective(Objective newObjective)
    {
        if (currentObjective != null && currentObjective == newObjective)
            return;

        currentObjective = newObjective;
        UpdateObjectiveUI();
    }

    private void UpdateObjectiveUI()
    {
        if (currentObjective == null)
        {
            objectiveTitleText.text = "";
            objectiveDescriptionText.text = "";
            objectiveItemsText.text = "";
        }
        else
        {
            objectiveTitleText.text = currentObjective.title;
            objectiveDescriptionText.text = currentObjective.description;

            // Specific UI updates based on objective type
            if (currentObjective is CollectItemObjective itemObjective)
            {
                objectiveItemsText.text = $"Collect {itemObjective.itemRequired}: {itemObjective.quantityRequired}";
            }
            else if (currentObjective is CollectItemsForObjectObjective objectObjective)
            {
                objectiveItemsText.text = $"Collect {objectObjective.itemRequired} for {objectObjective.objectToPower}: {objectObjective.quantityRequired}";
            }
            else if (currentObjective is CollectItemsForDoorObjective doorObjective)
            {
                objectiveItemsText.text = $"Collect {doorObjective.itemRequired} to open {doorObjective.doorName}: {doorObjective.quantityRequired}";
            }
            else if (currentObjective is GoToAreaObjective areaObjective)
            {
                objectiveItemsText.text = $"Go to {areaObjective.areaName}";
            }
        }
    }

    public void CompleteObjective()
    {
        if (currentObjective == null)
            return;

        currentObjective.OnObjectiveComplete();
        Objective nextObjective = currentObjective.nextObjective;

        if (nextObjective != null)
        {
            SetObjective(nextObjective);
        }
        else
        {
            currentObjective = null;
            UpdateObjectiveUI(); // No objective to show
        }
    }

    public void DisplayCurrentObjectiveInPauseMenu()
    {
        if (pauseMenu != null)
        {
            // Update the pause menu with the current objective info
        }
    }

    public void NotifyAreaEntered(string areaName)
    {
        currentArea = areaName;

        if (currentObjective is GoToAreaObjective areaObjective)
        {
            if (currentArea == areaObjective.areaName)
            {
                CompleteObjective();
            }
        }
        else
        {
            // Check if there is a new objective to be set
            foreach (var obj in FindObjectsOfType<AreaTrigger>())
            {
                if (obj.areaName == currentArea && currentObjective == null && obj.prerequisiteObjective == null)
                {
                    SetObjective(obj.areaObjective);
                }
            }
        }
    }

    public void NotifyAreaExited(string areaName)
    {
        // Optional: Handle logic for when the player exits an area, if needed
    }

    public void NotifyItemCollected(string itemName)
    {
        // Handle item collection logic and check if it completes any objectives
    }

    public void DebugCompleteObjective()
    {
        CompleteObjective();
    }
}