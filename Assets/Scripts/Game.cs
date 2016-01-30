using UnityEngine;
using System.Collections;
using System;

// This is going to control the game (like spawns, win conditions, joins, leaves, etc.)

public class Game : SingletonMonoBehaviour<Game> {
	public int NumberOfPlayers
	{
		get; private set;
	}

	Player[] Players = new Player[4];

	public event Action EventGameStarted;
	public event Action EventGameOver;
	public event Action<Player> EventPlayerJoined;

	[ContextMenu("AddPlayer")]
	public void AddPlayerAtFirstAvailableSpot () {
		for(int i = 0; i < 4; i++) {
			if(this.Players[i] == null) {
				AddPlayer( i );
				return;
			}
		}
	}

	public Player GetPlayer (int index) {
		return Players[index];
	}

	public void AddPlayer (int index) {
		var playerSpawner = Map.Instance.PlayerSpawners[index];
		if (playerSpawner == null) {
			Debug.LogError( "Could not find matching spawner for player index " + index );
			return;
		}
		NumberOfPlayers++;
		var newPlayer = PlayerFactory.Instance.CreatePlayerInstance( index, playerSpawner.Position, playerSpawner.Rotation );
		this.Players[index] = newPlayer;
	}
}
