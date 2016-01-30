using UnityEngine;
using System.Collections;

public class PlayerFactory : SingletonMonoBehaviour<PlayerFactory> {

	public enum SplitScreenMode
	{
		Single,
		Vertical,
		Horizontal,
		Quarter
	}

	public SplitScreenMode[] DefaultSplitscreenModesForPlayers;

	public int NumberOfPlayers
	{
		get; private set;
	}

	public SplitScreenMode SplitscreenMode
	{
		get
		{
			return this.DefaultSplitscreenModesForPlayers[this.NumberOfPlayers - 1];
		}
	}


	public Player PlayerPrefab;

	Player[] Players = new Player[4];

	public Player CreatePlayerInstance(int index, Vector3 position, Quaternion rotation) {
		NumberOfPlayers++;
		var newPlayer = GameObject.Instantiate<Player>( this.PlayerPrefab );
		this.Players[index] = newPlayer;
		newPlayer.InitPlayerIndex( index );
		UpdateCameraViewports();
		newPlayer.Position = position;
		newPlayer.Rotation = rotation;
		return newPlayer;
	}

	void UpdateCameraViewports () {
		var splitScreenMode = this.SplitscreenMode;
		for (int i = 0; i < 4; i++) {
			if (this.Players[i] != null) {
				this.Players[i].Camera.rect = GetViewportRect( splitScreenMode, i );
			}
		}
	}

	int getPlayerNumberForIndex(int index) {
		int result = 0;
		for(int i = 0; i < index; i++) {
			if (Players[i] != null)
				result++;
		}
		return result;
	}

	public Rect GetViewportRect (SplitScreenMode splitScreenMode, int playerIndex) {
		switch (splitScreenMode) {
			case SplitScreenMode.Single:
				switch (getPlayerNumberForIndex( playerIndex )) {
					case 0:
						return new Rect( 0.0f, 0.0f, 1.0f, 1.0f );
					default:
						return new Rect();
				}
			case SplitScreenMode.Vertical:
				switch (getPlayerNumberForIndex(playerIndex)) {
					
					case 0:
						return new Rect( 0.0f, 0.0f, 0.5f, 1.0f );
					case 1:
						return new Rect( 0.5f, 0.0f, 0.5f, 1.0f );
					default:
						return new Rect();
				}
			case SplitScreenMode.Horizontal:
				switch (getPlayerNumberForIndex( playerIndex )) {
					case 0:
						return new Rect( 0.0f, 0.5f, 1.0f, 0.5f );
					case 1:
						return new Rect( 0.0f, 0.0f, 1.0f, 0.5f );
					default:
						return new Rect();
				}
			case SplitScreenMode.Quarter:
				switch (playerIndex) {
					case 0:
						return new Rect( 0.0f, 0.5f, 0.5f, 0.5f );
					case 1:
						return new Rect( 0.5f, 0.5f, 0.5f, 0.5f );
					case 2:
						return new Rect( 0.0f, 0.0f, 0.5f, 0.5f );
					case 3:
						return new Rect( 0.5f, 0.0f, 0.5f, 0.5f );
					default:
						return new Rect();
				}
			default:
				return new Rect();
		}
	}
}
