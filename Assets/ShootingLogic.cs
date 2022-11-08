using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLogic : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] float _bulletSpeed = 10f;
    public float TimeBetweenAttacks = 0.7f;
    float _nextAttackTime = 0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < _nextAttackTime) return;
        if (!Input.GetMouseButton(0)) return;
        _nextAttackTime = Time.time + TimeBetweenAttacks;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0f;
        var dir = (worldPoint - transform.position).normalized;
        float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var bulletObj = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, rotAngle)));
        bulletObj.GetComponent<Rigidbody2D>().velocity = bulletObj.transform.right * _bulletSpeed;
    }
}
