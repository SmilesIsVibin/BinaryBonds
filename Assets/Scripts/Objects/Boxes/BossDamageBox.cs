using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageBox : MonoBehaviour
{
    public RobotBoxInteraction rbi;
    public BossBoxSpawner bossBoxSpawner;
    public int indexBox;
    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("BossArm")){
            rbi.StopInteraction();
            bossBoxSpawner.UpdateBoxes(indexBox);
            Destroy(this.gameObject);
        }
    }
}
