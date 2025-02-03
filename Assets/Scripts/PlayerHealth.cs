using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth;
    public int lives = 2;

    public Slider healthBar;
    public Text livesText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            lives--;
            if(lives <= 0)
            {
                Debug.Log("Game OVER");
            }
            else
            {
                currentHealth = maxHealth;
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        healthBar.value = (float)currentHealth / maxHealth;
        livesText.text = "Lives: " + lives;
    }
}
