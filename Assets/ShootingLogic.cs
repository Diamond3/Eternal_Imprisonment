using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLogic : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] float _bulletSpeed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0f;
        print(worldPoint);
        var dir = (worldPoint - transform.position).normalized;
        print(dir);
        float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var bulletObj = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, rotAngle)));
        bulletObj.GetComponent<Rigidbody2D>().velocity = bulletObj.transform.right * _bulletSpeed;
    }
}
