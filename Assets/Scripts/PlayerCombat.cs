using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1f;
    public int meleeDamage = 25;
    public LayerMask enemyLayers;
    private float attackCooldown = 0.5f;

    private Animator animator;
    private bool isAttacking = false;

    private Vector3 preAttackPosition;
    private Vector2 originalColliderOffset;
    private BoxCollider2D boxCollider;

    public float attackLift = 0.6f;
    public float colliderDrop = 0.6f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalColliderOffset = boxCollider.offset;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    void MeleeAtack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyLogic>()?.TakeDamage(meleeDamage);
            enemy.GetComponent<EnemyLogicRange>()?.TakeDamage(meleeDamage);
        }

    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        

        

        yield return new WaitForSeconds(0.15f);

        MeleeAtack();

        yield return new WaitForSeconds(attackCooldown - 0.15f);

        

        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null) {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
