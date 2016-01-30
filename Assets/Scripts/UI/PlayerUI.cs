using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {

	public Animator GameStateAnimator;

	public void GoToGameState(Game.GameStateId gameState) {
		this.GameStateAnimator.SetTrigger( gameState.ToString() );
	}

	public void GoToPlayerReadyState(bool isReady) {
		this.GameStateAnimator.SetBool( "isReady", isReady );
	}
}
