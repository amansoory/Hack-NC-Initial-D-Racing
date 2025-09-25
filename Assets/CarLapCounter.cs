using UnityEngine;
using UnityEngine.UI;

public class CarLapCounter : MonoBehaviour
{
    public int currentLap = 0;
    public int totalLaps = 3;
    public Text lapText;  // Assign this in the Inspector to display the lap number on screen

    private void Start()
    {
        UpdateLapText();
    }

    public void IncrementLap()
    {
        currentLap++;
        if (currentLap > totalLaps)
        {
            currentLap = totalLaps;  // Ensure it does not go beyond the total laps
            Debug.Log("Race Completed!");
        }
        UpdateLapText();
    }

    private void UpdateLapText()
    {
        if (lapText != null)
        {
            lapText.text = "Lap: " + currentLap + " / " + totalLaps;
        }
    }
}
