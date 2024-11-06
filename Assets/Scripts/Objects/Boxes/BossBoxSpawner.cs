using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoxSpawner : MonoBehaviour
{
    public GameObject boxBox;
    public List<GameObject> boxes = new List<GameObject>();
    public List<Transform> boxSpawnLocations = new List<Transform>();
    public int boxAmount;
    // Start is called before the first frame update
    void Start()
    {
        RespawnBoxes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBoxes(int index){
        boxes.Remove(boxes[index]);
        if(boxes.Count <= 0){
            RespawnBoxes();
        }
    }

    public void RespawnBoxes(){
        boxes.Clear();
        for(int i = 0; i < boxAmount; i++){
            Vector3 spawn = new Vector3(boxSpawnLocations[i].position.x,boxSpawnLocations[i].position.y,boxSpawnLocations[i].position.z); 
            GameObject newBox = Instantiate(boxBox, boxSpawnLocations[i].position, boxSpawnLocations[i].rotation);
            newBox.GetComponent<BossDamageBox>().indexBox = i;
            boxes.Add(newBox);
        }
    }
}
