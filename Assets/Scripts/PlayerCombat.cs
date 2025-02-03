using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1f;
    public int meleeDamage = 25;
    public LayerMask enemyLayers;
    
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MeleeAtack();
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

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null) {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
