using UnityEngine;
using UnityEngine.UI;

public class RiderController : MonoBehaviour
{
    public float speed = 5f;
    public float maxSpeed = 20f;
    public float boostAmount = 5f;
    public int totalBoosts = 3;
    private int remainingBoosts;
    private Rigidbody2D rb;

    public Text boostCounterText; // Assign in Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        remainingBoosts = totalBoosts;
        UpdateBoostUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseBoost();
        }
    }

    public void UseBoost()
    {
        if (remainingBoosts > 0)
        {
            remainingBoosts--;
            speed = Mathf.Min(speed + boostAmount, maxSpeed);
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            Debug.Log($"🚀 Boost Activated! Speed: {speed}, Boosts Left: {remainingBoosts}");
            UpdateBoostUI();
        }
        else
        {
            Debug.Log("No boosts left!");
        }
    }

    void UpdateBoostUI()
    {
        if (boostCounterText != null)
        {
            boostCounterText.text = $"Gummies: {remainingBoosts}";
        }
    }
}
