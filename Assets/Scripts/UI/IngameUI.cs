using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour {

	public Animator GameStateAnimator;
	public Animator[] PlayerCanJoinAnimators;
	public Text[] TeamScoreLabels;
	public Slider[] TeamScoreBars;

	public PlayerUI PlayerUIPrefab;

	PlayerUI[] PlayerUIs = new PlayerUI[4];

	void Start () {
		Game.Instance.EventPlayerJoined += Instance_EventPlayerJoined;
		Game.Instance.EventGameStateChanged += HandleGameStateChange;
		HandleGameStateChange( Game.Instance.GameState);
	}

	void OnDestroy () {
		if (Game.Instance == null)
			return;
		Game.Instance.EventPlayerJoined -= Instance_EventPlayerJoined;
		Game.Instance.EventGameStateChanged -= HandleGameStateChange;
	}

	private void Instance_EventPlayerCanJoinChanged (int index, bool canJoin) {
		this.PlayerCanJoinAnimators[index].SetBool( "isActive", canJoin );
	}

	private void Instance_EventPlayerJoined (Player player) {
		PlayerUI uiInstance = GameObject.Instantiate<PlayerUI>( this.PlayerUIPrefab );
		uiInstance.transform.SetParent( transform, false );
		this.PlayerUIs[player.Index] = uiInstance;
		uiInstance.GoToGameState( Game.Instance.GameState );
		switch (Game.Instance.GameState) {
			case Game.GameStateId.WaitingForPlayers:
				uiInstance.GoToPlayerReadyState( player.PlayerReady );
				break;
		}

		UpdateUIScales();
	}

	// kinda hacky. Should move viewport logic from playerfactory to another class
	void UpdateUIScales () {
		var splitScreenMode = PlayerFactory.Instance.SplitscreenMode;
		for (int i = 0; i < 4; i++) {
			if (this.PlayerUIs[i] != null) {
				var rectTransform = this.PlayerUIs[i].transform as RectTransform;
				var viewportRect = PlayerFactory.Instance.GetViewportRect( splitScreenMode, i );
				rectTransform.anchorMin = new Vector2( viewportRect.xMin, viewportRect.yMin );
				rectTransform.anchorMax = new Vector2( viewportRect.xMax, viewportRect.yMax );
			}
		}
	}

	private void HandleGameStateChange(Game.GameStateId newState) {
		this.GameStateAnimator.SetTrigger( newState.ToString() );
		foreach(var playerUI in this.PlayerUIs) {
			if(playerUI != null) {
				playerUI.GoToGameState( newState );
			}
		}
		
		if(newState == Game.GameStateId.WaitingForPlayers) {
			Game.Instance.EventPlayerCanJoinChanged += Instance_EventPlayerCanJoinChanged;
			Game.Instance.EventPlayerReadyChanged += Instance_EventPlayerReadyChanged;
			for (int i = 0; i < 4; i++) {
				Instance_EventPlayerCanJoinChanged( i, Game.Instance.CanPlayerJoin( i ) );
			}
		}
		else {
			Game.Instance.EventPlayerReadyChanged -= Instance_EventPlayerReadyChanged;
			Game.Instance.EventPlayerCanJoinChanged -= Instance_EventPlayerCanJoinChanged;
		}

		if (newState == Game.GameStateId.Playing) {
			Game.Instance.EventPlayerScored += Instance_EventPlayerScored;
		}
		else {
			Game.Instance.EventPlayerScored -= Instance_EventPlayerScored;
		}
	}

	private void Instance_EventPlayerReadyChanged (Player player, bool isReady) {
		if (this.PlayerUIs[player.Index] != null)
			this.PlayerUIs[player.Index].GoToPlayerReadyState( isReady );
	}

	private void Instance_EventPlayerScored(int playerIndex) {
		for(int i = 0; i < 2; i++) {
			this.TeamScoreLabels[i].text = Game.Instance.GetTeamScore( i ).ToString();
			this.TeamScoreBars[i].value = Game.Instance.GetTeamScore( i ) / Game.Instance.TargetScore;
		}
	}
}
