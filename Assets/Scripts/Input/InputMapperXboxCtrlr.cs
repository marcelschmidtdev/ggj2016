using UnityEngine;
using System.Collections;
using System;
using XboxCtrlrInput;

public class InputMapperXboxCtrlr : InputMapper
{
    private XboxController controller;
    private int playerIndex;
    
    public InputMapperXboxCtrlr(int index)
    {
        playerIndex = index;
        controller = (XboxController)index + 1;
    }

    public override bool GetCancel()
    {
        return XCI.GetButton(XboxButton.B, controller);
    }

    public override bool GetConfirm()
    {
        return XCI.GetButton(XboxButton.A, controller);
    }

    public override Vector2 getMovement()
    {
        movement.x = XCI.GetAxis(XboxAxis.LeftStickX, controller) + Input.GetAxis("TurnPlayer" + playerIndex + "Keyboard");
        movement.x = Mathf.Clamp(movement.x, -1, 1);
        movement.y = XCI.GetAxis(XboxAxis.RightTrigger, controller) - XCI.GetAxis(XboxAxis.LeftTrigger, controller) + Input.GetAxis("AccelerationBrakeKeyboardPlayer" + playerIndex);
        movement.y = Mathf.Clamp(movement.y, -1, 1);
        Debug.Log(movement + ", " + XCI.GetAxis(XboxAxis.RightTrigger, controller));
        return movement;
    }

    public override bool GetStart()
    {
        return XCI.GetButton(XboxButton.Start, controller);
    }

    public override bool IsConnected()
    {
        return Input.GetJoystickNames().Length >= playerIndex && !string.IsNullOrEmpty(Input.GetJoystickNames()[playerIndex - 1]);
    }
}
