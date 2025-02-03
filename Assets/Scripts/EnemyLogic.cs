using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    public float speed = 2f;
    public float detectionRange = 0.5f;
    public Rigidbody2D enemyRB;
    private bool movingRight = true;
    private bool isAttacking = false;
    private int enemyHP = 100;

    public float attackCooldown = 1.5f;
    public int attackDamage = 20;

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
            Move();
        }

        if (Vector2.Distance(transform.position, player.position) < detectionRange && !isAttacking)
        {
            StartCoroutine(PrepareAttack());
        }
        
    }

    void Move()
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
        transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);
    }

    IEnumerator PrepareAttack()
    {
        isAttacking = true;
        enemyRB.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackCooldown);

        Attack();

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            StartCoroutine(PrepareAttack());
        }
        else
        {
            isAttacking = false;
        }
    }

    void Attack()
    {
        Debug.Log("Enemy attack");
    }
}
