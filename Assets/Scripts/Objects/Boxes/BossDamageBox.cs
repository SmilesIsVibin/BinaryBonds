using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageBox : MonoBehaviour
{
    public RobotBoxInteraction rbi;
    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("BossArm")){
            rbi.StopInteraction();
            Destroy(this.gameObject);
        }
    }
}
