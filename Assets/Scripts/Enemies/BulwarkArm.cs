using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulwarkArm : MonoBehaviour
{
    public TheBulwark bulwark;
    public bool isActive;
    public void OnTriggerEnter(Collider other){
        if(isActive){
        if(other.CompareTag("Player") || other.CompareTag("Player_Elara") || other.CompareTag("Player_Happy")){
            PlayerManager.Instance.PlayerGameOver();
            return;
        }

        if(other.CompareTag("PushableBox")){
            bulwark.HurtBoss();
            return;
        }
        }
    }
}
