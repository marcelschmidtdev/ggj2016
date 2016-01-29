using UnityEngine;
using System.Collections;
using System;

public class Game : SingletonMonoBehaviour<Game> {

	public event Action EventGameStarted;
	public event Action EventGameOver;
	public event Action<Player> EventPlayerJoined;
}
