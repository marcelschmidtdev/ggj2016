using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class LobbyInputHandler : MonoBehaviour {

	public Lobby Lobby;

	void Update () {
		if (Lobby.GameConfig == null)
			return;
		for (int i = 0; i < 4; i++) {
			if (Game.Instance.GetPlayer( i ) == null) {
				var gamePadState = GamePad.GetState( (PlayerIndex)i );
				if (gamePadState.IsConnected) {
					if (gamePadState.Buttons.Start == ButtonState.Pressed) {
						Game.Instance.AddPlayer( i );
						Game.Instance.NotifyPlayerCanNotJoin( i );
					}
					else {
						if (!Game.Instance.CanPlayerJoin( i )) {
							Game.Instance.NotifyPlayerCanJoin( i );
						}
					}
				}
				else {
					if (Game.Instance.CanPlayerJoin( i )) {
						Game.Instance.NotifyPlayerCanNotJoin( i );
					}
				}
			}
			else {
				var gamePadState = GamePad.GetState( (PlayerIndex)i );
				if (gamePadState.IsConnected && gamePadState.Buttons.A == ButtonState.Pressed) {
					Game.Instance.PlayerReady( i );
				}
				if (gamePadState.IsConnected && gamePadState.Buttons.B == ButtonState.Pressed) {
					Game.Instance.PlayerNotReady( i );
				}
			}
		}
	}
}
