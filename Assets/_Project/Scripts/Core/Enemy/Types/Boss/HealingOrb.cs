using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{
    public int orbHealth = 10;
    public int healAmount = 1;
    private BossController boss;

    void Start() {
        boss = FindObjectOfType<BossController>();
        StartCoroutine(HealBoss());
    }

    IEnumerator HealBoss() {
        while (boss != null && orbHealth > 0) {
            boss.currentHealth += healAmount;
            yield return new WaitForSeconds(1f);
        }
    }

    public void TakeDamage(int damage) {
        orbHealth -= damage;
        if (orbHealth <= 0) Destroy(gameObject);
    }
}
