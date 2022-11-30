using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;
    public bool IsDead = false;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        var anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Destroy(healthBar.gameObject);
        IsDead = true;
        var anim = GetComponent<Animator>();

        if (anim)
        {
            anim.speed = 1f;
            anim.SetTrigger("Death");
        }
    }

    public void RestoreHp(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
