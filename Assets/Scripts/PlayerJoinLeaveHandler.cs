using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerJoinLeaveHandler : MonoBehaviour {

	void Update () {
		if (Game.Instance == null)
			return;
		for(int i = 0; i < 4; i++) {
			if(Game.Instance.GetPlayer( i ) == null) {
				var gamePadState = GamePad.GetState( (PlayerIndex)i );
				if (gamePadState.IsConnected && gamePadState.Buttons.Start == ButtonState.Pressed) {
					Game.Instance.AddPlayer( i );
				}
			}
		}
	}
}
