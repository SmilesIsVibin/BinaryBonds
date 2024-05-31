using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCarry : MonoBehaviour
{
    [SerializeField] public RobotPickup robotPickup;

    public void Pickup()
    {
        robotPickup.Pickup();
    }

    public void Drop()
    {
        robotPickup.Drop();
    }
}
