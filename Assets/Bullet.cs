using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 1f;
    public float BulletAliveTime = 1f;
    float _currentTime = 0f;
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
    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= BulletAliveTime)
        {
            Destroy(gameObject);
        }
    }
}
