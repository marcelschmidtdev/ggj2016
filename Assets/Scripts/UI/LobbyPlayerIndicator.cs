using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyPlayerIndicator : MonoBehaviour {


	public Color NotConnectedControllerColor;
	public Color ConnectedControllerColor;
	public Color LockedControllerColor;

	public Image ControllerSprite;
	public Image LeftArrowSprite;
	public Image RightArrowSprite;
	public Image ConfirmSprite;
	public Image CancelSprite;
	public Image Keyboard; 

	public enum LobbyPlayerState
	{
		NotConnected,
		Connected,
		Locked
	}

	public enum Direction
	{
		None,
		Left,
		Right,
		LeftRight
	}

	LobbyPlayerState _State;
	public LobbyPlayerState State
	{
		get { return _State; }
		set
		{
			this._State = value;
			UpdateView();
		}
	}

	Direction _AllowedMovement;
	public Direction AllowedMovement
	{
		get { return _AllowedMovement; }
		set
		{
			this._AllowedMovement = value;
			UpdateView();
		}
	}

	bool _AllowConfirm;
	public bool AllowConfirm
	{
		get
		{
			return _AllowConfirm;
		}
		set
		{
			this._AllowConfirm = value;
			UpdateView();
		}
	}

	bool _AllowCancel;
	public bool AllowCancel
	{
		get
		{
			return _AllowCancel;
		}
		set
		{
			this._AllowCancel = value;
			UpdateView();
		}
	}

	public void SetToKeyBoard ()
	{
		this.ControllerSprite.sprite = this.Keyboard.sprite; 
	}
	void OnEnable () {
		UpdateView();
	}

	void UpdateView () {
		switch (_State) {
			case LobbyPlayerState.NotConnected:
				this.ControllerSprite.color = this.NotConnectedControllerColor;
				this.LeftArrowSprite.color = new Color();
				this.RightArrowSprite.color = new Color();
				break;
			case LobbyPlayerState.Connected:
				this.ControllerSprite.color = this.ConnectedControllerColor;
				switch (this._AllowedMovement) {
					case Direction.None:
						this.LeftArrowSprite.color = new Color();
						this.RightArrowSprite.color = new Color();
						break;
					case Direction.Left:
						this.LeftArrowSprite.color = Color.white;
						this.RightArrowSprite.color = new Color();
						break;
					case Direction.Right:
						this.LeftArrowSprite.color = new Color();
						this.RightArrowSprite.color = Color.white;
						break;
					case Direction.LeftRight:
						this.LeftArrowSprite.color = Color.white;
						this.RightArrowSprite.color = Color.white;
						break;
				}
				break;
			case LobbyPlayerState.Locked:
				this.ControllerSprite.color = this.LockedControllerColor;
				this.LeftArrowSprite.color = new Color();
				this.RightArrowSprite.color = new Color();
				break;
		}

		if (this.AllowConfirm) {
			this.ConfirmSprite.color = Color.white;
		}
		else {
			this.ConfirmSprite.color = new Color();
		}

		if (this.AllowCancel) {
			this.CancelSprite.color = Color.white;
		}
		else {
			this.CancelSprite.color = new Color();
		}
	}

	public void AnchorToX (float x) {
		var rectTransform = transform as RectTransform;
		rectTransform.anchorMin = new Vector2( x, rectTransform.anchorMin.y );
		rectTransform.anchorMax = new Vector2( x, rectTransform.anchorMax.y );
	}

	public void AnchorTo(float x, float y) {
		var rectTransform = transform as RectTransform;
		rectTransform.anchorMin = new Vector2( x, y );
		rectTransform.anchorMax = new Vector2( x, y );
	}
}
