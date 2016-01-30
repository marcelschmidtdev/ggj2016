using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	public Image AllowGameStartIndicator;

	void InitPlayerIndicators () {
		for(int i = 0; i < 4; i++) {
			UpdatePlayerIndicator( i );
		}
	}

	void UpdatePlayerIndicator(int index) {
		//var indicator = this.LobbyPlayerIndicators[index];
	}

	public void PlayerJoinsTeam(int playerIndex, int teamNumber) {
		var indicator = this.LobbyPlayerIndicators[playerIndex];
		GameConfig.PlayerTeamNumbers[playerIndex] = teamNumber;
		switch (teamNumber) {
			case 0:
				indicator.AllowedMovement = LobbyPlayerIndicator.Direction.Right;
				indicator.AnchorToX( 0.25f );
				indicator.AllowConfirm = true;
				indicator.AllowCancel = false;
				break;
			case -1:
				indicator.AllowedMovement = LobbyPlayerIndicator.Direction.LeftRight;
				indicator.AnchorToX( 0.5f );
				indicator.AllowConfirm = false;
				indicator.AllowCancel = false;
				break;
			case 1:
				indicator.AllowedMovement = LobbyPlayerIndicator.Direction.Left;
				indicator.AnchorToX( 0.75f );
				indicator.AllowConfirm = true;
				indicator.AllowCancel = false;
				break;
		}
		UpdateAllowGameStart();
	}

	public void PlayerMoveLeft (int playerIndex) {
		int currentIndex = GameConfig.PlayerTeamNumbers[playerIndex];
		if (currentIndex == 0)
			return;
		int targetIndex = currentIndex == 1 ? -1 : 0;
		if(this.LobbyPlayerIndicators[playerIndex].State == LobbyPlayerIndicator.LobbyPlayerState.Connected) {
			PlayerJoinsTeam( playerIndex, targetIndex );
		}
	}

	public void PlayerMoveRight (int playerIndex) {
		int currentIndex = GameConfig.PlayerTeamNumbers[playerIndex];
		if (currentIndex == 1)
			return;
		int targetIndex = currentIndex == 0 ? -1 : 1;
		if (this.LobbyPlayerIndicators[playerIndex].State == LobbyPlayerIndicator.LobbyPlayerState.Connected) {
			PlayerJoinsTeam( playerIndex, targetIndex );
		}
	}

	public void PlayerControllerConnected(int playerIndex) {
		PlayerJoinsTeam( playerIndex, -1 );
		this.LobbyPlayerIndicators[playerIndex].State = LobbyPlayerIndicator.LobbyPlayerState.Connected;
		UpdateAllowGameStart();
	}

	public void PlayerControllerDisconnected(int playerIndex) {
		PlayerJoinsTeam( playerIndex, -1 );
		this.LobbyPlayerIndicators[playerIndex].State = LobbyPlayerIndicator.LobbyPlayerState.NotConnected;
		UpdateAllowGameStart();
	}

	public bool IsPlayerControllerConnected(int playerIndex) {
		return this.LobbyPlayerIndicators[playerIndex].State != LobbyPlayerIndicator.LobbyPlayerState.NotConnected;
	}

	public void PlayerConfirmed(int playerIndex) {
		if(GameConfig.PlayerTeamNumbers[playerIndex] != -1) {
			this.LobbyPlayerIndicators[playerIndex].State = LobbyPlayerIndicator.LobbyPlayerState.Locked;
			this.LobbyPlayerIndicators[playerIndex].AllowConfirm = false;
			this.LobbyPlayerIndicators[playerIndex].AllowCancel = true;
		}
		UpdateAllowGameStart();
	}

	public void PlayerCancelled (int playerIndex) {
		if(this.LobbyPlayerIndicators[playerIndex].State == LobbyPlayerIndicator.LobbyPlayerState.Locked && GameConfig.PlayerTeamNumbers[playerIndex] != -1) {
			this.LobbyPlayerIndicators[playerIndex].State = LobbyPlayerIndicator.LobbyPlayerState.Connected;
			this.LobbyPlayerIndicators[playerIndex].AllowCancel = false;
			this.LobbyPlayerIndicators[playerIndex].AllowConfirm = true;
		}
		UpdateAllowGameStart();
	}

	public void PlayerPressedStart (int playerIndex) {
		if (this.AllowGameStart) {
			Application.LoadLevel( "DefaultMap" );
		}
	}

	bool AllowGameStart
	{
		get
		{
			int numberOfReadyPlayers = 0;
			for(int i = 0; i < 4; i++) {
				if(GameConfig.PlayerTeamNumbers[i] != -1) {
					if (this.LobbyPlayerIndicators[i].State == LobbyPlayerIndicator.LobbyPlayerState.Connected)
						return false; // unconfirmed player in team slot
					else if (this.LobbyPlayerIndicators[i].State == LobbyPlayerIndicator.LobbyPlayerState.Locked)
						numberOfReadyPlayers++;
				}
			}
			return numberOfReadyPlayers > 0;
		}
	}

	void UpdateAllowGameStart () {
		if (this.AllowGameStart)
			this.AllowGameStartIndicator.color = Color.white;
		else
			this.AllowGameStartIndicator.color = new Color();
	}
}
