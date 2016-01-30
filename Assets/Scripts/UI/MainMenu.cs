using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void Update () {
		if (Input.anyKeyDown) {
			StartNewGame();
		}
	}

	void StartNewGame () {
		Application.LoadLevel( "Lobby" );
	}
}
