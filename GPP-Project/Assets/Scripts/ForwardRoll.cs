using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRoll : PowerUp
{
    public override void PickUp()
    {
        PlayerController.instance.CanForwardRoll();
        PlayerController.instance.forwardRoll = this;
    }

    public override void ResetPowerUp()
    {
        if(PlayerController.instance.forwardRoll == this)
        {
            PlayerController.instance.ResetForwardRoll();
        }
    }
}
