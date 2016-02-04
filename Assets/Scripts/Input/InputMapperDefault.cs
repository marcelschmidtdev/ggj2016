using UnityEngine;
using System.Collections;
using System;

public class InputMapperDefault : InputMapper {

    private int playerIndex;

	private bool _keyboardConnected; 
	public override bool keyboardConnected {
		get {
			return _keyboardConnected; 
		}
		protected set {
			_keyboardConnected = value; 
		}
	}
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
		var startButton = Input.GetButtonDown("Start" + playerIndex);
		if ( this.playerIndex == 1 && this.keyboardConnected) 
			startButton |= checkKeyboardStartButton() ; 
		return startButton; 
	}

	private bool checkKeyboardStartButton ()
	{
		return Input.GetKey(KeyCode.Space ) ; 
	}

	public override bool GetConfirm ()
	{
		var confirm = Input.GetButton("Boost" + playerIndex);
		if ( playerIndex == 1 && this.keyboardConnected) {
			confirm |= checkKeyboardConfirm(); 
		}
		return confirm; 
	}

	private bool checkKeyboardConfirm ()
	{
		return Input.GetKey(KeyCode.LeftAlt ) ; 
	}
	
	public override bool GetCancel ()
	{
		var cancel = Input.GetButtonDown("Cancel" + playerIndex);
		if ( playerIndex == 1 && this.keyboardConnected) {
			cancel |= Input.GetKey(KeyCode.LeftShift); 
		}
		return cancel; 
	}

	public override bool IsConnected () {
		var isConnected = Input.GetJoystickNames().Length >= playerIndex && !string.IsNullOrEmpty(Input.GetJoystickNames()[playerIndex-1]);
		if (playerIndex == 1 ) {
			isConnected |= checkKeyboardIsConnected(); 
		}
		return isConnected; 
	}

	private bool checkKeyboardIsConnected ()
	{
		this.keyboardConnected |= Input.GetKey(KeyCode.Space); 
		return this.keyboardConnected; 
	}

	public override bool IsCalibrated ()
	{
		var calibrated = Mathf.Abs(Input.GetAxis("AccelerationPlayer" + playerIndex)) > 0 && Mathf.Abs (Input.GetAxis("BrakePlayer" + playerIndex)) > 0;
		if ( playerIndex == 1 ) {
			calibrated |= checkKeyboardIsCalibrated(); 
		}
		return calibrated; 
	}

	private bool checkKeyboardIsCalibrated ()
	{
		return Input.GetKey(KeyCode.A); 
	}

    private float remap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }
}
