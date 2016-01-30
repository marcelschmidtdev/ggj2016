using UnityEngine;
using System.Collections;

public class PlayerJoinLeaveHandler : MonoBehaviour {

	public bool useKeyboardAsFourthController;

	void Update () {
		if (Game.Instance == null || Game.Instance.GameState != Game.GameStateId.WaitingForPlayers)
			return;
		for(int i = 0; i < 4; i++) {
			if(Game.Instance.GetPlayer( i ) == null) {
				var input = PlayerInput.GetInput( i );
				if (input.IsConnected()) {
					if (input.GetStart()) {
						Game.Instance.AddPlayer( i );
						Game.Instance.NotifyPlayerCanNotJoin( i );
					} else {
						if (!Game.Instance.CanPlayerJoin( i )) {
							Game.Instance.NotifyPlayerCanJoin( i );
						}
					}
				} else {
					if (Game.Instance.CanPlayerJoin( i )) {
						Game.Instance.NotifyPlayerCanNotJoin( i );
					}
				}
			}
			else {
				var input = PlayerInput.GetInput( i );
				if (input.IsConnected() && input.GetConfirm()) {
					Game.Instance.PlayerReady( i );
				}
				if (input.IsConnected() && input.GetCancel()) {
					Game.Instance.PlayerNotReady( i );
				}
			}
		}
	}
}
