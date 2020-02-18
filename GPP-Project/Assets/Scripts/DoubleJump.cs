using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : PowerUp
{
    public override void PickUp()
    {
        PlayerController.instance.CanDoubleJump();
        PlayerController.instance.doubleJump = this;
    }

    public override void ResetPowerUp()
    {
        if(PlayerController.instance.doubleJump == this)
        {
            PlayerController.instance.ResetDoubleJump();
        }
    }
}
