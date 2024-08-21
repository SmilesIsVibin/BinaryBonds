using UnityEngine;

public class LeverManager : MonoBehaviour
{
    public GameObject gate; // The gate or door to be unlocked
    public Lever[] levers; // Array of all levers this manager handles
    public bool[] correctCombination; // The correct combination for the gate to unlock

    void Start()
    {
        UpdateGateState(); // Check the state on start to ensure consistency
    }

    // Call this method whenever a lever is toggled
    public void LeverToggled()
    {
        UpdateGateState();
    }

    private void UpdateGateState()
    {
        for (int i = 0; i < levers.Length; i++)
        {
            if (levers[i].isOn != correctCombination[i])
            {
                gate.SetActive(true); // Keep the gate closed if the combination is wrong
                return;
            }
        }

        // If all lever states match the correct combination, open the gate
        gate.SetActive(false);
    }

    public void LeverToggled(Lever toggledLever)
    {
        UpdateGateState();

        // Additional logic for linked levers
        foreach (Lever lever in toggledLever.adjacentLevers)
        {
            lever.ToggleState();
        }
    }
}
