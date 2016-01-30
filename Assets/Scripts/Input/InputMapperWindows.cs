using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;

public class InputMapperWindows : InputMapper {
    PlayerIndex index;

    public InputMapperWindows(int index)
    {
        this.index = (PlayerIndex)index;
    }

    public override Vector2 getMovement()
    {
        GamePadState gamepad = GamePad.GetState(index);
        movement.x = gamepad.ThumbSticks.Left.X;
        movement.y = gamepad.ThumbSticks.Left.Y;
        return movement;
    }

    public override void Update()
    {
        wasCharging = charging;
        GamePadState gamepad = GamePad.GetState(index);
        charging = gamepad.Buttons.A == ButtonState.Pressed;
    }
}
