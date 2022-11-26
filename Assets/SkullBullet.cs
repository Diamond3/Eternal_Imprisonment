using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBullet : MonoBehaviour
{
    public float Damage = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("trigger enter player");
                collision.GetComponent<HealthManager>().TakeDamage(Damage);
            }
            Destroy(gameObject);

        }
    }
}
