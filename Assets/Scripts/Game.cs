using UnityEngine;
using System.Collections;
using System;

// This is going to control the game (like spawns, win conditions, joins, leaves, etc.)

public class Game : SingletonMonoBehaviour<Game> {

	public Action EventGameStarted;
	public Action<Player> EventPlayerJoined;
}
