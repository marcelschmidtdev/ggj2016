using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Animator GameStateAnimator;
	public Text ComboLabel;
	public Text ComboResultLabel;
	public Animator ComboAnimator;
	public Animator ComboResultAnimator;

	int lastComboCount = 0;

	public void GoToGameState(Game.GameStateId gameState) {
		this.GameStateAnimator.SetTrigger( gameState.ToString() );
	}

	public void GoToPlayerReadyState(bool isReady) {
		this.GameStateAnimator.SetBool( "isReady", isReady );
	}

	public void ShowCombo(int comboCount) {
		if(comboCount == 0 && this.lastComboCount > 0) {
			this.ComboResultLabel.text = lastComboCount > 1 ? "COMBO "+lastComboCount.ToString()+"x" : string.Empty;
			this.ComboResultAnimator.SetInteger( "ComboCount", lastComboCount );
			this.ComboResultAnimator.SetTrigger( "ShowResult" );
		}
		this.ComboLabel.text = comboCount > 1 ? comboCount.ToString()+"x" : string.Empty;
		this.ComboAnimator.SetInteger( "ComboCount", comboCount );
		this.ComboAnimator.SetTrigger( "UpdateComboCount" );
		this.lastComboCount = comboCount;
	}
}
