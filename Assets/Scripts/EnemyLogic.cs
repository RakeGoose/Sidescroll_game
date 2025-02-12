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

    private Animator animator;
    private Coroutine attackCoroutine;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            Patrol();
        }

        if (Vector2.Distance(transform.position, player.position) < detectionRange && !isAttacking)
        {
            
            attackCoroutine = StartCoroutine(PrepareAttack());
        }
        
    }

    void Patrol()
    {
        animator.SetBool("IsMoving", true);
        animator.SetBool("PreparingAttack", false);

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
        animator.SetBool("IsMoving", false);
        animator.SetBool("PreparingAttack", true);
        
     
        yield return new WaitForSeconds(attackCooldown);

        animator.SetBool("PreparingAttack", false);

        

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            attackCoroutine = StartCoroutine(AttackSequence());
        }
        else
        {
            isAttacking = false;
        }
    }

    IEnumerator AttackSequence()
    {
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
        }

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        animator.SetTrigger("TakeDamage");

        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            isAttacking = false;
            animator.SetBool("PreparingAttack", false);
        }

        if(enemyHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        enemyRB.velocity = Vector2.zero;
        isAttacking = true;

        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
