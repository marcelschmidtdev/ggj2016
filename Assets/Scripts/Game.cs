using UnityEngine;
using System.Collections;
using System;

public class Game : SingletonMonoBehaviour<Game> {

	public Action EventGameStarted;
	public Action<Player> EventPlayerJoined;
}
