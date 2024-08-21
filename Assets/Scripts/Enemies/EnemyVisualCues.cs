using UnityEngine;

public class EnemyVisualCues : MonoBehaviour
{
    public GameObject questionMark;
    public GameObject exclamationMark;
    private EnemyAIController aiController;

    void Start()
    {
        aiController = GetComponent<EnemyAIController>();
        questionMark.SetActive(false);
        exclamationMark.SetActive(false);
    }

    void Update()
    {
        switch (aiController.currentState)
        {
            case EnemyAIController.EnemyState.Suspicious:
                ShowQuestionMark();
                break;
            case EnemyAIController.EnemyState.Alerted:
            case EnemyAIController.EnemyState.Chasing:
                ShowExclamationMark();
                break;
            default:
                HideAll();
                break;
        }
    }

    void ShowQuestionMark()
    {
        questionMark.SetActive(true);
        exclamationMark.SetActive(false);
    }

    void ShowExclamationMark()
    {
        questionMark.SetActive(false);
        exclamationMark.SetActive(true);
    }

    void HideAll()
    {
        questionMark.SetActive(false);
        exclamationMark.SetActive(false);
    }
}
