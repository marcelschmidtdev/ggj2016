using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class LobbyInputHandler : MonoBehaviour {

	public Lobby Lobby;

	public float MoveLeftRightThreshold = 0.5f;
	bool[] LeftThresholdReached = new bool[4];
	bool[] RightThresholdReached = new bool[4];
	bool[] ConfirmPressed = new bool[4];
	bool[] CancelPressed = new bool[4];
	bool[] StartPressed = new bool[4];

	void Update () {
		if (Lobby.GameConfig == null)
			return;
		for (int i = 0; i < 4; i++) {
			var input = PlayerInput.GetInput( i );

			bool isConnected = input.IsConnected();
			if(Lobby.IsPlayerControllerConnected( i ) != isConnected){
				if (isConnected)
					Lobby.PlayerControllerConnected( i );
				else
					Lobby.PlayerControllerDisconnected( i );
			}
			if (!isConnected)
				continue;

			var movement = input.getMovement();
			if(!this.LeftThresholdReached[i]) {
				if(movement.x < -this.MoveLeftRightThreshold) {
					Lobby.PlayerMoveLeft( i );
					this.LeftThresholdReached[i] = true;
				}
			}
			else {
				if(movement.x >= -0.35f) {
					this.LeftThresholdReached[i] = false;
				}
			}
			if (!this.RightThresholdReached[i]) {
				if (movement.x > this.MoveLeftRightThreshold) {
					Lobby.PlayerMoveRight( i );
					this.RightThresholdReached[i] = true;
				}
			}
			else {
				if (movement.x <= 0.35f) {
					this.RightThresholdReached[i] = false;
				}
			}
			
			if(this.ConfirmPressed[i] != input.GetConfirm()) {
				this.ConfirmPressed[i] = !this.ConfirmPressed[i];
				if (this.ConfirmPressed[i])
					this.Lobby.PlayerConfirmed( i );
			}

			if (this.CancelPressed[i] != input.GetCancel()) {
				this.CancelPressed[i] = !this.CancelPressed[i];
				if (this.CancelPressed[i])
					this.Lobby.PlayerCancelled( i );
			}

			if (this.StartPressed[i] != input.IsCalibrated()) {
				this.StartPressed[i] = !this.StartPressed[i];
				if (this.StartPressed[i])
					this.Lobby.PlayerPressedStart( i );
			}
		}
	}
}
