using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;

    [Header("UI Elements")]
    public Animator canvasAnimator;
    public TextMeshProUGUI objectiveTitleText;
    public TextMeshProUGUI objectiveDescriptionText;
    public TextMeshProUGUI objectiveStatusText;
    public TextMeshProUGUI notificationText;
    public TextMeshProUGUI timerText;
    public ObjectiveMarker objectiveMarker;

    [Header("Objectives Data")]
    public List<Objective> allObjectives;
    private int currentObjectiveIndex = -1;

    private Queue<string> notificationQueue = new Queue<string>();
    private bool isNotificationActive = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (allObjectives.Count > 0)
        {
            SetObjective(0); // Start with the first objective in the list
        }
    }

    public void SetObjective(int index)
    {
        if (index < 0 || index >= allObjectives.Count)
        {
            Debug.LogError("Objective index out of range.");
            UpdateObjectiveUI();
            return;
        }

        if (currentObjectiveIndex == index && allObjectives[index].isCompleted)
        {
            return;
        }

        currentObjectiveIndex = index;
        UpdateObjectiveUI();

        if (currentObjectiveIndex >= 0)
        {
            Objective objective = allObjectives[currentObjectiveIndex];
            if (!objective.isCompleted)
            {
                QueueNotification("Objective: " + objective.description);

                if (objective.type == Objective.ObjectiveType.Timed)
                {
                    objective.StartTimer();
                    timerText.gameObject.SetActive(true);
                }
                else
                {
                    timerText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }
    }

    public void CompleteObjective()
    {
        objectiveMarker.DisableTracking();
        if (currentObjectiveIndex < 0 || currentObjectiveIndex >= allObjectives.Count)
            return;

        Objective currentObjective = allObjectives[currentObjectiveIndex];
        if (currentObjective.isCompleted){
            // Mark the current objective as completed
            currentObjective.isCompleted = true;
            QueueNotification("Completed: " + currentObjective.description);

            // Move to the next objective
            int nextObjectiveIndex = currentObjective.nextObjectiveIndex;
            if (nextObjectiveIndex >= 0 && nextObjectiveIndex < allObjectives.Count)
            {
                SetObjective(nextObjectiveIndex);
            }
            else
            {
                // No more objectives to move to
                currentObjectiveIndex = -1;
                UpdateObjectiveUI();
            }
            return;
        }
    }

    private void UpdateObjectiveUI()
    {
        if (currentObjectiveIndex < 0 || currentObjectiveIndex >= allObjectives.Count)
        {
            objectiveTitleText.text = "Nothing to do right now...";
            objectiveDescriptionText.text = ":D";
            objectiveStatusText.text = "";
            timerText.text = "";
        }
        else
        {
            Objective objective = allObjectives[currentObjectiveIndex];
            objectiveTitleText.text = objective.title;
            objectiveDescriptionText.text = objective.description;
            objectiveStatusText.text = objective.GetObjectiveStatus();

            if (objective.type == Objective.ObjectiveType.Timed)
            {
                timerText.text = $"Time Left: {objective.timeRemaining:F2}";
            }
        }
    }

    private void QueueNotification(string message)
    {
        notificationQueue.Enqueue(message);
        if (!isNotificationActive)
        {
            StartCoroutine(DisplayNotifications());
        }
    }

    private IEnumerator DisplayNotifications()
    {
        while (notificationQueue.Count > 0)
        {
            isNotificationActive = true;
            notificationText.text = notificationQueue.Dequeue();
            canvasAnimator.SetTrigger("OpenNotification");

            yield return new WaitForSeconds(2f);
            objectiveMarker.target = allObjectives[currentObjectiveIndex].markerLocation;
            objectiveMarker.EnableTracking();
            Debug.Log("Notification Closing");
        }
        isNotificationActive = false;
    }

    public void NotifyItemCollected(string itemName, int quantity)
    {
        if (currentObjectiveIndex < 0 || currentObjectiveIndex >= allObjectives.Count)
            return;

        //Objective objective = allObjectives[currentObjectiveIndex];
        foreach(Objective obj in allObjectives){
            if (obj.type == Objective.ObjectiveType.Collect && obj.itemName == itemName)
            {
                if(obj == allObjectives[currentObjectiveIndex])
                {
                    obj.ProgressObjective(quantity);
                    if (obj.isCompleted)
                    {
                        CompleteObjective();
                    }
                    else
                    {
                        QueueNotification($"Item Collected: {itemName}");
                        UpdateObjectiveUI();
                    }
                }else
                {
                    obj.ProgressObjective(quantity);
                    QueueNotification($"Item Collected: {itemName}");
                    if(obj.isCompleted){
                        allObjectives[currentObjectiveIndex].nextObjectiveIndex = obj.nextObjectiveIndex;
                    }
                }
            }
        }
    }

    public void NotifyAreaEntered(string areaName, bool unlockObj)
    {
        if(unlockObj){
            foreach (var objective in allObjectives)
            {
                if (objective.type == Objective.ObjectiveType.Explore && objective.areaName == areaName && !objective.isCompleted)
                {
                    SetObjective(allObjectives.IndexOf(objective));
                    break;
                }
            }
        }
        else{
            //Objective objective = allObjectives[currentObjectiveIndex];
            foreach(Objective obj in allObjectives){
                if (obj.type == Objective.ObjectiveType.Explore && obj.areaName == areaName)
                {
                    if(obj == allObjectives[currentObjectiveIndex])
                    {
                        obj.ProgressObjective();
                        if (obj.isCompleted)
                        {
                            CompleteObjective();
                        }
                        else
                        {
                            QueueNotification($"Area Entered: {areaName}");
                            UpdateObjectiveUI();
                        }
                    }else
                    {
                        obj.ProgressObjective();
                        QueueNotification($"Area Entered: {areaName}");
                        if(obj.isCompleted){
                        allObjectives[currentObjectiveIndex].nextObjectiveIndex = obj.nextObjectiveIndex;
                    }
                }
            }
        }
        }
    }

    public void NotifyActivationTriggered(string activationObjectName)
    {
    // Check if the current objective requires this activation
    if (currentObjectiveIndex >= 0 && currentObjectiveIndex < allObjectives.Count)
    {
        Objective currentObjective = allObjectives[currentObjectiveIndex];
        if (currentObjective.type == Objective.ObjectiveType.Activation && currentObjective.objectActivatedName == activationObjectName)
        {
            currentObjective.ProgressObjective();
            if (currentObjective.isCompleted)
            {
                CompleteObjective();
            }
            else
            {
                UpdateObjectiveUI();
            }
        }
    }

    // If a future objective needs this activation, mark it as completed
    for (int i = currentObjectiveIndex + 1; i < allObjectives.Count; i++)
    {
        Objective futureObjective = allObjectives[i];
        if (futureObjective.type == Objective.ObjectiveType.Activation && futureObjective.objectActivatedName == activationObjectName && !futureObjective.isCompleted)
        {
            futureObjective.ProgressObjective();

            // Auto-complete the objective if conditions are met
            if (futureObjective.isCompleted)
            {
                // Move to the next objective if auto-completed
                int nextObjectiveIndex = futureObjective.nextObjectiveIndex;
                if (nextObjectiveIndex >= 0 && nextObjectiveIndex < allObjectives.Count)
                {
                    SetObjective(nextObjectiveIndex);
                }
            }
            break;
        }
    }
}
}
