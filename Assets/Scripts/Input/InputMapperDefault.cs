using UnityEngine;
using System.Collections;
using System;

public class InputMapperDefault : InputMapper {

    private int playerIndex;

    public InputMapperDefault(int playerIndex)
    {
        this.playerIndex = playerIndex + 1;
    }

    public override Vector2 getMovement()
    {
        movement.y = Input.GetAxis("AccelerationPlayer" + playerIndex) - Input.GetAxis("BrakePlayer" + playerIndex) + Input.GetAxis("AccelerationBrakeKeyboardPlayer" + playerIndex);
        movement.x = Input.GetAxis("TurnPlayer" + playerIndex) + Input.GetAxis("TurnPlayer" + playerIndex + "Keyboard");
        movement.y = Mathf.Clamp(movement.y, -1, 1);
        movement.y = remap(movement.y, -1, 1, 0, 1);
        movement.x = Mathf.Clamp(movement.x, -1, 1);
        movement.x = remap(movement.x, -1, 1, 0, 1);
        return movement;
    }

	public override bool GetStart ()
	{
		return Input.GetButtonDown("Start" + playerIndex);
	}

	public override bool GetConfirm ()
	{
		return Input.GetButtonDown("Boost" + playerIndex);
	}
	
	public override bool GetCancel ()
	{
		return Input.GetButtonDown("Cancel" + playerIndex);
	}

	public override bool IsConnected () {
		return Input.GetJoystickNames().Length >= playerIndex && !string.IsNullOrEmpty(Input.GetJoystickNames()[playerIndex-1]);
	}

    private float remap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }
}
