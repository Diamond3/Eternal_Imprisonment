using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public List<PowerUpData> PowerUps = new();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddNewPowerUp(PowerUpData powerUp)
    {
        PowerUps.Add(powerUp);
        UpdatePlayerStats(powerUp);
    }

    private void UpdatePlayerStats(PowerUpData powerUp)
    {
        switch (powerUp.PowerUp)
        {
            case PowerUpType.AttackSpeed:
                GetComponent<ShootingLogic>().TimeBetweenAttacks *= (1 - powerUp.FloatValue);
                break;
            case PowerUpType.MovementSpeed:
                GetComponent<PlayerMovement>().Data.runMaxSpeed *= (1 + powerUp.FloatValue);
                break;
            case PowerUpType.JumpHeight:
                GetComponent<PlayerMovement>().Data.jumpHeight *= (1 + powerUp.FloatValue);
                break;
        }
    }

    internal void AddNewPowerUpRange(List<PowerUpData> powerUpList)
    {
        PowerUps.AddRange(powerUpList);
    }
}
