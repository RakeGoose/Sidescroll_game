using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidZone : MonoBehaviour
{

    public int damagePerSecond = 5;
    public float duration = 2f;

    void Start()
    {
        StartCoroutine(DestroyAfterSeconds());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                StartCoroutine(DamageOverTime(playerHealth));
            }
        }
    }

    IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    IEnumerator DamageOverTime(PlayerHealth playerHealth)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            playerHealth.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
    }
}
