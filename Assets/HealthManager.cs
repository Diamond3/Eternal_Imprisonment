using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;
    public bool IsDead = false;

    public HealthBar healthBar;

    public AudioClip death, attack, hit;

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
        PlayHitSound();
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
        PlayDeathSound();
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

    void PlayAttackSound()
    {
        if (attack == null) return;
        AudioSource.PlayClipAtPoint(attack, this.gameObject.transform.position, 0.3f);
    }
    void PlayHitSound()
    {
        if (hit == null) return;
        AudioSource.PlayClipAtPoint(hit, this.gameObject.transform.position, 0.3f);
    }
    void PlayDeathSound()
    {
        if (death == null) return;
        AudioSource.PlayClipAtPoint(death, this.gameObject.transform.position, 0.3f);
    }
}
