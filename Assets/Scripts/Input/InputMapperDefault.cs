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
        movement.y = remap(Input.GetAxis("AccelerationPlayer" + playerIndex), -1, 1, 0, 1) - remap(Input.GetAxis("BrakePlayer" + playerIndex), -1, 1, 0, 1) + Input.GetAxis("AccelerationBrakeKeyboardPlayer" + playerIndex);
        movement.x = Input.GetAxis("TurnPlayer" + playerIndex) + Input.GetAxis("TurnPlayer" + playerIndex + "Keyboard");
		//Workaround: sometimes controller axis is not getting 0 although it was released
		if(Mathf.Abs(movement.x)<= 0.35f){
			movement.x = 0;
		}
		//Debug.Log(movement);
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

	public override bool IsCalibrated ()
	{
		return Mathf.Abs(Input.GetAxis("AccelerationPlayer" + playerIndex)) > 0 && Mathf.Abs (Input.GetAxis("BrakePlayer" + playerIndex)) > 0;
	}

    private float remap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }
}
