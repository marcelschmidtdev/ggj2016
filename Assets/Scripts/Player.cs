using UnityEngine;
using System.Collections;

// This is going to be on every player instance and controls stuff like input, camera etc.

public class Player : MonoBehaviour {

	public Camera Camera;
	public PlayerControls PlayerControls;
	public PlayerView PlayerView;

	public int Index
	{
		get; private set;
	}

	public void InitPlayerIndex(int index) {
		this.Index = index;
		this.PlayerControls.playerNumber = (XInputDotNetPure.PlayerIndex) index;
	}

	public Vector3 Position
	{
		get
		{
			return this.PlayerView.transform.position;
		}
		set
		{
			this.PlayerView.transform.position = value;
		}
	}
}
