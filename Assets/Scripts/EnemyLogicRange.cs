using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogicRange : MonoBehaviour
{

    public float speed = 1.5f;
    public float detectionRange = 8f;
    public int enemyHP = 50;
    public float attackCooldown = 2f;
    public float throwForce = 3f;
    public GameObject bombPrefab;
    public Transform throwPoint;

    public Transform player;
    public Transform groundDetection;
    public LayerMask groundLayers;

    private Rigidbody2D enemyRB;
    private bool movingRight = true;
    private bool isAttacking = false;

    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            Patrol();
        }

        if(Vector2.Distance(transform.position, player.position) < detectionRange && !isAttacking)
        {

            StartCoroutine(PrepareAttack());

        }
    }

    void Patrol()
    {
        enemyRB.velocity = new Vector2(movingRight ? speed : -speed, enemyRB.velocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f, groundLayers);
        if (!groundInfo.collider)
        {
            Flip();
        }

    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector2(0, movingRight ? 0 : 180);
    }

    IEnumerator PrepareAttack()
    {
        isAttacking = true;
        enemyRB.velocity = Vector2.zero;
        Debug.Log("Range Enemy Preparing Attack");

        yield return new WaitForSeconds(attackCooldown);



        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            ThrowBomb();
        }
        else
        {
            isAttacking = false;
        }
    }

    void ThrowBomb()
    {
        if(bombPrefab != null)
        {
            GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);
            Rigidbody2D bombRB = bomb.GetComponent<Rigidbody2D>();
            if(bombRB != null)
            {
                Vector2 throwDirection = (player.position - throwPoint.position).normalized;
                bombRB.velocity = new Vector2(throwDirection.x * throwForce, Mathf.Abs(throwDirection.y * throwForce));
            }
        }
        else
        {
            Debug.LogError("Bomb Prefab is missing! Assing it");
        }

        
        if(Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            StartCoroutine(PrepareAttack());
        }
        else
        {
            isAttacking = false;
        }
        
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        if(enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
