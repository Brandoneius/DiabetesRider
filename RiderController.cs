using UnityEngine;
using UnityEngine.UI;

public class RiderController : MonoBehaviour
{
    public float speed = 5f;
    public float maxSpeed = 20f;
    public float boostAmount = 5f;
    public float verticalBoostForce = 10f; // 🚀 New vertical boost force
    public int totalBoosts = 3;
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

    public void UseBoost(Vector2 boostDirection)
    {
        if (remainingBoosts > 0)
        {
            remainingBoosts--;
            rb.linearVelocity += boostDirection; // 🚀 Apply force in chosen direction
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
            boostCounterText.text = $"Gummies Left: {remainingBoosts}";
        }
    }
}
