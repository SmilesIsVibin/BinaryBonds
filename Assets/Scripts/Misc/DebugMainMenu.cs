using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMainMenu : MonoBehaviour
{
    public GameObject debugMenu; // Assign the Debug Menu in the Inspector
    private bool isPaused = false;

    void Update()
    {
        // Listen for Ctrl + 0 to toggle the debug menu
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha0) )
        {
            ToggleDebugMenu();
        }
    }

    void ToggleDebugMenu()
    {
        isPaused = !isPaused;

        // Toggle pause state and show/hide the debug menu
        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game
            debugMenu.SetActive(true); // Show Debug Menu
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            debugMenu.SetActive(false); // Hide Debug Menu
        }
    }
}
