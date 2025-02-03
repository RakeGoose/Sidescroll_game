using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    public float speed = 2f;
    public float detectionRange = 0.5f;
    public float attackCooldown = 1.5f;
    private int enemyHP = 100;
    public int attackDamage = 20;

    public Rigidbody2D enemyRB;
    private bool movingRight = true;
    private bool isAttacking = false;
    
    
    public Transform player;
    public Transform groundDetection;
    public LayerMask groundLayers;

    

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            Patrol();
        }

        if (Vector2.Distance(transform.position, player.position) < detectionRange && !isAttacking)
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
        Debug.Log("Enemy Preparing Attack");
     
        yield return new WaitForSeconds(attackCooldown);

        

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            Attack();
        }
        else
        {
            
            isAttacking = false;
        }
    }

    void Attack()
    {
        Debug.Log("Enemy attack");
        player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
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
