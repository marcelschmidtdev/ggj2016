using UnityEngine;
using System.Collections;
using System;

// This is going to control the game (like spawns, win conditions, joins, leaves, etc.)

public class Game : SingletonMonoBehaviour<Game> {

	public float CountdownLength = 4.0f;

	public enum GameStateId
	{
		WaitingForPlayers,
		Countdown,
		Playing,
		Ending
	}

	GameStateId _GameState;
	public GameStateId GameState
	{
		get
		{
			return _GameState;
		}
		private set
		{
			if(_GameState != value) {
				_GameState = value;
				EventGameStateChanged( value );
			}
		}
	}

	public int NumberOfPlayers
	{
		get; private set;
	}

	Player[] Players = new Player[4];
	float RemainingCountdown = 0.0f;

	public event Action<GameStateId> EventGameStateChanged = (newState) => { };
	public event Action<int, bool> EventPlayerCanJoinChanged = (index, canJoin) => { };
	public event Action<Player> EventPlayerJoined = (player) => { };
	public event Action<Player, bool> EventPlayerReadyChanged = (player, isReady) => { };
	public Action<int> EventPlayerScored = (playerId) => { };
	public Action<int, bool> EventPlayerKilledMinion = (playerId, ownMinion) => { };

	void Start () {
		_GameState = GameStateId.WaitingForPlayers;
	}

	void Update () {
		switch (this.GameState) {
			case GameStateId.Countdown:
				this.RemainingCountdown -= Time.deltaTime;
				if(this.RemainingCountdown <= 0.0f) {
					this.GameState = GameStateId.Playing;
				}
				break;
		}
	}

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
		EventPlayerJoined( newPlayer );
	}
	
	public void PlayerNotReady (int index) {
		if (this.GameState != GameStateId.WaitingForPlayers)
			return;
		this.Players[index].PlayerReady = false;
		EventPlayerReadyChanged( this.Players[index], false );
	}

	public void PlayerReady(int index) {
		if (this.GameState != GameStateId.WaitingForPlayers)
			return;
		this.Players[index].PlayerReady = true;
		EventPlayerReadyChanged( this.Players[index], true );
		CheckIfAllPlayersAreReady();
	}

	bool[] PlayerCanJoin = new bool[4];
	public bool CanPlayerJoin(int index) {
		return PlayerCanJoin[index];
	}

	public void NotifyPlayerCanJoin(int index) {
		PlayerCanJoin[index] = true;
		EventPlayerCanJoinChanged( index, true );
	}

	public void NotifyPlayerCanNotJoin (int index) {
		PlayerCanJoin[index] = false;
		EventPlayerCanJoinChanged( index, false );
	}

	bool AllPlayersReady
	{
		get
		{
			if (this.NumberOfPlayers == 0)
				return false;
			for(int i = 0; i < 4; i++) {
				var player = this.Players[i];
				if (player != null && !player.PlayerReady)
					return false;
			}
			return true;
		}
	}

	void CheckIfAllPlayersAreReady () {
		if (AllPlayersReady) {
			this.RemainingCountdown = this.CountdownLength;
			this.GameState = GameStateId.Countdown;
		}
	}
}
