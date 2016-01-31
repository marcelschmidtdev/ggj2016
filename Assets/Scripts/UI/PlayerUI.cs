using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Animator GameStateAnimator;
	public Text ComboLabel;

	public void GoToGameState(Game.GameStateId gameState) {
		this.GameStateAnimator.SetTrigger( gameState.ToString() );
	}

	public void GoToPlayerReadyState(bool isReady) {
		this.GameStateAnimator.SetBool( "isReady", isReady );
	}

	public void ShowCombo(int comboCount) {
		this.ComboLabel.text = comboCount > 1 ? comboCount.ToString()+"x" : string.Empty;
	}
}
