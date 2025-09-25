using UnityEngine;

public class LapTracker : MonoBehaviour
{
    public int totalLaps = 3;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object passing through is a player's car
        if (other.CompareTag("PlayerCar"))
        {
            CarLapCounter carLapCounter = other.GetComponent<CarLapCounter>();
            if (carLapCounter != null)
            {
                carLapCounter.IncrementLap();
            }
        }
    }
}
