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
        movement.x = Input.GetAxis("Horizontal" + playerIndex) + Input.GetAxis("Horizontal" + playerIndex + "Keyboard");
        movement.y = Input.GetAxis("Vertical" + playerIndex) + Input.GetAxis("Vertical" + playerIndex + "Keyboard");
        return movement;
    }

	public override bool GetStart ()
	{
		return false;
	}

	public override bool GetConfirm ()
	{
		return Input.GetButtonDown( "Submit" );
	}
	
	public override bool GetCancel ()
	{
		return Input.GetButtonDown("Cancel");
	}

	public override bool IsConnected () {
		return true;
	}

	public override void Update()
    {
        wasCharging = charging;
        charging = Input.GetButton("Boost" + playerIndex);
    }
}
