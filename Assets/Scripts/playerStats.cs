using UnityEngine;
using TMPro;

public class playerStats : MonoBehaviour
{
    public playerHealth player; // Reference to the player's health script
    public TextMeshProUGUI healthText; // Reference to the UI text element

    void Update()
    {
        // Continuously update the health UI with the player's current health
        healthText.text = "" + player.health.ToString("F0"); // F0 = no decimals
    }
}
