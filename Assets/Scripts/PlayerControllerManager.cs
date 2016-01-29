using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerControllerManager : MonoBehaviour {

	GamePadState[] GamePadStates = new GamePadState[4];

	void Update () {
		for(int i = 0; i < 4; i++) {
			var oldState = GamePadStates[i];
			var newState = GamePad.GetState( (PlayerIndex) i );

			if(!oldState.IsConnected) {
				if (newState.IsConnected) {
					Debug.Log( "Controller[" + i + "] added." );
				}
			}
			else {
				if (!newState.IsConnected) {
					Debug.Log( "Controller[" + i + "] dropped." );
				}
			}

			GamePadStates[i] = newState;
		}
	}

	[ContextMenu("PrintJoystickNames")]
	void PrintJoystickNames () {
		var joystickNames = Input.GetJoystickNames();

		Debug.Log( "Joysticks:" );
		for(int i = 0; i < joystickNames.Length; i++) {
			if(joystickNames[i] == null) {
				Debug.Log( "[" + i + "]: " + "null" );
			} else {
				Debug.Log( "[" + i + "]: " + joystickNames[i] );
			}
		}
	}
}
