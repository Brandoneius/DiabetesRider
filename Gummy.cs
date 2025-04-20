using UnityEngine;

public class Gummy : MonoBehaviour
{
    public int boostAmount = 2; // How many boosts this gummy gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the rider collects it
        {
            RiderController rider = other.GetComponent<RiderController>();
            if (rider != null)
            {
                rider.AddBoost(boostAmount);
                Destroy(gameObject); // Remove gummy after collection
            }
        }
    }
}
