using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    public override void PickUp()
    {
        PlayerController.instance.ApplySpeedBoost();
        PlayerController.instance.speedBoost = this;
    }
    public override void ResetPowerUp()
    {
        if(PlayerController.instance.speedBoost == this)
        {
            PlayerController.instance.ResetSpeedBoost();
        }
    }
}
