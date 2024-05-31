using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPressurePlate : MonoBehaviour
{
    public List<GameObject> cubes = new List<GameObject>();

    public GameObject gate;

    public void OnTriggerEnter(Collider other)
    {
        foreach(GameObject obj in cubes)
        {
            if (obj == other.gameObject)
            {
                obj.GetComponent<ObjectWeighted>().isInPressurePlate = true;
            }
        }
        CheckObjectList();
    }

    public void OnTriggerExit(Collider other)
    {
        foreach (GameObject obj in cubes)
        {
            if (obj == other.gameObject)
            {
                obj.GetComponent<ObjectWeighted>().isInPressurePlate = false;
            }
        }
    }

    private void CheckObjectList()
    {
        foreach(GameObject obj in cubes)
        {
            if(obj.GetComponent<ObjectWeighted>().isInPressurePlate == true)
            {
                gate.SetActive(false);
            }
        }
    }
}
