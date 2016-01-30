using UnityEngine;
using System.Collections;
using System;
#if UNITY_STANDALONE_WIN
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
        movement.y = gamepad.Triggers.Right - gamepad.Triggers.Left;
        return movement;
    }

	public override bool GetStart () {
		return GamePad.GetState(index).Buttons.Start == ButtonState.Pressed;
	}

	public override bool GetConfirm ()
	{
		return GamePad.GetState(index).Buttons.A == ButtonState.Pressed;
	}

	public override bool GetCancel ()
	{
		return GamePad.GetState(index).Buttons.B == ButtonState.Pressed;
	}

	public override bool IsConnected () {
		return GamePad.GetState(index).IsConnected;
	}
}
#endif