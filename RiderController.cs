using UnityEngine;
using UnityEngine.UI;

public class RiderController : MonoBehaviour
{
    public float speed = 5f;
    public float maxSpeed = 20f;
    public float boostAmount = 5f;
    public float verticalBoostForce = 10f; // 🚀 New vertical boost force
    public int totalBoosts = 10;
    private int remainingBoosts;
    private Rigidbody2D rb;

    public Text boostCounterText; // UI to show remaining boosts

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        remainingBoosts = totalBoosts;
        UpdateBoostUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Standard boost (X direction)
        {
            UseBoost(Vector2.right * boostAmount);
        }

        if (Input.GetKeyDown(KeyCode.W)) // Jump boost (Y direction)
        {
            UseBoost(Vector2.up * verticalBoostForce);
        }
    }

public ParticleSystem boostTrailEffect;
public Transform playerBody; // Set to your player’s transform if needed
public float boostTrailOffset = 0.5f; // How far from center to move the particle trail

public void UseBoost(Vector2 boostDirection)
{
    if (remainingBoosts > 0)
    {
        remainingBoosts--;

        // Apply boost
        rb.AddForce(boostDirection * boostAmount, ForceMode2D.Impulse);

        // Position the particle effect to the OPPOSITE side of the boost direction
        if (boostTrailEffect != null)
        {
            // Normalize the direction to get just the angle
            Vector2 oppositeDir = -boostDirection.normalized;

            // Offset the particle system's local position
            boostTrailEffect.transform.localPosition = new Vector3(oppositeDir.x, oppositeDir.y, 0) * boostTrailOffset;

            // Rotate the particle system so it shoots in the correct direction
            float angle = Mathf.Atan2(boostDirection.y, boostDirection.x) * Mathf.Rad2Deg;
            boostTrailEffect.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            boostTrailEffect.Play();
        }

        UpdateBoostUI();
    }
}


    public void AddBoost(int amount)
    {
        remainingBoosts += amount;
        UpdateBoostUI();
    }

    void UpdateBoostUI()
    {
        if (boostCounterText != null)
        {
            boostCounterText.text = $"Gummies: {remainingBoosts}";
        }
    }
}
