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
    private Animator animator;
    private Vector2 lastDeathPosition;

    public bool isDead = false;

    public float respawnDelay = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        GameObject healthBarObj = GameObject.FindGameObjectWithTag("HealthBar");
        if (healthBarObj != null)
        {
            healthBar = healthBarObj.GetComponent<Slider>();
        }
        else
        {
            Debug.LogError("Error");
        }

        if (livesText == null)
        {
            GameObject livesTextObj = GameObject.FindGameObjectWithTag("Lives");
            if(livesTextObj != null)
            {
                livesText = livesTextObj.GetComponent<Text>();
            }
            else
            {
                Debug.LogError("Error");
            }
        }

        UpdateUI();
    }


    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        UpdateUI();
        if (currentHealth <= 0)
        {
            lives--;

            if(lives <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(Respawn());
            }
        }
    }

    private void Die()
    {
        isDead = true;
        lastDeathPosition = transform.position;
        animator.SetTrigger("dieAnimation");
        DisablePlayerControls();

        StartCoroutine(GameOver());
    }

    private IEnumerator Respawn()
    {
        isDead = true;
        animator.SetTrigger("dieAnimation");
        DisablePlayerControls();

        yield return new WaitForSeconds(respawnDelay);

        animator.Play("dieAnimation", 0, 1f);
        animator.SetFloat("dieAnimationSpeed", 0.2f);

        yield return new WaitForSeconds(1f);

        animator.SetFloat("dieAnimationSpeed", 0.2f);
        transform.position = lastDeathPosition;
        currentHealth = maxHealth;
        isDead = false;
        UpdateUI();

        EnablePlayerControls();
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Game OVER");
    }

    private void DisablePlayerControls()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void EnablePlayerControls()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
    }

    void UpdateUI()
    {
        if(healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }

        if(livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }
}
