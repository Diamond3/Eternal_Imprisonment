using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<HealthManager>().TakeDamage(Damage);
            }
            Destroy(gameObject);

        }
    }
}
