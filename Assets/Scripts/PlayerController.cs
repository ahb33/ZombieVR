using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] 
    private int health = 100;

    public int GetHealth()
    {
        return health;
    }

    // Method to apply damage to the player
    public void TakeDamage(int damage)
    {
        health -= damage; // Subtract damage from health
        health = Mathf.Max(health, 0); // Ensure health does not go below 0

        // Check if player is dead
        if (health <= 0)
        {
            // Handle player death (e.g., transition to game over screen, respawn, etc.)
        }
    }
}
