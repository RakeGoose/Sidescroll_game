using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogicRange : MonoBehaviour
{

    public float speed = 1.5f;
    public float detectionRange = 5f;
    public int enemyHP = 50;
    public float attackCooldown = 2f;
    public float throwForce = 3f;
    public GameObject bombPrefab;
    public Transform throwPoint;

    public Transform groundDetection;
    public LayerMask groundLayers;

    private Rigidbody2D enemyRB;
    private bool movingRight = true;
    private bool isAttacking = false;
    private Animator animator;
    private Coroutine attackCoroutine;
    private Transform player;

    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if(playerObj != null)
            {
                player = playerObj.transform;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {
        if(player == null)
        {
            return;
        }

        

        if(Vector2.Distance(transform.position, player.position) < detectionRange)
        {

            LookAtPlayer();
            if (!isAttacking)
            {
                attackCoroutine = StartCoroutine(PrepareAttack());
            }

        }
        else if (!isAttacking)
        {
            Patrol();
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

    void LookAtPlayer()
    {
        if (player == null)
        {
            return;
        }

        bool playerIsRight = player.position.x > transform.position.x;

        if (playerIsRight && !movingRight)
        {
            Flip();
        }
        else if (!playerIsRight && movingRight)
        {
            Flip();
        }
    }

    IEnumerator PrepareAttack()
    {
        isAttacking = true;
        enemyRB.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
        animator.SetBool("PreparingAttack", true);

        yield return new WaitForSeconds(attackCooldown);

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        ThrowBomb();

        yield return new WaitForSeconds(attackCooldown / 2);

        isAttacking = false;
    }

    void ThrowBomb()
    {
        if(player == null)
        {
            return;
        }

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
            attackCoroutine = StartCoroutine(PrepareAttack());
        }
        else
        {
            isAttacking = false;
        }
        
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        animator.SetTrigger("TakeDamage");
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            isAttacking = false;
            animator.SetBool("PreparingAttack", false);
        }

        if (enemyHP <= 0)
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
