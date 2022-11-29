using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpData _powerUpData;
    [SerializeField] bool _hpconsumable = false;
    [SerializeField] float _restoreHpAmount = 3f;
    Transform _playerTransform;

    PowerUpTextPanel _powerUpPanel;
    [SerializeField] string _text;
    [SerializeField] float _pickRadius = 3;
    PowerUpsManager _powerUpsManager;
    public bool Active = true;
    void Start()
    {
        _powerUpsManager = FindObjectOfType<PowerUpsManager>();

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _powerUpPanel = FindObjectOfType<PowerUpTextPanel>();
    }

    void Update()
    {
        if (Active && (_playerTransform.position - transform.position).sqrMagnitude <= _pickRadius * _pickRadius)
        {
            _powerUpPanel.SetPanel(_text, _playerTransform, this, _pickRadius);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_hpconsumable)
                {
                    _playerTransform.GetComponent<HealthManager>().RestoreHp(_restoreHpAmount);
                    Destroy(gameObject);
                }
                else
                {
                    _powerUpsManager.AddNewPowerUp(_powerUpData, transform);
                }
                _powerUpPanel.HidePanel();
                transform.GetComponentInChildren<SpriteSwap>().Swap();
                Active = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _pickRadius);
    }
}
