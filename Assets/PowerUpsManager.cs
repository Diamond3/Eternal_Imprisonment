using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public List<PowerUpData> PowerUps = new();
    static PowerUpsManager _instance;
    float _attackSpeed = 0f;
    float _jumpHeight = 0f;
    float _movementSpeed = 0f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddNewPowerUp(PowerUpData powerUp, Transform player)
    {
        PowerUps.Add(powerUp);
        UpdatePlayerStats(powerUp, player);

        FindObjectOfType<PowerUpUI>().UpdateText();
    }

    private void UpdatePlayerStats(PowerUpData powerUp, Transform player)
    {
        switch (powerUp.PowerUp)
        {
            case PowerUpType.AttackSpeed:
                player.GetComponent<ShootingLogic>().TimeBetweenAttacks *= (1 - powerUp.FloatValue);
                break;
            case PowerUpType.MovementSpeed:
                player.GetComponent<PlayerMovement>().Data.runMaxSpeed *= (1 + powerUp.FloatValue);
                break;
                case PowerUpType.JumpHeight:
                player.GetComponent<PlayerMovement>().Data.jumpHeight *= (1 + powerUp.FloatValue);
                player.GetComponent<PlayerMovement>().Data.OnValidatePublic();
                break;
            case PowerUpType.Shooting:
                player.GetComponent<ShootingLogic>().AbleToShoot = true;
                break;
        }
    }
    public void AddAllPowerUps(Transform player)
    {
        _attackSpeed = player.GetComponent<ShootingLogic>().TimeBetweenAttacks;
        _movementSpeed = player.GetComponent<PlayerMovement>().Data.runMaxSpeed;
        _jumpHeight = player.GetComponent<PlayerMovement>().Data.jumpHeight;

        if (PowerUps.Count == 0) return;
        foreach (var p in PowerUps)
        {
            UpdatePlayerStats(p, player);
        }

        FindObjectOfType<PowerUpUI>().UpdateText();
    }
    internal void AddNewPowerUpRange(List<PowerUpData> powerUpList)
    {
        PowerUps.AddRange(powerUpList);
    }

    public (float, float, float) GetPowerUpsCount()
    {
        var ats = 1 - FindObjectOfType<ShootingLogic>().TimeBetweenAttacks / _attackSpeed;
        var ms = FindObjectOfType<PlayerMovement>().Data.runMaxSpeed / _movementSpeed;
        var jh = FindObjectOfType<PlayerMovement>().Data.jumpHeight / _jumpHeight;
        return (ats, ms, jh);
    }
}
