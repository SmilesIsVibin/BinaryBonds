using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevelTrigger : MonoBehaviour
{
    public int charReq;
    public int currentChars;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player_Elara") || other.CompareTag("Player_Happy"))
        {
            currentChars++;
            if(currentChars >= charReq){
                GameManager.Instance.WinLevel();
            }
        }
    }
}
