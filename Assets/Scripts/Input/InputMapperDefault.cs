using UnityEngine;
using System.Collections;
using System;

public class InputMapperDefault : InputMapper {

    private int playerIndex;

    public InputMapperDefault(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public override Vector2 getMovement()
    {
        movement.x = Input.GetAxis("Horizontal" + playerIndex) + Input.GetAxis("Horizontal" + playerIndex + "Keyboard");
        movement.y = Input.GetAxis("Vertical" + playerIndex) + Input.GetAxis("Vertical" + playerIndex + "Keyboard");
        return movement;
    }

    public override void Update()
    {
        wasCharging = charging;
        charging = Input.GetButton("Boost" + playerIndex);
    }
}
