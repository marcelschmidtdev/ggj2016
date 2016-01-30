using UnityEngine;
using System.Collections;

public class GameConfiguration
{
	public int[] PlayerTeamNumbers;

	public GameConfiguration () {
		PlayerTeamNumbers = new int[4];

		for(int i = 0; i < 4; i++) {
			PlayerTeamNumbers[i] = -1;
		}
	}
}

public class Lobby : MonoBehaviour {

	public static GameConfiguration GameConfig;

	void Start () {
		GameConfig = new GameConfiguration();
		InitPlayerIndicators();
	}

	public LobbyPlayerIndicator[] LobbyPlayerIndicators;

	void InitPlayerIndicators () {
		for(int i = 0; i < 4; i++) {
			UpdatePlayerIndicator( i );
		}
	}

	void UpdatePlayerIndicator(int index) {
		var indicator = this.LobbyPlayerIndicators[index];
	}

	public void PlayerJoinsTeam(int playerIndex, int teamNumber) {
		var indicator = this.LobbyPlayerIndicators[playerIndex];
		GameConfig.PlayerTeamNumbers[playerIndex] = teamNumber;
		switch (teamNumber) {
			case -1:
				indicator.AllowedMovement = LobbyPlayerIndicator.Direction.LeftRight;
				indicator.AnchorToX( 0.5f );
				break;
			case 0:
				indicator.AllowedMovement = LobbyPlayerIndicator.Direction.Right;
				indicator.AnchorToX( 0.25f );
				break;
			case 1:
				indicator.AllowedMovement = LobbyPlayerIndicator.Direction.Left;
				indicator.AnchorToX( 0.75f );
				break;
		}
	}
}
