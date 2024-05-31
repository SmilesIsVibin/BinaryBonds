using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;
    [Header("Message UI")]
    [SerializeField] private TMP_Text messageText;
    [Header("Messages")]
    [SerializeField] private List<string> messageList = new List<string>();
    [SerializeField] public bool hasStartUpMessage;
    [SerializeField] public int startUpIndex;
    [SerializeField] private float messageTimer;

    private void Start()
    {
        Instance = this;
        messageText.gameObject.SetActive(false);
        if (hasStartUpMessage)
        {
            StartCoroutine(nameof(StartMessage));
        }
    }

    IEnumerator StartMessage()
    {
        yield return new WaitForSeconds(3f);
        TriggerMessage(startUpIndex);
    }

    public void TriggerMessage(int index)
    {
        for(int i = 0; i < messageList.Count; i++)
        {
            if (i == index)
            {
                messageText.text = messageList[i];
                messageText.gameObject.SetActive(true);
                StartCoroutine(nameof(DisplayTime), messageTimer);
            }
        }
    }

    IEnumerator DisplayTime(float timer)
    {
        yield return new WaitForSeconds(timer);
        messageText.gameObject.SetActive(false);
    }
}
