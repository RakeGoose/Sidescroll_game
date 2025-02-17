using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidZone : MonoBehaviour
{

    public int damagePerSecond = 5;
    public float duration = 2f;
    private HashSet<PlayerHealth> playersInZone = new HashSet<PlayerHealth>();

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        StartCoroutine(DestroyAfterSeconds());

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if(playerHealth != null && !playersInZone.Contains(playerHealth))
            {
                playersInZone.Add(playerHealth);
                StartCoroutine(DamageOverTime(playerHealth));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && playersInZone.Contains(playerHealth))
            {
                playersInZone.Remove(playerHealth);
            }
        }
    }

    IEnumerator DestroyAfterSeconds()
    {
        animator.SetTrigger("Explosions");
        yield return new WaitForSeconds(duration);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator DamageOverTime(PlayerHealth playerHealth)
    {
        float elapsed = 0f;

        while (elapsed < duration && playersInZone.Contains(playerHealth))
        {
            playerHealth.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
    }
}
