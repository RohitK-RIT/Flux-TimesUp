using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

public class HealingOrb : MonoBehaviour, IDamageable
{
    //public int orbHealth = 10;
    private int healAmount = 1;
    private BossController boss;
    [SerializeField] private float currentHealth = 0;
    private float maxHealth = 300f;
    

    void Start() {
        //base.Start();
        currentHealth = maxHealth;
        boss = FindObjectOfType<BossController>();
        StartCoroutine(HealBoss());
    }

    IEnumerator HealBoss() {
        while (boss != null) {
            boss._enemyController.currentHealth += healAmount;
            yield return new WaitForSeconds(1f);
        }
    }

    public void TakeDamage(Weapon weapon, float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0)
            gameObject.SetActive(false);
    }
}
