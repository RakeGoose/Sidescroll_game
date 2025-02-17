using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public GameObject acidZonePrefab;
    public int damage = 15;
    public float acidZoneDuration = 2f;

    private Animator animator;
    private bool hasExploded = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;
            animator.SetTrigger("Explode");
            StartCoroutine(SpawnAcidZone());
        }
    }

    IEnumerator SpawnAcidZone()
    {
        yield return new WaitForSeconds(0.5f);

        if(acidZonePrefab != null)
        {
            GameObject acidZone = Instantiate(acidZonePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Acid Zone Prefab is missing! Assing it");
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Explode();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Explode();
        }
    }
}
