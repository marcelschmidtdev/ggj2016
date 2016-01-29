using UnityEngine;
using System.Collections;
using System;

// This is going to control the game (like spawns, win conditions, joins, leaves, etc.)

public class Game : SingletonMonoBehaviour<Game> {

	public enum SplitScreenMode {
		Single,
		Vertical,
		Horizontal,
		Quarter
	}

	public SplitScreenMode[] DefaultSplitscreenModesForPlayers;

	public SplitScreenMode SplitscreenMode
	{
		get
		{
			return this.DefaultSplitscreenModesForPlayers[this.NumberOfPlayers-1];
		}
	}

	public int NumberOfPlayers
	{
		get; private set;
	}

	Player[] Players = new Player[4];

	public Action EventGameStarted;
	public Action<Player> EventPlayerJoined;
	public Player PlayerPrefab;

	[ContextMenu("AddPlayer")]
	public void AddPlayerAtFirstAvailableSpot () {
		for(int i = 0; i < 4; i++) {
			if(this.Players[i] == null) {
				AddPlayer( i );
				return;
			}
		}
	}

	public void AddPlayer (int index) {
		NumberOfPlayers++;
		var newPlayer = GameObject.Instantiate<Player>( this.PlayerPrefab );
		this.Players[index] = newPlayer;
		UpdateCameraViewports();
	}

	void UpdateCameraViewports () {
		var splitScreenMode = this.SplitscreenMode;
		for(int i = 0; i < 4; i++) {
			if(this.Players[i] != null) {
				this.Players[i].Camera.rect = GetViewportRect( splitScreenMode, i );
			}
		}
	}

	Rect GetViewportRect(SplitScreenMode splitScreenMode, int playerIndex) {
		switch (splitScreenMode) {
			case SplitScreenMode.Single:
				switch (playerIndex) {
					case 0:
						return new Rect( 0.0f, 0.0f, 1.0f, 1.0f );
					default:
						return new Rect();
				}
			case SplitScreenMode.Vertical:
				switch (playerIndex) {
					case 0:
						return new Rect( 0.0f, 0.0f, 0.5f, 1.0f );
					case 1:
						return new Rect( 0.5f, 0.0f, 0.5f, 1.0f );
					default:
						return new Rect();
				}
			case SplitScreenMode.Horizontal:
				switch (playerIndex) {
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
